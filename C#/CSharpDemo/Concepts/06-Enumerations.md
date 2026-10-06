# 06 - Enumerations in C#

## Why have enums?

Programs often need to represent one choice from a small, known set: a workflow state, a menu action, a direction, or a display mode. One approach is to use unexplained integers (`0`, `1`, `2`) or arbitrary strings (`"Ready"`, `"Running"`). Both make mistakes easy: numbers hide intent, and strings can be misspelled.

An **enumeration**, written with the `enum` keyword, defines named constants backed by an integral value. It makes the intended choices visible in code and lets the compiler check many accidental mismatches.

```csharp
enum RunState
{
    Unknown = 0,
    Ready = 1,
    Running = 2,
    Finished = 3
}

RunState state = RunState.Ready;
```

`RunState` is a type. `RunState.Ready` is one named constant of that type. An enum is a value type, not a collection and not a string.

## The underlying integral type

Unless specified, an enum's underlying type is `int`. C# permits these integral types as an enum's underlying type: `byte`, `sbyte`, `short`, `ushort`, `int`, `uint`, `long`, and `ulong`.

```csharp
enum HttpLikeCode : ushort
{
    Unknown = 0,
    Accepted = 200,
    Rejected = 400
}
```

Choose a non-default underlying type only when its range or an external binary/data contract requires it. Smaller is not automatically better; choosing the wrong type can constrain future values. The enum member values must fit in the selected type.

## Values: implicit numbering, explicit contracts, and aliases

If you omit a member's value, the first member receives zero and later members receive the previous value plus one:

```csharp
enum Priority
{
    Low,    // 0
    Normal, // 1
    High    // 2
}
```

You can assign explicit values. Two members can even have the same value, which is called an alias:

```csharp
enum OperationResult
{
    Success = 0,
    Completed = 0,
    InvalidInput = 10,
    Failed = 20
}
```

Aliases can be useful for compatibility or terminology, but reverse conversion from a value to a name may select one name rather than preserving which alias a caller wrote. Do not use aliases when the distinct names are meant to represent different states.

If numeric values are written to a file, transmitted through a protocol, or otherwise persisted, those numbers become a compatibility contract. Reordering implicit members can silently change meaning. Assign explicit stable values and do not casually reuse a retired number.

## The zero/default value

The default of every enum is the value zero, whether or not a member named zero exists. This matters because fields and array elements receive default values automatically, and `default(MyEnum)` is zero.

```csharp
enum JobState
{
    Pending = 1,
    Running = 2,
    Complete = 3
}

JobState state = default;
Console.WriteLine((int)state); // 0, but no declared member names it.
```

Prefer a meaningful zero member such as `Unknown`, `None`, or `Unspecified` when the enum can be default-initialized. That does not remove the need to validate external input: arbitrary integral values can still be converted to an enum.

## Conversions and validation

An enum can be explicitly converted to its underlying integer type. An integer can also be explicitly cast to an enum, but the cast does **not** check whether a named member has that value:

```csharp
RunState state = (RunState)99; // Legal; 99 has no named RunState member.
int numericValue = (int)RunState.Running;
```

That means an enum's static type is useful but not a complete validation guarantee. Validate values that come from untrusted input, a file, or an external system.

For ordinary non-flags enums, `Enum.IsDefined` checks whether a value exactly matches a declared constant:

```csharp
bool known = Enum.IsDefined(typeof(RunState), state);
```

Modern .NET also has a generic form, `Enum.IsDefined(state)`. It is an exact named-value check. It is generally not suitable for a flags combination such as `Read | Write`, because the combination need not be declared as one separate member.

For textual names, `Enum.TryParse<TEnum>` avoids exceptions for invalid text:

```csharp
if (Enum.TryParse<RunState>("Running", ignoreCase: true, out RunState parsed))
{
    Console.WriteLine(parsed);
}
```

Parsing text does not necessarily prove that the parsed value is a declared member; numeric text may be parsed as an enum value. If the input must be one of the declared non-flags names/values, combine parsing with the appropriate validation. For flags, validate that no bits outside the supported mask are set instead of requiring the exact combination to have a name.

Useful reflection helpers include `Enum.GetNames<TEnum>()` and `Enum.GetValues<TEnum>()` for listing declared names and values. Use them for display/configuration where appropriate; do not assume enum declaration order is a business priority unless you explicitly define that rule.

## A normal enum: choose exactly one state

An ordinary enum usually represents mutually exclusive alternatives: at one moment, a job is pending **or** running **or** complete. Use comparisons and `switch` to handle those alternatives:

```csharp
static string Describe(RunState state)
{
    return state switch
    {
        RunState.Unknown => "State has not been set.",
        RunState.Ready => "Ready to start.",
        RunState.Running => "Currently running.",
        RunState.Finished => "Finished.",
        _ => "Unrecognized state value."
    };
}
```

The discard arm `_` is a defensive fallback. A switch is not automatically exhaustive over every possible integral value of an enum because values outside the named members can exist.

## Flags enum: combine independent options

Sometimes a value means “one or more independent capabilities are enabled,” not exactly one choice. Use `[Flags]` and assign a separate power-of-two bit to each option:

```csharp
[Flags]
enum Permission
{
    None = 0,
    Read = 1,
    Write = 2,
    Delete = 4,
    Admin = 8
}
```

The values are bits: `Read` is `0001`, `Write` is `0010`, `Delete` is `0100`, and `Admin` is `1000`. Bitwise operators combine or inspect them:

```csharp
Permission permission = Permission.Read | Permission.Write;

bool canRead = (permission & Permission.Read) != 0;
bool canDelete = (permission & Permission.Delete) != 0;

permission |= Permission.Delete; // Add Delete.
permission &= ~Permission.Write; // Remove Write.
```

