# 10 - Strings and Text in C#

## What is a string?

Text in C# is commonly represented by `string`, an alias for the .NET type `System.String`. A string is a reference type containing a sequence of UTF-16 code units. It can represent an empty sequence, but it cannot be modified after it has been created: strings are **immutable**.

```csharp
string name = "Ada";
string greeting = "Hello, " + name;
```

The expression creates a greeting value; it does not alter the characters in `name`. Immutability makes strings predictable to share between methods. Operations such as concatenation, replacing, trimming, or changing case return another string value.

## String literal forms

### Regular quoted literal

Escape sequences express characters that are awkward to type directly:

```csharp
string line = "First line\nSecond line";
string quote = "She said, \"Hello.\"";
string folder = "C:\\Users\\Student";
```

Common escapes include `\n` newline, `\r` carriage return, `\t` tab, `\\` backslash, `\"` quote, and `\u`/`\U` Unicode escapes.

### Verbatim literal

Prefix a string with `@` to treat backslashes literally. Double a quote to put one inside the literal:

```csharp
string windowsPath = @"C:\Users\Student\Documents";
string quoted = @"The word ""C#"" is quoted.";
```

### Raw string literal

Modern C# supports raw string literals delimited by three or more quotes. They reduce escaping in text such as JSON, regular expressions, or multiline templates:

```csharp
string json = """
    {
            "name": "Ada"
    }
    """;
```

For JSON specifically, use a JSON serializer in application code rather than manually assembling JSON text. Raw literals are syntax for writing the literal, not a JSON parser/serializer.

### Interpolated strings

Prefix with `$` to embed expressions in braces:

```csharp
string message = $"Hello, {name}!";
string summary = $"{name} has {3 + 2} tasks.";
```

Use interpolation for readable formatting. If literal braces are needed in an interpolated string, escape them by doubling: `{{` and `}}`.

## Immutability, concatenation, and allocation

Assigning a string variable copies a reference to an immutable string value. Operations that produce modified text return new string values. The runtime may share identical literals through interning, but program correctness must not rely on two equal strings being the same object reference.

For a few known pieces, `+` or interpolation is clear and usually optimized well:

```csharp
string fullName = firstName + " " + lastName;
string label = $"Item {number}: {name}";
```

Repeated concatenation in a large loop can repeatedly create intermediate strings. Use `StringBuilder` when text is assembled incrementally:

```csharp
using System.Text;

var builder = new StringBuilder();
foreach (string item in items)
{
    builder.Append(item).AppendLine();
}
string output = builder.ToString();
```

`StringBuilder` is mutable while building and produces a string from `ToString()`. Do not use it automatically for every interpolation; clarity and measurement matter. It is not generally thread-safe for concurrent mutation.

## Comparing strings: ordinal or linguistic?

The string `==` operator compares string contents using ordinal equality; it does not compare whether two references point to the same object. To make comparison intent explicit, use an overload that accepts `StringComparison`:

```csharp
bool sameIdentifier = string.Equals(
    input,
    expected,
    StringComparison.OrdinalIgnoreCase);
```

Common choices:

- `Ordinal`: exact code-unit-based comparison; suitable for programmatic identifiers and protocol tokens.
- `OrdinalIgnoreCase`: case-insensitive identifier comparison using ordinal rules.
- `CurrentCulture`: linguistic comparison using the current culture; suitable for user-facing language sorting/search behavior where culture is intended.
- `InvariantCulture`: culture-aware comparison using invariant rules, useful for some stable linguistic scenarios but not a replacement for ordinal comparison of identifiers.

Use `StringComparer` when a collection needs a comparison rule:

```csharp
var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
names.Add("admin");
bool contains = names.Contains("ADMIN"); // true
```

Avoid culture-sensitive comparison for machine identifiers, security checks, file/protocol keys, and case-insensitive tokens. Different cultures can have different casing and sorting rules. For human-language text, choose culture-aware rules deliberately rather than assuming ordinal order is natural-language order.

