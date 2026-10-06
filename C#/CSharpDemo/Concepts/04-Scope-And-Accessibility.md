# 04 - Scope and Accessibility in C#

## Why this topic matters

When a compiler says that a variable is unavailable or a member cannot be accessed, two different rules may be involved. Understanding the difference helps you fix the code for the right reason and design types that do not expose more than they should.

This guide teaches both ideas from the beginning:

- **Scope** answers: “Where in the source code can I use this name?”
- **Accessibility** answers: “Which code is allowed to access this type or member?”

They are related, but neither one replaces the other. A public method can still be impossible to call if the class containing it is inaccessible. A local variable can be inaccessible after its block ends even though no access modifier is involved.

## Prerequisites: type, member, assembly

- A **type** describes a kind of value or object. A class such as `Score` is a type.
- A **member** is declared inside a type: for example, a field, property, method, constructor, or nested type.
- An **assembly** is a compiled .NET unit, usually a `.dll` or `.exe`. A project normally builds an assembly. An assembly is not the same thing as a namespace or folder.
- A **derived type** is a class that inherits from a base class using `:`.
- A **containing type** is the type in which a member or nested type is declared.

The six familiar member access forms are `public`, `private`, `protected`, `internal`, `protected internal`, and `private protected`. C# also has the `file` modifier for a top-level type. `file` is not a seventh member access choice: it controls which source file can name a type.

## Part 1: Scope

### Local-variable scope

A **local variable** is declared inside a method, constructor, property accessor, or block. It can be used from its declaration through the end of its scope, subject to definite-assignment rules. **Definite assignment** means C# checks that a local has been given a value before it is read.

```csharp
using System;

class Program
{
    static void Main()
    {
        int total = 10;

        if (total > 0)
        {
            int bonus = 2;
            total += bonus;
            Console.WriteLine(total); // 12: both names are in scope here.
        }

        Console.WriteLine(total); // 12: total was declared in the outer block.
        // Console.WriteLine(bonus); // Does not compile: bonus's block has ended.
    }
}
```

The commented line is intentionally invalid. Uncomment it and the compiler reports that `bonus` does not exist in the current context. The variable is not “private”; it is simply out of scope.

### Parameters, fields, and properties

A **parameter** is a named input declared in a method signature. It is in scope in that method body. A **field** is a variable declared directly in a type. An **instance field** belongs to one object; a **static field** belongs to the type and is shared by its instances. A **property** exposes controlled get/set operations and is not itself a field, even though it can use a field to store data.

```csharp
using System;

class Counter
{
    private int value; // One value per Counter object.
    private static int createdCount; // Shared by all Counter objects.

    public Counter()
    {
        createdCount++;
    }

    public int Value
    {
        get { return value; }
    }

    public int Add(int amount) // amount is a parameter.
    {
        int nextValue = value + amount; // Local to Add.
        value = nextValue;
        return value;
    }

    public static int CreatedCount
    {
        get { return createdCount; }
    }
}

class Program
{
    static void Main()
    {
        Counter first = new Counter();
        Counter second = new Counter();
        first.Add(3);
        second.Add(8);

        Console.WriteLine(first.Value); // 3
        Console.WriteLine(second.Value); // 8
        Console.WriteLine(Counter.CreatedCount); // 2
    }
}
```

This complete program illustrates scope and storage without confusing them with access permission. `amount` and `nextValue` are local to `Add`. `value` belongs separately to each object. `createdCount` is one shared field. The `public` and `private` words control access; they do not determine whether a variable is local or an instance field.

### Scope is not lifetime

**Scope** is where the source code can name a variable. **Lifetime** is how long its value exists at runtime. They often line up for locals, but they are not definitions of one another. A returned object can outlive the method that created it. A captured local can remain alive because a delegate or lambda still refers to it. Do not use “stack variable” as a synonym for “local scope”; actual storage is an implementation detail and depends on context.

### Shadowing

