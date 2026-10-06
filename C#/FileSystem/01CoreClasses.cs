using System;
using System.IO;
using System.Diagnostics;

/*
(1.1) The System.IO Namespace
Everything related to file system operations in C# lives primarily in System.IO, with some extensions in:
-- System.IO.Compression (zip)
-- System.IO.MemoryMappedFiles (large file mapping)
-- System.IO.Pipes (IPC)

--------------------------------------------

(1.2) Static vs Instance Classes
-- Static Classes :- File, Directory, Path
-- Instance Classes :- FileInfo, DirectoryInfo
*/

class Program {

    static void Main() {

        FileVsFileInfo();
        DirectoryVsDirectoryInfo();
    }

    static void FileVsFileInfo() {

        // path of file
        string path = "demo.txt";

        // write to file
        File.WriteAllText(path, "Hello File System !");



        // --- using static File class ---
        // every call below independently checks that path & permissions
        Console.WriteLine("---Using static File class---");
        bool exists = File.Exists(path);
        DateTime created = File.GetCreationTime(path);
        DateTime lastWrite = File.GetLastWriteTime(path);
        long length = new FileInfo(path).Length; // file class has no direct .Length
        Console.WriteLine($"Exists: {exists}, Created: {created}, LastWrite: {lastWrite}, Length: {length}");





        // --- using instance FileInfo class ---
        Console.WriteLine("--- Using instance FileInfo class---");
        FileInfo fileInfo = new FileInfo(path);
        // info is cached after first access, so subsequent property reads are faster.
        Console.WriteLine($"Exists: {fileInfo.Exists}");
        Console.WriteLine($"Created: {fileInfo.CreationTime}");
        Console.WriteLine($"LastWrite: {fileInfo.LastWriteTime}");
        Console.WriteLine($"Length: {fileInfo.Length} bytes");
        Console.WriteLine($"Extension: {fileInfo.Extension}");
        Console.WriteLine($"FullName: {fileInfo.FullName}");
        Console.WriteLine($"DirectoryName: {fileInfo.DirectoryName}");
        Console.WriteLine($"IsReadOnly: {fileInfo.IsReadOnly}");



        // performance comparision between File and FileInfo
        var sw1 = Stopwatch.StartNew();
        for (int i = 0; i < 10000; i++) {
            bool e = File.Exists(path); // re-checks OS/security every time
        }
        sw1.Stop();

        Console.WriteLine($"File.Exists (static, repeated): {sw1.ElapsedMilliseconds} ms");

        var sw2 = Stopwatch.StartNew();
        var fi = new FileInfo(path);
        for (int i = 0; i < 10000; i++)
        {
            bool e = fi.Exists; // Note: FileInfo also refreshes on some properties,
            // but avoids repeated object construction overhead
            // (avoids redundant OS calls, and reads are cached until you call .Refresh()).
        }
        sw2.Stop();
        Console.WriteLine($"FileInfo.Exists (instance, repeated): {sw2.ElapsedMilliseconds} ms");
    }   

    static void DirectoryVsDirectoryInfo() {

        string dirPath = "DemoFolder";

        // ---- Static Directory class ----
        Directory.CreateDirectory(dirPath);
        Console.WriteLine($"Created: {Directory.Exists(dirPath)}");

        File.WriteAllText(Path.Combine(dirPath, "a.txt"), "A");
        File.WriteAllText(Path.Combine(dirPath, "b.txt"), "B");

        string[] files = Directory.GetFiles(dirPath);
        Console.WriteLine("Files (static Directory):");
        foreach (var f in files) Console.WriteLine($"  {f}");


        // ---- Instance DirectoryInfo class ----
        DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
        Console.WriteLine($"\nDirectory: {dirInfo.FullName}");
        Console.WriteLine($"Created: {dirInfo.CreationTime}");
        Console.WriteLine($"Parent: {dirInfo.Parent?.FullName}");
        Console.WriteLine($"Root: {dirInfo.Root}");


        Console.WriteLine("Files (instance DirectoryInfo):");
        foreach (FileInfo file in dirInfo.GetFiles()) {
            Console.WriteLine($"  {file.Name} - {file.Length} bytes");
        }

        // cleanup
        Directory.Delete(dirPath, recursive: true);
    }
}

/* 
(1.3) Important Caching Concept
If the file might have changed since you created the FileInfo/DirectoryInfo object (especially by another process or another code path), call .Refresh() before reading properties.

Code:-
FileInfo fi = new FileInfo("test.txt");
Console.WriteLine(fi.Exists); // False (file doesn't exist yet)

File.WriteAllText("test.txt", "content"); // created externally

Console.WriteLine(fi.Exists); // Still False! Cached value.
fi.Refresh();
Console.WriteLine(fi.Exists); // True now — refreshed
*/