# 01 - Project Anatomy: `.csproj`, `bin`, and `obj`

## Start with the question: what is a C# project?

A C# project is a buildable unit of work. It contains source code and a project file that tells the .NET build system how to compile that code. A project can produce an executable application, a reusable class library, or another kind of output.

The source file contains the code a person edits. The project file describes the rules for turning that source into a program. The SDK reads those rules, resolves dependencies, compiles code, and creates generated and final output files.

This workspace is an **SDK-style executable project**:

- Project file: `CSharpDemo.csproj`
- Output type: `Exe`, meaning the project is an application
- Target framework: `net10.0`, meaning the project targets .NET 10
- Source files: files such as `Program.cs` and `ConsoleHelper.cs`

The project file is not the same thing as the source code, the running program, the .NET SDK, or the .NET runtime.

## A few terms before we follow a build

- **.NET SDK**: development tools that include the .NET CLI, MSBuild build engine, compilers, and framework targeting packs. You use it to create, restore, build, test, and publish projects.
- **.NET runtime**: the software needed to load and execute a compiled .NET application. The SDK includes a runtime, but a deployed application may run on a separate runtime installation or carry its own runtime if published self-contained.
- **MSBuild**: the build engine used by the .NET SDK. It evaluates project settings and runs build tasks in a defined order.
- **Compiler**: a tool that checks source code and translates it into compiled output. For C#, the compiler is Roslyn (`csc`).
- **Dependency**: code or an asset the project needs, such as a NuGet package or another project.
- **Generated file**: a file produced by tools from source/configuration. It is not normally edited by hand because a later build can replace it.

## What is a `.csproj` file?

The suffix `.csproj` means C# project. It is an XML document containing project properties, item declarations, and sometimes build targets. The SDK-style project file in this workspace is:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

</Project>
```

The opening `Project` element selects the .NET SDK. That SDK supplies default build behavior, so a small project file can still cause many build steps to happen. `PropertyGroup` contains named settings, called properties. A project can have several property groups with conditions; later evaluation and imports determine the effective value.

### Read every setting in this project's file

#### `OutputType`

`Exe` says this project is an application with an entry point. A class library generally uses `Library` or leaves the property at the SDK's library default. This setting affects the kind of output and entry-point requirements; it does not contain the application code.

#### `TargetFramework`

`net10.0` is the target framework moniker (TFM). It tells the SDK which .NET API reference assemblies and target framework rules to use. It does not mean the project file contains the .NET runtime, and it does not choose every deployment detail. For deployment, runtime identifiers and framework-dependent/self-contained settings can matter too.

#### `ImplicitUsings`

When this is enabled, the SDK generates common `global using` directives appropriate for the project type and target. This means source files can use common types such as `Console` without each file writing `using System;`. It does not download a package and does not make arbitrary libraries available. Generated implicit-usings source is normally visible under `obj`.

#### `Nullable`

`enable` turns on nullable reference type analysis. The compiler can warn when code appears to use a possibly-null reference as though it cannot be null. This is compile-time analysis and annotation behavior; it does not make null impossible at runtime or automatically validate user input.

### How is a `.csproj` created and changed?

A template creates the initial project file. For example, `dotnet new console` creates a console application in the current directory, including a project file and initial source file. An IDE can create a project from a template too.

The SDK does **not** rewrite the `.csproj` every time you build. You or a tool edit it when the project's configuration changes. For example, `dotnet add package Some.Package` adds a `PackageReference` to the project file; adding a source file to a normal SDK-style project usually does not require editing the project file.

## Properties, items, and targets

The example above contains a property group. Project files can also declare **items**: named collections of inputs to build tasks. Common item types include:

- `Compile`: C# source files passed to compilation
- `PackageReference`: NuGet packages required by the project
- `ProjectReference`: another project whose output is needed
- `None`: files included in the project but not compiled as C# source
- `Content`: files copied or published according to build settings

An explicit package reference can look like this:

```xml
<ItemGroup>
  <PackageReference Include="Example.Package" Version="1.2.3" />
</ItemGroup>
```

That XML declares a dependency. Restore resolves it and its dependencies; the package's code is not copied into your `.csproj`.

An SDK-style project normally uses implicit source inclusion. To explicitly exclude a file, a project may use an item such as:

```xml
<ItemGroup>
  <Compile Remove="Scratch\**\*.cs" />