**Shadowing** occurs when a nested declaration reuses a name from an outer scope, hiding the outer name in that region. C# disallows many confusing local-variable shadowing cases, but member names can be hidden (for example, a derived member can hide a base member). Prefer distinct names and use `this.MemberName` or `base.MemberName` when you intentionally need to distinguish members.

## Part 2: The access modifiers

### First, what does “accessible” mean?

Accessibility is checked by the C# compiler based on the declaration and the caller's relationship to it. “The file is in the same folder” is not generally an access rule. A namespace is not an access boundary. The most important boundaries are the containing type, the assembly, the inheritance relationship, and (for `file`) the source file.

The table describes the normal rule for a member declared on an accessible type:

| Modifier | Same containing type | Other type, same assembly | Derived type, other assembly | Unrelated type, other assembly |
|---|---:|---:|---:|---:|
| `public` | Yes | Yes | Yes | Yes |
| `private` | Yes | No | No | No |
| `protected` | Yes | No | Yes, through the derived-type access rules | No |
| `internal` | Yes | Yes | No, unless also in the same assembly | No |
| `protected internal` | Yes | Yes | Yes | No |
| `private protected` | Yes | Only when caller is also derived | No | No |

This table assumes the containing type itself is accessible and that the caller has a reference to the assembly. A member cannot be more reachable in practice than its containing type.

### 1. `public`

`public` means code can access the declaration wherever its containing type is accessible. A public instance member still needs an object; a public static member is called through the type. Public members are part of the API callers can depend on, so changing or removing them can break those callers.

Complete runnable example:

```csharp
using System;

public class GreetingService
{
    public string Prefix { get; set; } = "Hello";

    public string CreateGreeting(string name)
    {
        return $"{Prefix}, {name}!";
    }
}

public class Program
{
    public static void Main()
    {
        GreetingService service = new GreetingService();
        service.Prefix = "Welcome";
        Console.WriteLine(service.CreateGreeting("Mina"));
    }
}
```

Expected output:

```text
Welcome, Mina!
```

Both the type and the members used by `Program` are public. `CreateGreeting` is an instance method, so the call uses `service`. If `GreetingService` were internal, another assembly could not use it even if its method remained public.

### 2. `private`

`private` makes a class or struct member available only inside its containing type (including applicable nested-type access rules). It is the narrowest ordinary member access level and is the usual choice for implementation details. A top-level class itself cannot be declared `private`; a nested type can be private to its containing type.

Complete runnable example:

```csharp
using System;

public class BankAccount
{
    private decimal balance;

    public decimal Balance
    {
        get { return balance; }
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount));
        }

        balance += amount; // Allowed: this code is inside BankAccount.
    }
}

public class Program
{
    public static void Main()
    {
        BankAccount account = new BankAccount();
        account.Deposit(25m);
        Console.WriteLine(account.Balance); // 25

        // account.balance = 1_000_000m;
        // Does not compile: balance is private to BankAccount.
    }
}
```

The private field protects the rule that deposits must be positive. The public method is the supported operation; the public property is read-only to callers. `private` is about which code may access a member, not whether the member exists at runtime.

### 3. `protected`

`protected` permits access from the declaring class and classes derived from it. It is useful when a base class intentionally provides a hook or shared implementation to subclasses. It does **not** mean “any class in the same project.”

Complete runnable example:

```csharp
using System;

public class Animal
{
    protected string Name { get; }

    public Animal(string name)
    {
        Name = name;
    }

    public virtual void Speak()
    {
        Console.WriteLine($"{Name} makes a sound.");
    }
}

public class Dog : Animal
{
    public Dog(string name) : base(name)
    {
    }

    public override void Speak()
    {
        Console.WriteLine($"{Name} barks."); // Allowed in the derived class.
    }
}

public class Program
{
    public static void Main()
    {
        Animal animal = new Animal("Animal");
        Dog dog = new Dog("Pip");
        animal.Speak();
        dog.Speak();

        // Console.WriteLine(dog.Name);
        // Does not compile: Program is not Animal or a derived class.
    }
}
```