## Searching, splitting, trimming, and replacing

Common APIs include:

- `Contains(value, comparisonType)`: check for a substring with explicit comparison rules.
- `StartsWith` / `EndsWith`: check prefixes/suffixes.
- `IndexOf`: find a position, or `-1` when absent.
- `Substring` or range slicing: obtain a portion as a new string.
- `Split`: break text into pieces.
- `Trim`, `TrimStart`, `TrimEnd`: remove surrounding characters/whitespace.
- `Replace`: return a string with matching text replaced.
- `Join`: combine values using a separator.

Many search APIs have overloads with `StringComparison`; choose the intended behavior. `Trim()` removes whitespace at the ends, not from the middle. `Split` can allocate many strings, so streaming parsing may be better for large input. Validate and normalize user input according to the exact business rule rather than applying broad transformations blindly.

## Formatting and culture

Formatting turns values into text. Culture affects decimal separators, date patterns, currency symbols, and other conventions:

```csharp
using System.Globalization;

decimal amount = 1234.5m;
string display = amount.ToString("C", CultureInfo.CurrentCulture);
string protocol = amount.ToString(CultureInfo.InvariantCulture);
```

Use current/user culture for UI display where appropriate. Use a documented invariant or explicit format for machine-readable output. A localized display string is not automatically a stable serialization format.

Interpolation can specify format strings and providers:

```csharp
string countText = $"Count: {count:N0}";
string stable = amount.ToString("0.00", CultureInfo.InvariantCulture);
```

Formatting numeric data to two decimal places controls the displayed representation; it does not necessarily define the numeric rounding/storage policy.

Do not construct commands for another language/system by concatenating untrusted text. Use that system's parameterized/structured API and correct escaping. String interpolation makes text construction readable, but it does not automatically make inserted data safe for SQL, shell commands, HTML, or other contexts.

## Null, empty, and whitespace are different

- `null`: there is no string reference/value.
- `""`: a valid string with zero characters.
- `"   "`: a string containing whitespace characters.

Choose the check that matches the rule:

```csharp
string? value = Console.ReadLine();
bool missing = string.IsNullOrEmpty(value);
bool blankOrMissing = string.IsNullOrWhiteSpace(value);
```

Nullable reference annotations such as `string?` communicate possible null to compiler analysis but do not change the runtime representation. Calling an instance member on null still fails. Validate at the boundary where input arrives.

## Unicode: code units versus characters people see

`string.Length` counts UTF-16 **code units**. A `char` is one 16-bit code unit, not necessarily one complete Unicode character. Some Unicode scalar values use a surrogate pair (two `char` values). A user-perceived character, or **grapheme cluster**, can contain multiple Unicode scalar values, such as a base letter plus a combining accent or a multi-code-point emoji sequence.

```csharp
string emoji = "\U0001F600";
Console.WriteLine(emoji.Length); // 2 UTF-16 code units.
```

Use `System.Text.Rune` when processing Unicode scalar values, and `StringInfo` text-element APIs when the requirement is closer to user-perceived text elements. Even then, user-interface rules for cursor movement and counting can have product-specific details.

Unicode can represent visually equivalent text using different sequences. For example, an accented character may be one precomposed scalar or a base letter plus a combining mark. If equality/search requirements consider those equivalent, normalization such as `string.Normalize()` may be needed at a clearly defined boundary. Normalization is not a substitute for input validation or culture-aware comparison.

## Advanced: `ReadOnlySpan<char>`

`ReadOnlySpan<char>` is a view over a contiguous region of characters. It can let parsing code inspect a slice without first allocating a substring. Spans are `ref struct` values with lifetime restrictions: they cannot be stored in ordinary heap fields or used across `await` in the general case. Use span-based APIs when performance or API design warrants them; ordinary string methods are often easier to read.

## Complete runnable example

This program demonstrates immutability, explicit identifier comparison, culture-stable formatting, null/blank checks, Unicode length, and incremental construction:

