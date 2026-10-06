# 03 - C# Program Foundations

## What does a C# program consist of?

A **program** is instructions plus data. In a C# project, people normally write those instructions in `.cs` source files. The project build compiles the included files together. The runtime needs an **entry point**: the method where execution begins.

You already saw the project/build journey in the previous guides. This lesson focuses on reading the C# code that goes through that journey: types, variables, expressions, statements, methods, and control flow.

## The entry point: where execution starts

This project uses the traditional explicit entry point:

```csharp
using System;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Hello, C#");
    }
}
```

- `class Program` declares a class named `Program`.
- `public` makes the class accessible outside its assembly, subject to references.
- `static` means the runtime can invoke `Main` through the type without first constructing a `Program` object.
- `void` means the method does not return a value to its caller.
- `Main` is the conventional entry-point name.
- Parentheses `()` declare the parameter list; an empty list means this method accepts no arguments.
- Braces `{ }` delimit a type or method body.

Common entry-point signatures include `static void Main()`, `static int Main()`, `static Task Main()`, `static Task<int> Main()`, and forms that accept `string[] args`. An integer result becomes the process exit code: zero conventionally indicates success, and nonzero can indicate failure. Async forms allow an asynchronous entry point.

Modern C# also supports **top-level statements**. Instead of explicitly writing the `Program` class and `Main`, put statements directly in a source file:

```csharp
Console.WriteLine("Hello from top-level statements");
```

The compiler creates the entry point. A project should have only one effective entry point; combining a regular `Main` with top-level statements can cause entry-point conflicts unless project settings explicitly select one.

## Types: what values mean

A **type** defines what kind of values a variable can hold and what operations can be performed with them. C# is statically typed: the compiler knows each expression's type before the program runs, except where dynamic features deliberately defer some checks.

Some built-in types:

| C# type | Typical meaning | Example value |
|---|---|---|
| `int` | whole number in a 32-bit signed range | `42` |
| `long` | larger whole-number range | `9_000_000_000L` |
| `decimal` | base-10 decimal arithmetic, commonly prices | `19.95m` |
| `double` | binary floating-point measurement/calculation | `3.14159` |
| `bool` | true/false condition | `true` |
| `char` | one UTF-16 code unit | `'A'` |
| `string` | immutable text | `"hello"` |

The suffixes `L` and `m` tell the compiler which numeric type is intended for a literal. For instance, `19.95m` is a `decimal`; without `m`, a decimal-looking literal is normally a `double`.

Types are also user-defined. A `class` defines a reference type; a `struct` defines a value type; an `enum` gives names to a set of integral values; an `interface` declares a contract types can implement. These are separate concepts and get their own deeper guides where applicable.

## Variables and assignment

A **variable** is a named storage location with a type. A declaration introduces the name and type; an assignment stores a value:

```csharp
int count = 3;       // Declare and initialize.
count = count + 1;   // Read the old value, add one, store the result.
string course = "C#";
bool isReady = true;
```

The compiler rejects incompatible assignments unless a valid conversion is available or explicitly requested. A variable must be definitely assigned before it is read.

### `var` does not mean dynamically typed

`var` asks the compiler to infer the static type from the initializer:

```csharp
var count = 3; // The compiler infers int.
```

After inference, `count` is still an `int`; assigning a string to it is a compile-time error. `var` is not the same as `object` or `dynamic`. Use it when the type is clear from the right-hand side or is awkward to spell; avoid it when it obscures what the value represents.

### Null and nullable reference annotations

`null` represents the absence of a reference to an object. A reference may be null, so dereferencing it can fail at runtime. With nullable reference types enabled, `string` means code expects a non-null string, while `string?` communicates that null is allowed:

```csharp
string course = "C#";
string? answer = Console.ReadLine();

if (answer is not null)
{
    Console.WriteLine(answer.Length);
}
```

`Console.ReadLine()` can return null when no more input is available. Nullable reference types provide compile-time warnings; they do not add automatic runtime validation. The `?` annotation on a reference type is different from `Nullable<T>` for value types, such as `int?`, which represents either an integer or no integer.

## Value types and reference types

Common value types include numeric types, `bool`, `char`, enums, and structs. Common reference types include classes, arrays, delegates, and `string`.

The useful assignment mental model is:

- A value-type variable contains its value. Assigning it copies that value.
- A reference-type variable contains a reference to an object. Assigning it copies the reference, so both variables can refer to the same object.

```csharp
int firstNumber = 5;
int secondNumber = firstNumber;
secondNumber = 8;
Console.WriteLine(firstNumber); // 5: independent copied value.

int[] firstArray = [5];
int[] secondArray = firstArray;
secondArray[0] = 8;
Console.WriteLine(firstArray[0]); // 8: both references point to the same array.
```