Expected output:

```text
Animal makes a sound.
Pip barks.
```

There is an additional protected-access rule: a derived class cannot always use a protected member through an arbitrary base-class-typed object. Protected access is intended for the derived object's implementation, not as a general public accessor. If callers need to read a value, expose an appropriately designed public or protected-internal property instead.

### 4. `internal`

`internal` allows access from anywhere in the **same assembly**. It is often used for implementation types shared across projects' source files in one build, while keeping them out of the normal public API of a library.

Complete runnable same-assembly example:

```csharp
using System;

internal class TaxCalculator
{
    internal decimal Calculate(decimal subtotal)
    {
        return subtotal * 0.10m;
    }
}

public class Program
{
    public static void Main()
    {
        TaxCalculator calculator = new TaxCalculator();
        Console.WriteLine(calculator.Calculate(50m)); // 5.00
    }
}
```

This works because both types are compiled into the same project assembly. If another project references the compiled assembly, its code cannot ordinarily name `TaxCalculator` or call `Calculate`, even though both declarations say `internal`.

An assembly can deliberately grant another assembly friend access with `InternalsVisibleTo`. This is commonly used for test assemblies, but it changes the boundary and should be intentional. Strong-named assemblies require the appropriate strong-name public key in the friend declaration.

### 5. `protected internal`

`protected internal` means **same assembly OR derived type**. The word `internal` does not narrow `protected` here; it adds another allowed route. A non-derived caller in the same assembly may access the member. A derived caller in another assembly may also access it under protected rules.

Runnable same-assembly example:

```csharp
using System;

public class Report
{
    protected internal void WriteLine(string text)
    {
        Console.WriteLine(text);
    }
}

public class UnrelatedHelper
{
    public void Run()
    {
        Report report = new Report();
        report.WriteLine("Allowed: helper is in the same assembly.");
    }
}

public class Program
{
    public static void Main()
    {
        new UnrelatedHelper().Run();
    }
}
```

An unrelated helper can call `WriteLine` because it is compiled into the same assembly. In a different assembly, an unrelated helper would not be able to call it. A class deriving from `Report` in that other assembly can access it through the protected inheritance route.

### 6. `private protected`

`private protected` means **derived type AND same assembly**. Both conditions must be true. It is narrower than either `protected` or `internal` alone. Use it when only subclasses that are part of the same assembly's implementation should access a member.

Complete runnable example:

```csharp
using System;

public class Document
{
    private protected void RecordAuditEntry(string message)
    {
        Console.WriteLine($"Audit: {message}");
    }
}

public class Invoice : Document
{
    public void Approve()
    {
        RecordAuditEntry("Invoice approved");
        // Allowed: Invoice derives from Document and is in this assembly.
    }
}

public class Program
{
    public static void Main()
    {
        new Invoice().Approve();
    }
}
```

Expected output:

```text
Audit: Invoice approved
```

An unrelated type in this assembly cannot call `RecordAuditEntry` because it is not derived from `Document`. A derived type in a different assembly cannot call it because it is outside the declaring assembly. This is why `private protected` is not interchangeable with `protected internal`.

### 7. `file` for a source-file-local type

`file` is for a **top-level type declaration**. It makes that type name available only within the source file where it is declared. It is useful for helper types that should not become visible to other source files in the same assembly. It was introduced in C# 11, so it is available in this project's .NET 10/C# toolchain.

`FileLocalExample.cs`:

```csharp
using System;

file class MessageFormatter
{
    public string Format(string message)
    {
        return $"[demo] {message}";
    }
}

public class FileLocalExample
{
    public static void Main()
    {
        MessageFormatter formatter = new MessageFormatter();
        Console.WriteLine(formatter.Format("Ready"));
    }
}
```

