# 11 - Files and Directories in .NET

## What is file I/O?

**File input/output (I/O)** means reading or writing data in the filesystem. A filesystem organizes named files and directories and enforces operating-system rules such as permissions and sharing. C# code uses .NET APIs to request operations; the OS can reject them or the state can change while the program runs.

There are three ideas to keep separate:

- A **path** names a file or directory.
- **File contents** are the bytes stored at that location.
- **I/O APIs** perform operations on those contents and may fail.

Do not assume a file exists, that the process has permission, or that a path means the same thing on every machine.

## Paths: absolute, relative, and platform-aware

An **absolute path** identifies a location from a filesystem root. A **relative path** is interpreted against a base directory, usually the process's current working directory. That current directory can vary depending on whether the program was launched from an IDE, terminal, scheduled job, service, or test runner.

Use `Path.Combine` to join path components instead of manually inserting `\` or `/`:

```csharp
string folder = Path.Combine(Path.GetTempPath(), "CSharpDemo");
string filePath = Path.Combine(folder, "notes.txt");
```

Useful APIs:

- `Path.Combine`: join path components using platform conventions.
- `Path.GetFullPath`: normalize a path to an absolute path using a base directory.
- `Path.GetFileName` / `GetDirectoryName`: extract a file or directory component.
- `Path.GetExtension`: obtain the file-name extension.
- `Path.GetTempPath`: get the current user's temporary directory.
- `Path.GetRandomFileName`: create a random file-name string, not the file itself.

`Environment.CurrentDirectory` is the process's working directory. `AppContext.BaseDirectory` is the application base directory. Neither one means “the user's Documents directory.” For user-owned persistent data, use an appropriate known-folder/platform API or application configuration rather than assuming the current directory.

Filesystem case sensitivity and permitted path characters vary by platform and filesystem. Avoid comparing paths with a hard-coded case rule unless the target environment defines it.

## `File` and `Directory`: convenient whole-operation helpers

`System.IO.File` and `System.IO.Directory` expose static helper methods. For small bounded text files, whole-file APIs are convenient:

```csharp
using System.IO;
using System.Text;

string folder = Path.Combine(Path.GetTempPath(), "CSharpDemo");
Directory.CreateDirectory(folder); // Creates parents; succeeds if it already exists.

string path = Path.Combine(folder, "notes.txt");
await File.WriteAllTextAsync(path, "First line\nSecond line\n", Encoding.UTF8);
string contents = await File.ReadAllTextAsync(path, Encoding.UTF8);
```

Common operations:

- `File.ReadAllText` / `ReadAllTextAsync`: read all text into one string.
- `File.ReadAllLines` / `ReadAllLinesAsync`: read all lines into an array.
- `File.ReadLines`: enumerate lines without first creating an array of every line.
- `File.WriteAllText` / `WriteAllTextAsync`: create or overwrite a text file.
- `File.AppendAllText` / `AppendAllTextAsync`: add text to the end, creating the file if needed.
- `File.Copy`, `Move`, and `Delete`: copy, move, and delete files.
- `Directory.CreateDirectory`: create the named directory and missing parents.
- `Directory.GetFiles` / `EnumerateFiles`: get file paths in a directory.
- `Directory.Delete`: remove a directory; recursive deletion can remove its entire tree.

Know whether an API overwrites, appends, or fails when a destination exists. `WriteAllText` overwrites an existing file. Choose a collision policy intentionally.

### Encoding and line endings

Text files store bytes, so reading/writing text requires an **encoding** that maps characters to bytes. UTF-8 is a common default/interchange format. If a file contract specifies an encoding, pass it explicitly. Do not assume every legacy file is UTF-8.

Line endings can be LF or CRLF and may vary by platform/file. `Environment.NewLine` gives the platform newline. If a protocol requires a particular newline, follow that protocol instead. Avoid changing line endings accidentally when exact bytes matter.

## Check-then-act is a race

`File.Exists(path)` can be useful for display or a best-effort check, but this pattern is unsafe as a guarantee:

```csharp
if (!File.Exists(path))
{
	File.WriteAllText(path, "data");
}
```

Another process can create or replace the file between the check and the write. This is a **time-of-check to time-of-use (TOCTOU)** race. Prefer an operation that directly expresses the needed behavior, such as opening with `FileMode.CreateNew` when the requirement is “create only if absent,” and handle the failure if it already exists.

`File.Exists` also does not prove the process can successfully read/write the file. Handle errors from the actual operation.

## Streams: process data incrementally

A **stream** is an abstraction for reading or writing a sequence of bytes over time. Streams let an application process a large file without loading the entire file into memory. `FileStream` accesses file bytes; `StreamReader` and `StreamWriter` decode/encode text on top of streams.

```csharp
using System.IO;
using System.Text;

