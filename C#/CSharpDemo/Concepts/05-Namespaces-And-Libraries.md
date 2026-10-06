# 05 - Namespaces and Libraries

## The problem: types need names and code needs boundaries

As a program grows, it can contain many types. Two unrelated parts may both need a type called `Logger`, `Address`, or `Helper`. We also need a way to compile a reusable component once and let another project consume it.

C# and .NET use several related mechanisms for these jobs. A **namespace** groups names. A **project** describes a build unit. An **assembly** is compiled output. A **reference** makes another assembly's types available while compiling. A **NuGet package** distributes reusable assemblies and other assets. These are not synonyms.

## Namespaces from the beginning

A **namespace** is a logical name that qualifies types and helps avoid naming collisions. Consider two fully qualified type names:

```text
Contoso.Billing.Invoice
Contoso.Sales.Invoice
```

Both final type names are `Invoice`, but the complete names differ. The fully qualified name includes the namespace plus the type name.

A file-scoped namespace can be declared like this:

```csharp
namespace CSharpDemo.Tools;

public static class TextTools
{
    public static string Normalize(string value)
    {
        return value.Trim();
    }
}
```

The equivalent block-scoped form is:

```csharp
namespace CSharpDemo.Tools
{
    public static class TextTools
    {
        public static string Normalize(string value)
        {
            return value.Trim();
        }
    }
}
```

File-scoped namespace syntax reduces indentation and allows one namespace declaration for the file. It does not change the meaning of the namespace. A file-scoped namespace must appear before type declarations in that file, and it cannot be combined with another namespace declaration in the same file.

## Namespace is not folder, project, or assembly

- A **folder** organizes files on disk. Moving a file does not automatically rename its namespace.
- A **namespace** organizes type names. It is declared in source; nested namespace names are a naming convention, not necessarily nested directory structures.
- A **project** is a build unit described by a `.csproj` file.
- An **assembly** is compiled output, usually a `.dll`. It can contain many namespaces and types.
- A **package** is a distribution unit and may contain one or several assemblies, metadata, analyzers, build files, or content.

One namespace can have declarations across many source files and assemblies. One project can define many namespaces. By convention, folders and namespace segments often match because it is easier for people to navigate, but the language does not require that.

The `RootNamespace` project property can influence generated names and tooling in particular project scenarios. It does not automatically wrap every handwritten `.cs` file in a namespace. Put an explicit namespace declaration in source when you want the type to belong to that namespace.

## `using`: shorter name lookup, not a dependency

A `using` directive makes names from a namespace easier to refer to. Without it, you can use the fully qualified name:

```csharp
System.Console.WriteLine("Hello");
```

With a `using` directive:

```csharp
using System;

Console.WriteLine("Hello");
```

`using System;` does not download a package, create a project reference, or load an assembly. It affects how the compiler looks up names. The project still needs a reference to the assembly that defines the type. Framework reference assemblies are supplied by the target framework; package/project references are declared separately.

### Namespace using directive

```csharp
using CSharpDemo.Tools;

TextTools.Normalize(" hello ");
```

This brings namespace members into simple-name lookup in the scope of the directive. It does not copy code into the current file.

### Alias directive

An alias gives a shorter or clearer local name to a namespace or type:

```csharp
using Tools = CSharpDemo.Tools;
using Formatter = CSharpDemo.Tools.TextTools;

string cleaned = Formatter.Normalize(" hello ");
```

Aliases are especially helpful if two namespaces contain a type with the same short name:

```csharp
using BillingInvoice = Contoso.Billing.Invoice;
using SalesInvoice = Contoso.Sales.Invoice;
```

The aliases do not create new types. They are names used by the compiler while resolving the original types.

### `using static`

`using static` makes static members of one type available without writing the type name each time:

```csharp
using static System.Math;

double distance = Sqrt(25);
```

This can shorten code but may make it less obvious where a member comes from. Prefer regular qualification if the origin would otherwise be unclear.

### `global using` and implicit usings

A `global using` directive applies across the compilation rather than only one file:

```csharp
global using System;
```

Modern SDK projects can generate global using directives when `<ImplicitUsings>enable</ImplicitUsings>` is set. In this workspace, common names such as `Console` can be available without an explicit `using System;` in every file. The generated directives live in build-generated source under `obj`; they are not handwritten additions to each file.

Use global usings sparingly. They are convenient for common, unambiguous namespaces but can make name resolution less obvious if many namespaces are imported globally.

## Libraries and references

A **library** is reusable code that a program can reference rather than run by itself. In .NET, a class library project normally builds a DLL and exposes selected types. Another project references it so the compiler can resolve its types and the build can include its output as a dependency.

### Project reference

A project reference connects projects during development. The consumer project refers to the library project, so building the consumer also builds the dependency in the correct order.

Commands to create a small example solution from an empty directory:

```powershell
dotnet new classlib --name GreetingLibrary
dotnet new console --name GreetingApp
dotnet add .\GreetingApp\GreetingApp.csproj reference .\GreetingLibrary\GreetingLibrary.csproj
dotnet run --project .\GreetingApp\GreetingApp.csproj
```