</ItemGroup>
```

MSBuild **targets** are named groups of build tasks, such as `Restore`, `Build`, or `Publish`. The SDK imports many target definitions behind the scenes. Most application developers can use the standard targets without writing custom ones.

## What is the project directory and what is a solution?

The **project directory** is the directory containing a project file and its related source/assets. A **solution** (`.sln` or newer `.slnx`) groups projects so IDEs and commands can work with several projects together. A solution is not required for a single project. A project can also reference another project, creating a build dependency.

The folder names in a repository are mainly organization. A folder does not become a project unless it contains or belongs to project/build configuration, and a folder name does not define a C# namespace. Namespace declarations in `.cs` source define type names.

## What is `obj`?

`obj` is the project's **intermediate output directory**. Build and restore steps use it while preparing final output. It is usually organized by configuration and target framework, for example `obj/Debug/net10.0`.

Common contents include:

- `project.assets.json`: restore's resolved dependency graph and asset selection used by later build steps.
- NuGet-generated `.props` and `.targets` files: imported build settings from package restore.
- Generated C# source: for example, global using declarations and assembly attributes.
- Reference assemblies: compile-time forms of assemblies, sometimes under `ref` or `refint` folders.
- Caches, file lists, and other build bookkeeping.

The exact files vary by SDK version, project options, configuration, and packages. `obj` is not one single file and is not normally the final application you launch. Do not manually edit generated files there; change the source or configuration that produced them.

## What is `bin`?

`bin` is the project's **build output directory**. It normally contains the compiled application output, organized by configuration and target framework. A Debug build for this project commonly uses `bin/Debug/net10.0`; a Release build commonly uses `bin/Release/net10.0`.

Typical output files for a framework-dependent .NET application include:

```text
bin/Debug/net10.0/
  CSharpDemo.dll
  CSharpDemo.deps.json
  CSharpDemo.runtimeconfig.json
```

- `CSharpDemo.dll` is the compiled .NET assembly containing the program and metadata.
- `CSharpDemo.deps.json` describes runtime dependencies and selected assets.
- `CSharpDemo.runtimeconfig.json` tells the runtime which framework the application targets and relevant runtime settings.
- A platform-specific app-host executable, such as `CSharpDemo.exe` on Windows, may also be present. Its presence depends on build/publish settings.

Do not edit these generated files to make application changes. Edit `.cs` source or project configuration, then build again. `dotnet publish` creates deployment output; a publish directory is not necessarily identical to the ordinary `bin/Debug` build directory.

## How the folders relate: follow one build

The short version is:

```text
project settings + source files + dependencies
                    |
                    v
       restore and intermediate work (`obj`)
                    |
                    v
       compiled/build output (`bin`)
                    |
                    v
             .NET runtime executes