Do not reduce this to “value types live on the stack and reference types live on the heap.” Actual storage depends on context and implementation. The language-level distinction is about the value/reference semantics, not a universal physical location.

## Expressions, statements, and operators

An **expression** produces a value. Examples: `count + 1`, `name.Length`, and `isReady && count > 0`. A **statement** performs an action or controls execution. Examples: a variable declaration, an assignment, a method call, or an `if` block.

Common operator groups:

- Arithmetic: `+`, `-`, `*`, `/`, `%`
- Comparison: `==`, `!=`, `<`, `<=`, `>`, `>=`
- Boolean logic: `&&` (and), `||` (or), `!` (not)
- Assignment: `=`, `+=`, `-=`, `++`
- Null handling: `??` (fallback), `?.` (conditional access), `!` (null-forgiving annotation in nullable analysis)

Integer division truncates the fractional part: `7 / 2` is `3`. If a fractional result is wanted, use an appropriate floating-point or decimal operand, such as `7.0 / 2.0`.

## Methods: naming behavior

A **method** is a named operation. Its signature includes its name, parameter types, and (for overload resolution) other applicable signature details. A **parameter** is a named input declared by a method; an **argument** is the value supplied by a caller.

```csharp
static int Add(int left, int right)
{
    return left + right;
}

int total = Add(4, 7); // 4 and 7 are arguments; total receives 11.
```

The return type here is `int`. A `void` method performs work without returning a value. `return` ends the current method; it does not necessarily terminate the whole program.

### Method overloading

Methods can share a name when their parameter lists distinguish them. This is **overloading**:

```csharp
static int Add(int left, int right) => left + right;
static decimal Add(decimal left, decimal right) => left + right;
```

The compiler selects an overload based on the argument types and overload-resolution rules. Changing only a method's return type does not create a valid overload, because a call generally does not identify an overload by the value it expects back.

### Parameter passing

Ordinary parameters are passed by value. For a value type, the value is copied. For a reference type, the reference is copied; a method can mutate the referenced object, but assigning the parameter to another object does not replace the caller's variable.

```csharp
static void ChangeFirstItem(int[] values)
{
    values[0] = 99; // Mutates the shared array object.
    values = [1, 2, 3]; // Reassigns only this local parameter.
}
```

`ref` passes a variable by reference so the method can read and update the caller's variable. `out` also passes by reference but the method must assign the output before returning. `in` passes a readonly reference under its rules. Use these modifiers for explicit API needs; ordinary parameters are simpler and clearer most of the time.

## Classes, objects, and members

A **class** is a type definition. An **object** is a runtime instance of a class. Fields store state; properties offer controlled access; methods define behavior; constructors initialize new instances.

```csharp
class Greeting
{
    private readonly string name;

    public Greeting(string name)
    {
        this.name = name;
    }

    public string SayHello()
    {
        return $"Hello, {name}!";
    }
}
```

`new Greeting("Mina")` constructs an object. `this.name` identifies the field belonging to the current object; it is used here because the constructor parameter is also named `name`. `readonly` means this field can be assigned during declaration or construction but not reassigned by ordinary instance code afterward. `private`/`public` are access modifiers and are explained in the scope/accessibility guide.

## Decisions, repetition, and input

### `if` and `switch`

Use `if` for a condition with true/false branches:

```csharp
if (count > 0)
{
    Console.WriteLine("Positive");
}
else
{
    Console.WriteLine("Zero or negative");
}
```

Use `switch` when selecting among distinct values or patterns. A `switch` statement and a `switch` expression are related forms with different syntax and result behavior.

### Loops

- `for` is useful when initialization, a condition, and an update define the loop.
- `foreach` visits each element in a sequence.
- `while` repeats while a condition remains true.
- `do`/`while` checks its condition after the body, so it runs at least once.

Watch for off-by-one errors and loops whose condition never changes. In C#, `break` exits the nearest loop or switch, and `continue` advances to the next loop iteration.

### Parse untrusted input safely

Text input is not automatically a number. `int.Parse` throws when text is invalid; `int.TryParse` returns whether conversion succeeded and provides the parsed value through an `out` parameter. Use `TryParse` when invalid input is an expected user outcome:

```csharp
string? text = Console.ReadLine();
if (int.TryParse(text, out int quantity))
{
    Console.WriteLine($"Quantity: {quantity}");
}
else
{
    Console.WriteLine("Please enter a whole number.");
}
```

## Exceptions: report failures that cannot be ordinary results

An **exception** is an object that represents an error or unusual condition during execution. A `try` block marks code that may fail; a matching `catch` handles a failure when the program can respond usefully; `finally` runs cleanup logic regardless of success/failure in common control flows. `using`/`await using` is the preferred pattern for disposing many resources.

