# C# Interview Learning Roadmap

This folder is a self-contained path from opening a C# project for the first time to explaining common .NET concepts at interview depth. The numbered files are ordered intentionally; each one can also be read on its own.

## Recommended order

1. [Project anatomy](01-Project-Anatomy.md): SDK-style projects, `.csproj`, source files, `bin`, and `obj`.
2. [Build and run](02-Build-And-Run.md): restore, compile, execute, publish, and get changes into the next run.
3. [C# program foundations](03-CSharp-Program-Foundations.md): entry points, types, variables, methods, and control flow.
4. [Scope and accessibility](04-Scope-And-Accessibility.md): where names are usable, who can access members, and encapsulation.
5. [Namespaces and libraries](05-Namespaces-And-Libraries.md): code organization, references, assemblies, and NuGet.
6. [Enumerations](06-Enumerations.md): named constants, underlying values, flags, and API design.
7. [DataTable](07-DataTable.md): in-memory relational data, schema, rows, constraints, and DataView.
8. [Date and time](08-Date-And-Time.md): dates, times, offsets, UTC, parsing, formatting, and time zones.
9. [Math and numbers](09-Math-And-Numbers.md): numeric types, precision, overflow, rounding, and randomness.
10. [Strings](10-Strings.md): immutability, comparison, formatting, Unicode, and efficient construction.
11. [Files and directories](11-Files-And-Directories.md): paths, text, streams, async I/O, errors, and safe file handling.

## How to use each guide

Read the foundation first, then trace each example line by line. Try the exercises without looking at the answers you expect, and practice answering the interview questions aloud. For interview preparation, explain not only *what* an API does but also its tradeoffs, failure cases, and when you would choose another approach.

The examples use ordinary C# and the .NET SDK. They are teaching examples, not a claim that every technique is appropriate for every production system. Project-specific facts in these guides refer to this workspace's SDK-style executable targeting .NET 10.