```csharp
using System;
using System.Globalization;
using System.Text;

internal class Program
{
    private static void Main()
    {
        string original = "Ada";
        string upper = original.ToUpperInvariant();
        Console.WriteLine($"Original: {original}; transformed: {upper}");

        bool sameId = string.Equals("Admin", "ADMIN", StringComparison.OrdinalIgnoreCase);
        Console.WriteLine($"Same identifier: {sameId}");

        decimal amount = 1234.5m;
        Console.WriteLine(amount.ToString("C", CultureInfo.InvariantCulture));

        string? blank = "   ";
        Console.WriteLine($"Blank input: {string.IsNullOrWhiteSpace(blank)}");

        string emoji = "\U0001F600";
        Console.WriteLine($"Emoji UTF-16 length: {emoji.Length}");

        var builder = new StringBuilder();
        builder.AppendLine("Modules:");
        builder.AppendLine("- Namespaces");
        builder.AppendLine("- Enumerations");
        Console.Write(builder.ToString());
    }
}
```

Expected output (currency display may use a different format if you replace the invariant culture):

```text
Original: Ada; transformed: ADA
Same identifier: True
¤1,234.50
Blank input: True
Emoji UTF-16 length: 2
Modules:
- Namespaces
- Enumerations
```

The `¤` symbol represents the invariant-culture currency symbol; the important lesson is that culture affects formatting. `original` remains `Ada` after creating `upper`, demonstrating that string operations return values rather than mutating the original text.

## Common mistakes

- Assuming a string operation changes the original string in place.
- Using current-culture comparison for machine identifiers or security decisions.
- Assuming `Length` means visible characters.
- Treating null, empty, and whitespace as equivalent without checking the requirement.
- Using `StringBuilder` for every short concatenation or never using it for large incremental construction.
- Assuming interpolation safely escapes text for every output context.
- Assuming formatting output is culture-independent.
- Assuming two visually identical Unicode strings have identical code-unit sequences.
- Comparing strings by reference identity when content equality is intended.

## Interview questions with answers

### Why are strings immutable?

Their contents cannot be changed after creation. This makes sharing predictable and supports runtime optimizations; operations that produce changed text return another string value.

### What does `==` do for strings?

It compares string contents using ordinal equality, not whether the variables refer to the same object instance.

### When should I use `StringBuilder`?

When many pieces are appended iteratively and a mutable builder improves the natural design or avoids repeated intermediate strings. For simple interpolation/concatenation, ordinary strings are clearer.

### What is the difference between ordinal and culture-aware comparison?

Ordinal comparison uses code-unit-based rules and is appropriate for machine identifiers. Culture-aware comparison uses linguistic rules for a culture and is appropriate for selected user-facing text behavior.

### Does `string.Length` count visible characters?

No. It counts UTF-16 code units. A surrogate pair has length two, and a grapheme cluster can contain multiple code units/scalars.

### Why can interpolation be unsafe?

Interpolation only inserts text. It does not apply context-specific escaping or parameterization for HTML, shells, query languages, or other formats.

### What is the difference between null, empty, and whitespace?

Null means no string value, empty means zero characters, and whitespace contains one or more whitespace characters. Choose `IsNullOrEmpty` or `IsNullOrWhiteSpace` according to the input rule.

## Practice

1. Concatenate a name and observe that the original variable remains unchanged.
2. Compare identifiers using `OrdinalIgnoreCase`; then explain when current-culture comparison is appropriate.
3. Format a number under invariant and current cultures and compare the separators/symbols.
4. Inspect the `Length` of an emoji and a decomposed accented character; explain code units versus text elements.
5. Build a report with many lines using `StringBuilder`, then explain why a short interpolated string does not need one.
6. Validate null, empty, and whitespace inputs using separate rules.

## Mental model

A string is immutable UTF-16 text. Pick comparison rules based on whether text is an identifier or human language; pick formatting culture based on whether output is for a person or a stable contract. Treat nullability, Unicode text elements, and context-specific output safety as separate concerns.