This file compiles as an executable and prints `[demo] Ready`. If a separate `AnotherFile.cs` in the same project tries `new MessageFormatter()`, compilation fails: `MessageFormatter` is not visible outside `FileLocalExample.cs`. Do not put a second `Main` into the same project when trying the second file; test the inaccessible name from another class or use a separate scratch project.

`file` does not mean that the type is private to one object or hidden from reflection at runtime. It is a source-level name/access boundary. It is also not the same as placing a class in a separate folder.

## Combining rules and the accessibility domain

The visibility of a member is limited by every containing declaration. For example, a `public` method inside an `internal` class is callable only from code that can access that internal class. This is sometimes described as the member's **effective accessibility**.

Accessibility also applies to API signatures. A public method cannot expose a less-accessible type in a way that would require callers to name that type. The compiler reports inconsistent accessibility. For example, a public method should not return an internal class as its declared return type.

Some declarations permit only specific modifiers. A top-level class or struct is normally `public` or `internal` (or `file` where appropriate), not `private` or `protected`. A nested type can use member accessibility. Constructors, properties, events, and accessors have their own applicable rules. When an accessor needs narrower visibility than its property, at most one accessor may have an explicit access modifier, and it must be more restrictive than the property:

```csharp
public class Score
{
    public int Value { get; private set; }
}
```

Here callers can read `Value`; only `Score` can assign it. The property itself is public, while its setter is private.

## Default accessibility

Defaults depend on the kind and location of the declaration. Useful common rules are:

- A top-level class, struct, interface, enum, or delegate with no modifier is generally `internal`.
- A class or struct member with no modifier is generally `private`.
- A nested type in a class or struct defaults to `private`.
- Interface members have interface-specific rules and are commonly implicitly public; do not apply the class-member default rule to them.
- A local variable has no accessibility modifier. Its availability is determined by scope.

In teaching code and public libraries, writing modifiers explicitly often makes intent easier to see. Do not infer every default from a single example; declaration kind matters.

## Accessibility versus namespaces, folders, and projects

These ideas are often confused:

- A **folder** organizes files for people and tools; it does not make a C# type private or public.
- A **namespace** organizes names; two types can be in the same namespace but different assemblies.
- A **project** is a build configuration. A project normally produces one assembly, but advanced build setups can produce more.
- An **assembly** is the boundary used by `internal`.
- An **inheritance relationship** is the boundary used by `protected`.
- A **source file** is the boundary used by `file`.

Moving a type into another folder does not change its accessibility. Moving code into another project often changes its assembly, so `internal` access may stop working unless the projects are configured as friends or the API is made appropriately accessible.

## Encapsulation: choosing a useful boundary

**Encapsulation** means keeping state and implementation details behind a controlled interface. It is not achieved by making everything private or everything public. Choose the narrowest access that supports the intended callers:

- Use `private` for internal state and helper methods that only one type needs.
- Use `public` for intentional API surface that callers should rely on.
- Use `internal` for collaboration within one assembly.
- Use `protected` when designed subclasses need an extension point.
- Use `protected internal` or `private protected` only when the two-part boundary is genuinely intended.
- Use `file` for a helper type that should not be named by other source files.

Avoid exposing mutable fields directly. A public field lets callers change state without validation. A property or method lets the type preserve rules, as `BankAccount.Deposit` did above.

Inheritance is a strong coupling point. Making many members protected can make future base-class changes risky because unknown subclasses may rely on them. Prefer a small, documented set of subclass hooks.

## Common compiler mistakes and how to read them

### “Inaccessible due to its protection level”

The code can resolve the type/member name, but the caller is not allowed to access it. Check the declaration and compare the caller against the modifier's boundary: same type, same assembly, derived type, or same file.

### “The name does not exist in the current context”

This often indicates a scope or name-resolution problem, not an access-modifier problem. Check spelling, declaration location, braces, namespace imports, and whether the variable is outside its block.

### “Inconsistent accessibility”

A more-visible declaration exposes a less-visible type in its signature or structure. Make the types' intended API visibility consistent, or reduce the visibility of the exposing member/type.

