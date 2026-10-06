dotnet restore
dotnet build
dotnet run
# 02 - Build and Run: From C# Source to a Running Process

## The complete journey

When you edit `Program.cs`, you are editing **source code**, not the program that is already running and not necessarily the assembly currently under `bin`. The normal path is:

```text
.cs source + .csproj + dependencies
		|
		v
	 restore dependencies
		|
		v
    compile source to assembly
		|
		v
	build output in bin
		|
		v
 .NET runtime starts a process
```

The **SDK** provides developer/build commands. The **compiler** checks C# and emits a .NET assembly containing intermediate language and metadata. The **runtime** loads the assembly, provides services such as garbage collection, and executes code. The runtime commonly uses a JIT compiler to translate methods to native instructions as the program runs. Some deployment modes can use ahead-of-time compilation.

## Check which .NET tools are installed

From PowerShell, run:

```powershell
dotnet --info
dotnet --list-sdks
dotnet --list-runtimes
```

`--list-sdks` reports installed SDK versions; `--list-runtimes` reports runtime versions. A machine may have an SDK but lack the runtime version required by a particular framework-dependent application. An SDK can select a version through a `global.json` file when one applies. The project still controls its target framework in the `.csproj`.

## The main commands, one at a time

Run these from the directory containing `CSharpDemo.csproj`, or use `--project` to identify it explicitly.

### Restore: `dotnet restore`

Restore resolves NuGet and project dependencies for the target framework. It writes generated dependency information under `obj`, including `project.assets.json`. Restore does not compile your C# program and does not start it.

```powershell
dotnet restore
```

You usually do not need to call restore separately before every build: `dotnet build` restores automatically when required. Restore may need network access for uncached packages and can fail because of unavailable feeds, authentication, incompatible package versions, or invalid project configuration.

### Build: `dotnet build`

Build evaluates the project, restores dependencies when needed, compiles source, and writes build outputs. It checks syntax, type rules, references, and other compile-time requirements. It does **not** normally launch the application.

```powershell
dotnet build
```

For this project, the default configuration is normally Debug. To select another configuration:

```powershell
dotnet build --configuration Release
```

Common options:

- `-c Debug` or `-c Release` selects the configuration (`--configuration` is the long form).
- `--no-restore` skips restore and assumes valid restore assets already exist.
- `--no-incremental` requests a non-incremental build, making more build work run again.
- `--verbosity minimal` changes how much build detail is printed.

An exit code of zero means the command succeeded. Warnings are not always errors; compiler warnings and errors are reported separately. A failed build does not produce a newly successful output for the failing project, so do not expect an old output file to contain the newest source changes.

### Run: `dotnet run`

Run is a development command. It selects the project, performs the required restore/build work unless told not to, then launches the application. It combines building and running for convenience; it is not merely another name for the compiler.

```powershell
dotnet run
```

Use `--project` when the current directory contains several projects or is not the project directory:

```powershell
dotnet run --project .\CSharpDemo.csproj
```

Program arguments go after `--`, which separates .NET CLI options from arguments for your own program:

```powershell
dotnet run --project .\CSharpDemo.csproj -- first second
```

Your `Main(string[] args)` can receive `first` and `second`. Without the separator, the CLI may interpret a value as one of its own options.

`dotnet run --no-build` launches the existing build output without compiling. Use it only when the output is already up to date. It is a common reason for seeing old behavior after an edit.

## What do I do after adding, changing, or removing a file?

For a normal SDK-style project like this one:

1. Save the edited source.
2. Run `dotnet run` to build as needed and start it, or run `dotnet build` and then execute the newly built output.
3. If building explicitly succeeded but the behavior is old, check which project, configuration, output path, and process you launched.

Adding or removing a `.cs` file under the project normally changes the next build's input set automatically. You generally do not need to edit the `.csproj`. If the project disables default compile items, has explicit include/exclude rules, or uses a non-SDK project format, update those rules as required.

The important distinction is **source versus built output**. Editing a source file does not patch an already-built DLL. The next successful build must compile the new input. If you launch an old DLL directly, it remains old regardless of what the editor displays.

### A careful troubleshooting sequence

If a removed or renamed file appears to cause a strange build, use a sequence that tells you where the problem is:

```powershell
dotnet clean
dotnet build
```

`clean` removes build outputs for the selected project/configuration; the following build regenerates them. Usually you do not need to manually erase `bin` and `obj`. First read the actual diagnostic: a duplicate type or method, an incorrect project path, or a compile error is not fixed by deleting caches.

If you have multiple projects, make the selection explicit:

```powershell
dotnet build .\CSharpDemo.csproj
dotnet run --project .\CSharpDemo.csproj
```

If a file is included twice, inspect explicit `Compile Include` entries and linked files. SDK default inclusion combined with manual inclusion can create duplicate compile items.

## Ways to execute a C# application

“Run the program” can mean several different workflows. Each begins with compiled output, but differs in how that output is selected and where the runtime comes from.

### 1. Run the project during development

```powershell
dotnet run
```

This is the easiest repeated development loop. The SDK builds when needed and launches the selected project. IDE Run/Debug buttons usually perform a similar build-and-launch workflow, with launch settings and debugger support.

### 2. Build, then run the framework-dependent DLL

```powershell
dotnet build
dotnet .\bin\Debug\net10.0\CSharpDemo.dll
```

This makes the build and execution steps explicit. The `dotnet` host starts the DLL using a compatible .NET runtime installed on the machine. The DLL does not usually contain the entire runtime.

### 3. Run the app host