await using var fileStream = new FileStream(
	path,
	FileMode.Open,
	FileAccess.Read,
	FileShare.Read,
	bufferSize: 4096,
	useAsync: true);

using var reader = new StreamReader(fileStream, Encoding.UTF8);
string? line;
while ((line = await reader.ReadLineAsync()) is not null)
{
	ProcessLine(line);
}
```

`FileMode` defines how opening behaves. Common modes include `Open` (existing file required), `Create` (create or overwrite), `CreateNew` (fail if already present), `Append` (write at end), `OpenOrCreate`, and `Truncate` (existing file required and emptied). `FileAccess` selects read/write capability; `FileShare` controls what other opens are allowed while the handle is in use.

Use `using` for synchronous disposable resources and `await using` for asynchronous disposal when supported. Disposal closes handles even if an exception occurs. Do not depend on garbage collection timing to close a stream.

## Synchronous versus asynchronous I/O

Synchronous APIs are simple and can be suitable for small local command-line tasks. Asynchronous I/O (`ReadAsync`, `WriteAsync`, and async `File` helpers) lets a thread do other work while waiting for I/O completion. It is useful for responsive UI or scalable server code. It does not make the storage device itself faster, and adding async to a tiny one-off operation may not provide a practical benefit.

Use `await` to observe completion and exceptions. Do not start an asynchronous write and exit without awaiting it. Pass cancellation tokens when the surrounding operation supports cancellation and stopping work is part of the design.

## File and directory metadata

`FileInfo` and `DirectoryInfo` are object-oriented wrappers for metadata and operations; `File` and `Directory` provide static convenience methods. Use `FileInfo.Length`, `LastWriteTime`, and `Exists` when those properties are useful, while remembering that metadata can become stale after another process changes the file.

For directory enumeration:

```csharp
foreach (string candidate in Directory.EnumerateFiles(folder, "*.txt"))
{
	Console.WriteLine(Path.GetFileName(candidate));
}
```

`EnumerateFiles` can yield paths incrementally; `GetFiles` materializes an array. For a large tree, incremental enumeration can reduce memory use. Recursive traversal can fail partway through due to permissions, disappearing directories, or invalid links; decide whether to stop, skip, or report those failures.

## Errors and resource safety

Filesystem operations can fail for many reasons:

- `FileNotFoundException` or `DirectoryNotFoundException`: expected path component is missing.
- `UnauthorizedAccessException`: access is denied by permissions or policy.
- `IOException`: general I/O failure, sharing violation, device problem, and related cases.
- `ArgumentException` / `ArgumentNullException`: invalid path or API argument.
- `PathTooLongException`: path exceeds supported limits in the relevant environment/API.

Catch an exception only where the program can recover, choose another action, or report useful context. Catch specific types when they require different handling. An empty `catch` hides lost data and broken assumptions. Cleanup belongs in `using`/`finally` so it still happens when reading/writing fails.

For important file replacement, a common design is to write a temporary file in the same directory, flush and close it, and then replace/move it using an operation supported by the target platform/filesystem. This can reduce the chance of exposing a partially written destination, but atomicity and durability depend on the operation and filesystem. Define a recovery policy for critical data; do not assume all moves are universally atomic.

## Untrusted paths and directory traversal

Never assume a path from a user or external request is safe. Inputs containing `..`, rooted paths, alternate separators, or symbolic links can escape an intended directory if handled carelessly. A string-prefix check is not sufficient: a sibling directory can share the same textual prefix as the allowed root.

At minimum, normalize both root and candidate with `Path.GetFullPath`, derive a relative path with `Path.GetRelativePath`, and reject results that escape the allowed root. Also consider symbolic links/reparse points: lexical normalization does not resolve links, and a link inside the root can point outside it. High-security code needs an OS-aware policy and careful handling of races between validation and opening.

Authorization must be checked for the actual operation. Do not rely on `File.Exists` as a permission check. When possible, avoid allowing callers to provide arbitrary filesystem paths; accept a constrained identifier and construct a path under a controlled root.

## Complete runnable example

This asynchronous console app creates a uniquely named file under the operating system's temporary directory, writes UTF-8 text, reads it back, and reports a useful I/O error. It does not overwrite a pre-existing file because its name contains a GUID and the write uses `CreateNew`:

```csharp
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