Do not use exceptions as routine branching for expected bad input when `TryParse` or an explicit result is clearer. Do not catch every exception and silently ignore it: that hides failure and makes diagnosis difficult. Catch at a boundary that can add context, recover, or show a useful message.

## Complete runnable example

The following console program brings together an entry point, nullable input, parsing, a method, a conditional, and an exit code. Save it as `Program.cs` in a console project and run it with the .NET SDK:

```csharp
using System;

internal class Program
{
    private static int Main(string[] args)
    {
        Console.Write("Enter a whole number: ");
        string? input = Console.ReadLine();

        if (!int.TryParse(input, out int number))
        {
            Console.WriteLine("That was not a valid whole number.");
            return 1;
        }

        int doubled = Double(number);
        Console.WriteLine($"Twice {number} is {doubled}.");

        if (args.Length > 0)
        {
            Console.WriteLine($"First argument: {args[0]}");
        }

        return 0;
    }

    private static int Double(int value)
    {
        return value * 2;
    }
}
```

Example interaction:

```text
Enter a whole number: 6
Twice 6 is 12.
```

What happens in order:

1. The runtime calls `Main` and supplies an array of command-line arguments.
2. `Console.Write` prints a prompt without ending the line.
3. `ReadLine` returns text or null; the `string?` annotation communicates that possibility.
4. `TryParse` either writes an integer to `number` and returns `true`, or returns `false`.
5. On invalid input, the program prints an error and returns exit code `1` from `Main`.
6. On valid input, `Double` calculates a result, interpolation formats output, and `Main` returns `0`.

The method `Double` is a deterministic function for the normal range of `int`, but multiplication can overflow for extreme values depending on checked context. Production code must decide how to handle numeric bounds.

## What compilation and runtime each do

During compilation, the C# compiler parses source, resolves names and references, checks types and language rules, and emits an assembly containing intermediate language and metadata. A successful compile does not execute the application.

At runtime, the .NET runtime loads assemblies and types, executes methods, manages managed memory, and supplies framework services. The JIT compiler commonly translates methods to native machine instructions as needed. Deployment can use ahead-of-time compilation in supported configurations. Garbage collection reclaims managed objects that are no longer reachable, but it does not automatically close every file, socket, or operating-system resource; disposable resources still need deterministic disposal.

## Common misconceptions

- `var` is compile-time type inference, not dynamic typing.
- A reference-type variable and the object it refers to are not the same thing.
- Passing a reference-type parameter normally still passes the reference **by value**.
- `string?` produces nullable-analysis information, not a runtime guarantee or check.
- A successful build proves compilation, not correct runtime behavior.
- Garbage collection manages managed memory, not every external resource.
- `Main` returning an integer produces a process exit code; `return` in another method only returns from that method.
- `return` is different from `Console.ReadLine()` and `Console.WriteLine()`: one changes control flow, the others perform console I/O.

## Interview questions with answers

### What is an entry point?

It is the method where program execution begins. A traditional executable uses an appropriate static `Main`; top-level statements let the compiler generate that entry point.

### What does `static` mean?

A static member belongs to the type rather than an individual instance. It can be used through the type without constructing an object, subject to access rules.

### What is the difference between `var` and `dynamic`?

`var` is inferred once at compile time and remains statically typed. `dynamic` defers certain member binding/type checks until runtime.

### What does pass-by-value mean for an array parameter?

The method receives a copy of the array reference. Both the caller and parameter can refer to the same array and see element mutations. Reassigning the parameter does not reassign the caller's variable unless `ref` is used.

### Why use `TryParse` instead of `Parse`?

When invalid text is an expected result, `TryParse` makes success/failure explicit and avoids using exceptions for normal control flow. `Parse` is appropriate when failure is exceptional or has already been ruled out.

### What is the difference between a compile-time error and a runtime exception?

A compile-time error prevents the selected source from producing a successful build. A runtime exception occurs while an already-built program is executing.

### Is every class object stored on the heap and every local on the stack?

That is not a sound general rule for C#. Explain value versus reference semantics at the language level; actual memory placement is an implementation detail and can depend on optimization and context.

## Practice

1. Trace the complete example using input `6`, invalid input such as `six`, and end-of-input.
2. Add a method that converts a temperature from Celsius to Fahrenheit. State its input and return types.
3. Copy an `int` into another `int`, change the copy, and compare. Repeat with an array and explain the difference.
4. Add a loop that asks for another value until the user types `q`; decide how invalid numbers are handled.
5. Trace `Program.Main` in this workspace: find its menu loop, input read, switch, and module method calls.

## Mental model

Types describe valid values and operations. Variables name values or references. Expressions calculate values; statements sequence actions. Methods package behavior. Conditions and loops choose/repeat work. The compiler checks and builds the program; the runtime starts at the entry point and executes the compiled result.