The reference command updates `GreetingApp.csproj`. A project reference resembles:

```xml
<ItemGroup>
  <ProjectReference Include="..\GreetingLibrary\GreetingLibrary.csproj" />
</ItemGroup>
```

`GreetingLibrary/Greeter.cs`:

```csharp
namespace GreetingLibrary;

public class Greeter
{
    public string Greet(string name)
    {
        return $"Hello, {name}!";
    }
}
```

`GreetingApp/Program.cs`:

```csharp
using GreetingLibrary;

Greeter greeter = new Greeter();
Console.WriteLine(greeter.Greet("Mina"));
```

Expected output:

```text
Hello, Mina!
```

The namespace in `Greeter.cs` determines the type's name. The project reference makes the library assembly available to `GreetingApp`. The `using` directive lets this source write `Greeter` instead of `GreetingLibrary.Greeter`.

### Package reference and NuGet

NuGet is .NET's package ecosystem and tooling. A package reference declares a dependency in a project, commonly as:

```xml
<ItemGroup>
  <PackageReference Include="Example.Package" Version="1.2.3" />
</ItemGroup>
```

The CLI can add one with `dotnet add package Example.Package`. Restore resolves that package and any **transitive dependencies** (dependencies required by the package). The package ID can differ from the namespace used in C# code; package documentation tells you which namespace and types it provides.

Before adding a package, check that it supports the project's target framework, is maintained, is appropriate for the task, and has acceptable security/licensing terms. Prefer a project reference while actively developing related projects together; packages are useful for versioned distribution and reuse across repositories/teams.

### Framework references

Types in the .NET base libraries are available through the target framework reference packs selected by `TargetFramework`. You generally do not add a NuGet package just to use `System.Console` or `System.String`. Some optional .NET components require packages or additional framework references; follow the documentation for that API.

## What makes a type visible to another project?

For another project to use a type, several independent conditions must be met:

1. The consumer has a project/package/framework reference to the assembly containing the type.
2. The source names the type correctly, either fully qualified or through suitable `using` directives/aliases.
3. The type and member have sufficient C# accessibility, usually `public` across assemblies.
4. The type supports the target framework and the required runtime dependencies are present.

`using` cannot make an inaccessible `internal` type public, and `public` cannot make an unreferenced assembly available. This is a common source of confusion: dependency/reference, name lookup, and access permission are separate checks.

## Public API and implementation hiding

A reusable library should expose the small set of types and members consumers need. `public` declarations form an API that other code can depend on. Internal/private implementation details can change more freely. Changing or removing a public method can break consumers even if the library itself still builds.

Namespaces do not enforce this boundary. Access modifiers do. A namespace named `Internal` does not make its types inaccessible, and a namespace named `Public` does not make them public.

## Ambiguous names and good habits

If two imported namespaces provide a type with the same simple name, the compiler may report ambiguity. Resolve it with a fully qualified name or alias. Avoid adding `using` directives just to shorten a single rare name if they make the rest of the file less clear.

Use stable, descriptive namespace names that match the library's ownership/domain and common project conventions. Keep folder layout and namespace layout aligned for readability, but remember that this is convention rather than an access rule.

## Interview questions with answers

### Does a namespace correspond to a DLL?

No. An assembly can contain many namespaces, and the same namespace can have types compiled into multiple assemblies.

### Does `using` add a package or assembly reference?

No. It changes name lookup in source. A project/framework/package reference is what makes the type's assembly available to compilation.

### What is the difference between a namespace and a project?

A namespace groups type names in source/metadata. A project is a build unit configured by a project file. A project can define many namespaces.

### What is the difference between a project reference and a package reference?

A project reference points to another project, usually for coordinated source development. A package reference consumes a versioned NuGet package resolved by restore. Both can provide types to the compiler, but their source/distribution workflows differ.

### What is a transitive dependency?

A dependency required by another dependency. Restore resolves it as part of the graph even if the application did not directly add that package.

### Why can `using MyLibrary;` fail to find a type?

The namespace may be wrong, the assembly may not be referenced/restored, the type may have a different namespace, or it may be inaccessible. A using directive solves only name lookup.

### If I move a source file to another folder, does its namespace change?

No. The declared namespace in code stays the same unless a tool edits it. Folder/namespace matching is a convention.

## Practice

1. Find namespace declarations in this workspace and compare them with folder names. Note any mismatch.
2. In `Program.cs`, find a `using` directive and name the type whose short name it enables.
3. Remove that directive mentally and write the fully qualified type name that would replace it.
4. Create the two-project greeting example above and explain separately what the project reference, namespace declaration, and using directive each contribute.
5. Make `Greeter` internal and try to use it from the separate app project. Explain why a namespace import cannot fix the accessibility error.

## Mental model

The namespace answers “what is this type called?” The project/assembly reference answers “is the type's compiled code available to this build?” The access modifier answers “is this caller allowed to use it?” A `using` directive only shortens the name you write.