- `|` combines bits.
- `&` keeps only bits present in both operands.
- `~` flips bits; combining with `&` removes the selected bit.
- `|=` and `&=` update the stored enum value.

`[Flags]` communicates intent and makes `ToString()` commonly format combinations as comma-separated names. The attribute does not itself make the values powers of two or enforce valid combinations. Assign the bits correctly.

Always define `None = 0`. A zero flag cannot be tested as “present” using a bitwise intersection because it has no bits. `HasFlag` is readable:

```csharp
bool canRead = permission.HasFlag(Permission.Read);
```

The direct bitwise test is also common and makes the operation explicit. Remember that `HasFlag(None)` is true by definition: every value contains all zero bits. Do not use it to ask whether a nonempty permission exists.

Do not combine mutually exclusive states with `[Flags]`. `Ready | Finished` usually makes no semantic sense for a single state; `Read | Write` does for independent permissions.

## Complete runnable example

Save the following as the program source in a console project. It demonstrates an ordinary state enum, a flags enum, a switch expression, input validation, and expected output:

```csharp
using System;

internal enum JobState
{
    Unknown = 0,
    Ready = 1,
    Running = 2,
    Finished = 3
}

[Flags]
internal enum Permission
{
    None = 0,
    Read = 1,
    Write = 2,
    Delete = 4
}

internal class Program
{
    private static void Main()
    {
        JobState state = JobState.Running;
        Console.WriteLine(Describe(state));

        Permission permission = Permission.Read | Permission.Write;
        Console.WriteLine($"Permissions: {permission}");
        Console.WriteLine($"Can write: {(permission & Permission.Write) != 0}");

        Console.Write("Enter a state name: ");
        string? input = Console.ReadLine();

        if (Enum.TryParse(input, ignoreCase: true, out JobState selected)
            && Enum.IsDefined(selected))
        {
            Console.WriteLine($"Selected: {selected}");
        }
        else
        {
            Console.WriteLine("Unknown state name.");
        }
    }

    private static string Describe(JobState state)
    {
        return state switch
        {
            JobState.Unknown => "State has not been set.",
            JobState.Ready => "Ready to start.",
            JobState.Running => "Currently running.",
            JobState.Finished => "Finished.",
            _ => "Unrecognized state value."
        };
    }
}
```

Example interaction:

```text
Currently running.
Permissions: Read, Write
Can write: True
Enter a state name: Ready
Selected: Ready
```

In this example `Enum.TryParse` converts the text, while `Enum.IsDefined` rejects numeric values that are not named. The check is appropriate because `JobState` is not a flags enum and only one declared state is expected.

## When an enum is a good choice

Use an enum when the set is:

- Closed or controlled by the application.
- Small enough to name and understand.
- Mostly stable across the lifetime of the API/data contract.
- A set of alternatives of one conceptual kind.

Consider another design when values are supplied freely by users/configuration, are expected to grow independently, carry different data, or need behavior that varies significantly. A validated string/key, class hierarchy, or explicit result type may be a better model.

## Common mistakes

- Assuming every enum value must match a declared name.
- Forgetting that the default enum value is zero.
- Assigning sequential values to independent flags instead of powers of two.
- Using `[Flags]` for mutually exclusive states.
- Using `Enum.IsDefined` to validate arbitrary flags combinations.
- Persisting implicit numeric values and later reordering members.
- Assuming enum names are serialized as strings by every serializer.
- Treating `None = 0` as a bit that can be detected with `HasFlag`.

## Interview questions with answers

### Are enums strings?

No. An enum is a value type backed by an integral type. Its names are symbolic constants; conversion/serialization determines how the value is represented elsewhere.

### Can an enum variable contain a value with no declared name?

Yes. The zero default may be unnamed, and an explicit cast from an integer can create any representable underlying value. Validate external values.

### What does `[Flags]` do?

It marks an enum as intended for bitwise combinations and influences formatting. It does not assign correct bit values or enforce combinations; the developer must define powers of two and validate supported bits.

### Why does a flags enum use powers of two?

Each value occupies an independent bit. Powers of two have no overlapping bits, so bitwise combinations can preserve which options were selected.

### What is the difference between an ordinary enum and a flags enum?

An ordinary enum normally represents one alternative at a time. A flags enum represents zero or more independent options combined in one value.

### Why define a zero member?

Zero is the default enum value and represents no set bits. A named `Unknown`/`None` makes the default understandable and safer to inspect.

### Is `Enum.IsDefined` enough for validating flags?

Usually not. A valid combination may not be declared as one named member. Validate that only allowed bits are set, for example by checking `(value & ~allSupportedFlags) == 0` with a correctly typed mask.

### Why explicitly number persisted enum members?

Implicit values depend on declaration order. Reordering or inserting members can change the meaning of stored numbers. Explicit values protect a numeric contract, though the values must still be managed carefully.

## Practice

1. Add a zero-valued `Unknown` member to an enum and explain what `default(TheEnum)` returns.
2. Cast an integer not in the enum and inspect `Enum.IsDefined`.
3. For a flags enum with `Read`, `Write`, and `Delete`, combine two options, test the third, add it, and then remove one.
4. Explain why `Enum.IsDefined` is useful for `JobState` but not sufficient for a combined `Permission`.
5. Choose numeric or string serialization for a hypothetical long-lived API and explain how you would handle future enum values.

## Mental model

An ordinary enum is a named integral value representing one choice. A flags enum is a set of independent bits representing several choices at once. Names improve source readability, but values can exist without names, so defaults, casts, persistence, parsing, and external input need deliberate handling.