```

More precisely:

1. The SDK finds and evaluates the project file, SDK defaults, and imported settings.
2. Restore resolves package/project dependencies for the selected target framework and writes assets into `obj`.
3. The build selects source and generated files and invokes the compiler.
4. Intermediate build products and metadata are written to `obj`.
5. Final build outputs are copied or generated under `bin`.
6. `dotnet run` (when requested) launches the project output with the .NET runtime.

Restore and build are related but different. Restore answers “what dependencies and assets does this project need?” Build answers “can these inputs be compiled into outputs?” Run adds “start the application.” The next guide explores those commands in depth.

## Why do paths include `Debug` and `net10.0`?

Build output has dimensions. The configuration distinguishes Debug and Release, while the TFM distinguishes target frameworks. A project targeting more than one framework can produce a separate output for each. A publish that specifies a runtime identifier can add another path dimension, for example a platform-specific output.

- **Debug** usually enables development-friendly build settings and is convenient while debugging.
- **Release** usually enables optimized build settings for deployment/performance testing.
- **TFM** identifies a target framework such as `net10.0`.
- **RID** (runtime identifier) identifies an operating system/architecture target, such as a Windows x64 target. RID-specific output is especially relevant to publishing.

These names describe build selections; they are not separate copies of your handwritten source.

## When are `bin` and `obj` created?

They are usually created or updated when restore/build/run needs them. `dotnet restore` primarily updates restore assets under `obj`. `dotnet build` writes intermediate state and final output. `dotnet run` normally invokes the required build work before launching.

`dotnet clean` removes outputs for the selected project/configuration. A later build can recreate them. Deleting `bin` and `obj` manually is also usually recoverable because they are generated, but it forces work to be repeated and should not be the first response to every error. Stop a running program that is holding output files, then use `dotnet clean` or delete generated directories only when troubleshooting calls for it.

## How source files get included

Normal SDK-style projects use **default items**. The SDK automatically includes C# source files under the project directory in compilation, with build output folders excluded. Therefore, adding `Concepts/NewExample.cs` inside this project normally makes it part of the next build without a `Compile` line in the `.csproj`.

There are important exceptions:

- The project can disable default compile items.
- A custom `Compile Remove` can exclude a path.
- A file can be linked from another location or included explicitly.
- A file might be outside the project tree.
- The project might use an older non-SDK format that lists files explicitly.

If a new C# file does not seem to participate, inspect the project file and build output before assuming the compiler ignored it. If the file is included, syntax/type errors will appear during build.

Removing a source file from an SDK-style project normally removes it from the next build's source set. If the code still appears to run, make sure you rebuilt and are launching the output for the correct project/configuration. The old assembly in `bin` remains until a successful build overwrites it or a clean removes it.

## What should go into source control?

Usually commit handwritten source, `.csproj` files, solution files when used, configuration, required assets, tests, and package version policy. Usually do not commit `bin` and `obj`; they are generated, can be large, and differ across machines/builds. A `.gitignore` file commonly excludes them.

There can be special cases: some deployment or generated-code workflows intentionally version generated artifacts. Follow the repository's explicit policy rather than assuming every generated file is always disposable.

## Common misunderstandings

- `obj` is not the same as `bin`: `obj` is primarily intermediate/restore/build state; `bin` contains build output.
- The `.csproj` is not regenerated by every build. It is project configuration and is edited when configuration/dependencies change.
- A folder is not a namespace. A namespace comes from source declarations (or a configured root namespace in particular generated scenarios).
- `using System;` does not install the `System` library. It is a C# name-lookup directive; the target framework supplies the relevant reference assemblies.
- `net10.0` in the project does not mean the application is automatically self-contained with a .NET 10 runtime.
- A successful build means the inputs compiled; it does not prove the application behaves correctly.

## Interview questions with clear answers

### What is the `.csproj`?

It is an XML build description for one .NET project. It declares settings and dependencies that the SDK/MSBuild use to restore and build the project. It is configuration, not the compiled program.

### What is the difference between `bin` and `obj`?

`obj` holds intermediate work such as restore assets, generated source, references, and build metadata. `bin` holds the resulting build output, such as the application assembly and runtime configuration files.

### When are these folders created?

Restore and build operations create or update them as needed. A template creates a `.csproj`; ordinary builds do not recreate that project file.

### Why is `project.assets.json` in `obj`?

Restore writes it to record which dependency assets were resolved for the project's target framework and build. Build tasks use those assets. It is generated state, not the list of package references a developer should edit by hand.

### Do I need to edit the `.csproj` to add a `.cs` file?

Usually not in an SDK-style project with default compile items enabled. The file must be under the project's inclusion rules. Older or customized projects can require explicit inclusion.

### Why should `bin` and `obj` usually stay out of Git?

They are reproducible build outputs and intermediate state. Committing them creates noise, repository size, and machine/configuration-specific changes. Commit the inputs needed to reproduce them instead.

### What happens if I delete `obj`?

The next restore/build regenerates intermediate assets and may take longer. It is commonly recoverable, but deleting it can remove useful caches and is not a substitute for understanding a build error.

### Does `OutputType` mean Windows `.exe`?

`Exe` means the project builds an executable application with an entry point. The exact output files depend on target/platform/build settings. .NET applications commonly have a `.dll` assembly and may also have an app-host `.exe`.

## Hands-on investigation

From the directory containing `CSharpDemo.csproj`, try these read-only inspections:

```powershell
Get-Content .\CSharpDemo.csproj
Get-ChildItem .\obj\Debug\net10.0
Get-ChildItem .\bin\Debug\net10.0
```

Then answer these questions in your own words:

1. Which setting makes this an application rather than a class library?
2. Which setting selects the .NET API target?
3. Which file under `obj` records resolved restore assets?
4. Which file under `bin` is the compiled assembly?
5. If you add a `.cs` file under this project and build successfully, why does no explicit `Compile Include` have to be added?

## Mental model

Think of the `.csproj` as the recipe, `.cs` files and dependencies as the ingredients, `obj` as the preparation/intermediate work area, and `bin` as the built result. The SDK/MSBuild follows the recipe. The runtime executes the result. When source changes, change the inputs and build again; do not edit the generated result as if it were the source.