internal class Program
{
	private static async Task Main()
	{
		string folder = Path.Combine(Path.GetTempPath(), "CSharpDemo-FileLesson");
		string fileName = $"notes-{Guid.NewGuid():N}.txt";
		string path = Path.Combine(folder, fileName);

		try
		{
			Directory.CreateDirectory(folder);

			await using (var stream = new FileStream(
				path,
				FileMode.CreateNew,
				FileAccess.Write,
				FileShare.None,
				bufferSize: 4096,
				useAsync: true))
			await using (var writer = new StreamWriter(stream, new UTF8Encoding(false)))
			{
				await writer.WriteLineAsync("C# file I/O");
				await writer.WriteLineAsync("This file was created by the demo.");
			}

			string contents = await File.ReadAllTextAsync(path, Encoding.UTF8);
			Console.WriteLine($"Created: {path}");
			Console.Write(contents);
		}
		catch (UnauthorizedAccessException exception)
		{
			Console.WriteLine($"Access denied: {exception.Message}");
		}
		catch (IOException exception)
		{
			Console.WriteLine($"File operation failed: {exception.Message}");
		}
	}
}
```

The file is intentionally left in the temporary directory so you can inspect it. The generated directory is under the system temp location, not the project source folder. In a real application, choose a lifecycle and cleanup policy appropriate to the data.

Important steps:

1. `Path.Combine` builds paths without hard-coding separators.
2. `Guid.NewGuid()` avoids choosing a predictable existing name for this demo; `CreateNew` still enforces “must not already exist.”
3. `Directory.CreateDirectory` creates the directory if needed and succeeds if it already exists.
4. `FileStream` opens with explicit mode, access, sharing, and asynchronous options.
5. `await using` disposes the stream/writer reliably.
6. `File.ReadAllTextAsync` is appropriate because this generated demo file is small.
7. Specific catches give a useful failure message without silently hiding the error.

## Common mistakes

- Hard-coding path separators with string concatenation.
- Assuming the working directory is the project or application directory.
- Using `WriteAllText` without realizing it overwrites an existing file.
- Checking `File.Exists` and assuming the next operation is guaranteed to succeed.
- Loading a huge file entirely into memory when a stream would work.
- Forgetting to dispose a stream or not awaiting async operations.
- Catching all exceptions and ignoring them.
- Treating a normalized path as safe despite symbolic links or races.
- Assuming `File.Move` is always atomic across filesystems/platforms.
- Confusing a path's extension with proof of its file content/type.

## Interview questions with answers

### Why use `Path.Combine`?

It joins path components using platform conventions and avoids fragile manual separator handling. It does not validate whether a path is safe or permitted.

### Why is `File.Exists` not enough before writing?

The filesystem can change between check and use; existence does not guarantee access or a successful write. Express the intended open behavior with a file mode and handle the operation's result.

### When use streams instead of `ReadAllText`?

For large data, incremental processing, lower memory use, or controlled asynchronous operations. Whole-file APIs are simpler for small bounded files.

### What does `using` do for a stream?

It ensures disposal on normal exit or exception, releasing the stream and underlying OS handle. `await using` supports asynchronous disposal.

### Is `Environment.CurrentDirectory` always the application folder?

No. It is the process working directory and depends on how the application was launched. `AppContext.BaseDirectory` is a different concept.

### What is the difference between `Create` and `CreateNew`?

`Create` creates or overwrites. `CreateNew` creates only if the file does not already exist and otherwise fails.

### Does asynchronous file I/O make a disk faster?

No. It helps the application avoid blocking a thread while waiting for I/O, which is valuable for responsiveness/scalability. It does not reduce device latency by itself.

### Why is a string-prefix path check unsafe?

An attacker can use `..`, separator tricks, casing differences, or a sibling path with the same textual prefix. Normalize and compare path relationships, and account for symbolic links and race conditions.

## Practice

1. Create a directory under `Path.GetTempPath`, write a small UTF-8 file, and read it back.
2. Try `FileMode.Create` and `FileMode.CreateNew` when the destination already exists; compare the behavior.
3. Read a large text file line by line with `StreamReader` rather than `ReadAllText`.
4. Enumerate only `.txt` files and compare `EnumerateFiles` with `GetFiles`.
5. Explain why a relative path can work in the terminal but fail when launched from an IDE/service.
6. Design a path policy for an untrusted filename and explain how symbolic links affect it.

## Mental model

A path is a request to locate something; it is not proof the target exists or is safe. Use `Path` to construct/inspect names, `File`/`Directory` for convenient operations, and streams for incremental data. Select overwrite/share/encoding policies explicitly, dispose resources, await asynchronous work, and handle failures where recovery is possible.