### “Cannot access protected member through a qualifier of type ...”

The caller may be in a derived class but using a base-class-typed object that does not satisfy protected access rules. Access the protected member through the derived instance in the permitted context, or expose a deliberate public/protected API.

## Interview questions with answers

### 1. What is the difference between scope and accessibility?

Scope is where a name is available in source code, usually determined by declaration and braces. Accessibility is a compiler-enforced permission for types and members based on modifiers and caller relationships. A local variable normally has scope but no access modifier.

### 2. What is the difference between `private` and `protected`?

Both prevent unrelated callers from using the member directly. `private` is for the declaring type; `protected` also allows derived types to use it under the protected-access rules.

### 3. What is the difference between `internal` and `protected`?

`internal` grants access based on the assembly. `protected` grants access based on inheritance. A same-assembly unrelated type can access an internal member but not a protected one.

### 4. Explain `protected internal` versus `private protected`.

`protected internal` means same assembly **or** derived type. `private protected` means same assembly **and** derived type. Think OR versus AND.

### 5. Can a public member be inaccessible?

Yes. Its containing type may be less accessible, its containing namespace/type may not be available through a reference, or an instance may be required. `public` does not override the accessibility of the declarations that contain it.

### 6. Is `internal` the same as “same project”?

Usually one project produces one assembly, so the terms often appear to line up in ordinary solutions. The language rule is same assembly, not same folder, namespace, or project name. Build configuration and friend-assembly declarations can affect the practical result.

### 7. Does `private` mean only the same object can access it?

No. Private access is scoped to the containing type, not a particular instance. Code inside the declaring type may access private members of another instance of that same type.

### 8. What is `file` used for?

It makes a top-level type name usable only in its declaring source file. It is a source-level type-visibility boundary, not a member access modifier and not a runtime security mechanism.

### 9. Why not make every member public?

Every public member becomes a potential dependency for callers. That increases coupling and makes future changes breaking. Expose only the operations and state that callers need.

### 10. Does garbage collection determine scope?

No. Scope is a language/name-visibility concept. Garbage collection is runtime memory management for reachable managed objects. A captured local can remain alive after its lexical block because a delegate still refers to it.

## Practice: predict before compiling

For each change, state whether it compiles and why before trying it:

1. Uncomment `Console.WriteLine(bonus)` after the `if` block in the first example.
2. Uncomment `account.balance = 1_000_000m` in the bank-account example.
3. Move `UnrelatedHelper` into a second project and keep `Report.WriteLine` as `protected internal`.
4. Move `Invoice` into a second project and keep `Document.RecordAuditEntry` as `private protected`.
5. Try to use `MessageFormatter` from another source file.
6. Make `GreetingService` internal while leaving `CreateGreeting` public; decide whether another assembly can call it.

Answers: (1) no, because the local is out of scope; (2) no, because the field is private; (3) no, because the helper is unrelated and outside the assembly; (4) no, because the derived type is outside the assembly; (5) no, because the type is file-local; (6) no, because the containing type is inaccessible externally.

## Practice using this project

Open `ConsoleHelper.cs` and identify `public`, `static`, the class declaration, and the `Clear` method. `Program` can call `ConsoleHelper.Clear()` because the class and method are public and the method is static; no helper object is needed. If this project were consumed as a library and `ConsoleHelper` were internal, a separate consuming assembly ordinarily could not name it.

## Mental model

When code cannot use a name, ask two questions in order:

1. **Is the name in scope and resolvable here?** Check its declaration, braces, namespace, and references.
2. **If the name resolves, does this caller have permission?** Check the containing type and the access modifier's boundary: type, inheritance, assembly, or source file.

For member modifiers, remember the axes: `public` broadly; `private` within the declaring type; `protected` through inheritance; `internal` within the assembly; `protected internal` by either route; `private protected` only when both inheritance and same-assembly conditions hold. `file` limits a top-level type to one source file.