Some builds create a platform-specific app-host executable. On Windows, if present, it can be launched directly:

```powershell
.\bin\Debug\net10.0\CSharpDemo.exe
```

The app host is a native launcher associated with the managed application assembly. Its availability depends on project/build settings; `OutputType=Exe` does not guarantee every platform-specific file will be present in every output.

### 4. Publish for deployment

`dotnet publish` prepares output for deployment. It is different from `dotnet build`: publish gathers the application and required deployment assets into a publish output directory.

```powershell
dotnet publish --configuration Release --output .\publish
```

A **framework-dependent** deployment expects a suitable .NET runtime on the destination machine. A **self-contained** deployment includes the runtime for a selected platform and is larger. Example:

```powershell
dotnet publish --configuration Release --runtime win-x64 --self-contained true --output .\publish\win-x64
```

Publishing must target the operating system/architecture you intend to deploy to. Options such as single-file packaging or trimming change deployment behavior and should be tested; they are not required for ordinary local development.

### 5. Run from an IDE or debugger

Visual Studio and VS Code with suitable C# tooling can build, launch, pass arguments, and attach a debugger. The IDE adds convenience; it does not replace the compiler or runtime. The selected project, configuration, and launch profile still matter.

### What about running a `.cs` file directly?

The classic .NET project workflow compiles C# source into an assembly before the runtime executes it. Some SDK versions and tools support file-based apps or interactive C# scripting, but those are distinct workflows with their own requirements. For this project and for reliable interview explanations, describe the project -> build -> assembly -> runtime flow rather than implying the runtime normally executes raw `.cs` text.

## Incremental build: why the second build is often faster

MSBuild evaluates inputs and outputs and can skip work it determines is already up to date. This is called an **incremental build**. It is a performance optimization, not a promise that every possible external change will be detected in every custom build setup. If you edit tracked source and invoke the normal `dotnet build` or `dotnet run`, the SDK checks the change and rebuilds the affected outputs.

Cleaning forces generated outputs to be removed before a later build recreates them. It can help diagnose stale generated state, but it is slower and should not replace reading compiler/build errors. `--no-build` deliberately skips rebuilding and therefore deliberately uses existing outputs.

## Build success is not behavior correctness

A successful build establishes that the selected inputs passed the build and compile steps. It does not prove:

- Every menu option behaves correctly.
- User input is valid or safely handled.
- Files exist at runtime or permissions allow access.
- The application works on another operating system.
- A deployment has all required configuration and dependencies.

Tests, debugging, and runtime checks establish different kinds of evidence. For an interview, explain build as a compile-time gate, not a guarantee that the application is bug-free.

## Common problems and a good first check

### “It still prints the old message.”

Check that the source was saved, the build succeeded, and you launched the correct project/configuration. Check whether you used `--no-build` or ran an old DLL from a different directory. Stop an already-running old process and launch again.

### “The build says it cannot find a type.”

Check spelling, namespace/imports, whether the source file is included, whether the type is accessible, and whether the project reference exists. Clean is unlikely to fix a missing reference or inaccessible type.

### “The file I deleted is still affecting compilation.”

Check whether another copy exists, whether the file is linked or explicitly included, and whether you are building a different project. A successful fresh build of the intended project normally removes deleted source from compilation.

### “The command runs a different app.”

Use `dotnet run --project path-to-project.csproj`. A solution directory can contain multiple runnable projects, so relying on the current directory may be ambiguous.

## Interview questions with answers

### What is the difference between restore, build, run, and publish?

Restore resolves dependencies. Build compiles inputs and creates build output. Run builds as needed and starts the application for development. Publish prepares application output for deployment, with options controlling runtime inclusion and target platform.

### Does `dotnet build` run my program?

No. It compiles and writes output. `dotnet run` also launches the application after the required build work.

### Does `dotnet run` always compile every file from scratch?

No. It performs required build work and can use incremental build checks. The `--no-build` option explicitly skips that work.

### What does `--no-restore` mean?

It tells the build not to perform package restore. It is appropriate only when valid restore assets already exist for the current project/configuration/target.

### Why does an old DLL still run after I delete a source file?

Deleting source does not delete a previously built output. The next successful build updates the output; if you directly run an old DLL without rebuilding, it can still contain the old compiled code.

### Does a `.dll` include the .NET runtime?

Usually a normal framework-dependent build output does not include the runtime. The host expects a compatible runtime installed. A self-contained publish includes runtime files for its target platform.

### How do you pass command-line arguments through `dotnet run`?

Put `--` between CLI options and program arguments, for example `dotnet run -- --verbose`. The arguments can be received by an entry point such as `Main(string[] args)`.

## Practice: observe each stage

From the directory containing this project file:

```powershell
dotnet restore
dotnet build
dotnet .\bin\Debug\net10.0\CSharpDemo.dll
dotnet run
```

Then make a harmless console-message edit, save it, and compare these two commands:

```powershell
dotnet run --no-build

```

The first deliberately launches existing output; the second performs the normal build/run workflow. Before trying this, stop any old process and ensure you are using the Debug output path for the project you edited.

Next add a `.cs` file containing a small helper type under the project directory and call it from `Program`. Build. Remove the helper and its call, build again, and explain why no project-file edit was needed. Do not add a second entry point to the same executable project.

## Mental model

`dotnet restore` prepares dependency assets. `dotnet build` turns the selected source/configuration into output. `dotnet run` performs the needed build work and launches it. `dotnet publish` prepares output for deployment. Source edits become executable behavior only after a successful build produces output that you actually launch.
