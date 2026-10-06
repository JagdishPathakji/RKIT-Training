using System;
using System.IO;
/*
The `Path` class is purely string manipulation. It does not touch the file system at all. It does not check if a file exists, does not create anything. It just works with path strings intelligently, respecting OS conventions.
*/


/*
(2.1) Why not just concatenate strings ?
It is bad because:
-- fragile
-- breaks cross platform
*/

// Core Path Methods
class PathDemo {

    static void Main() {

        // sample full path for demo
        string samplePath = @"C:\Projects\MyApp\bin\Debug\report_2024.pdf";



        Console.WriteLine("=== Decomposing a Path ===");
        Console.WriteLine($"Full path:        {samplePath}");
        Console.WriteLine($"GetFileName:       {Path.GetFileName(samplePath)}"); // report_2024.pdf
        Console.WriteLine($"GetFileNameWithoutExtension: {Path.GetFileNameWithoutExtension(samplePath)}"); // report_2024
        Console.WriteLine($"GetExtension:      {Path.GetExtension(samplePath)}"); // .pdf
        Console.WriteLine($"GetDirectoryName:  {Path.GetDirectoryName(samplePath)}"); // C:\Projects\MyApp\bin\Debug
        Console.WriteLine($"GetPathRoot:       {Path.GetPathRoot(samplePath)}"); // C:\



        Console.WriteLine("\n=== Combining Paths ===");
        string combined1 = Path.Combine("C:\\Data", "Logs", "app.log");
        string combined2 = Path.Combine("C:\\Data\\", "Logs", "app.log"); // trailing slash — no problem
        Console.WriteLine($"Combine 1: {combined1}");
        Console.WriteLine($"Combine 2: {combined2}");

        // Path.Join (newer, .NET Core+) is similar but does NOT handle rooted-path override behavior
        string joined = Path.Join("C:\\Data", "Logs", "app.log");
        Console.WriteLine($"Join:      {joined}");



        Console.WriteLine("\n=== Gotcha: Combine with a Rooted Second Path ===");
        // If any segment after the first is an ABSOLUTE path, Combine discards everything before it!
        string trap = Path.Combine("C:\\Data", "Logs", "D:\\OtherDrive\\file.txt");

        // Join does not handle rooted-path override behavior
        string trap1 = Path.Join("C:\\Data", "Logs", "D:\\OtherDrive\\file.txt");

        Console.WriteLine($"Result: {trap}"); // D:\OtherDrive\file.txt  <-- "C:\Data\Logs" got dropped!
        Console.WriteLine($"Result: {trap1}"); //  C:\Data\Logs\D:\OtherDrive\file.txt



        Console.WriteLine("\n=== Changing Extensions ===");
        string newExt = Path.ChangeExtension(samplePath, ".docx");
        Console.WriteLine($"Changed extension: {newExt}");
        string removedExt = Path.ChangeExtension(samplePath, null);
        Console.WriteLine($"Removed extension:  {removedExt}");



        Console.WriteLine("\n=== Absolute vs Relative ===");
        string relative = @"..\Data\file.txt";
        Console.WriteLine($"IsPathRooted('{relative}'): {Path.IsPathRooted(relative)}");        // False
        Console.WriteLine($"IsPathRooted('{samplePath}'): {Path.IsPathRooted(samplePath)}");    // True
        Console.WriteLine($"GetFullPath (resolves relative to CWD): {Path.GetFullPath(relative)}");




        /*
            (1) DirectorySeparatorChar — the character that separates folders.
                -- Windows: \ (backslash)
                -- Linux/macOS: / (forward slash)
            (2) AltDirectorySeparatorChar — the "alternate" separator.
                -- Windows: / (forward)
                -- Linux/macOS: / (no alternate provided)
            (3) PathSeparator — separates multiple paths in a single string.
                -- Windows: ; (semicolon)
                -- Linux/macOS: : (colon)
            (4) VolumeSeparatorChar — separates the drive letter from the rest.
                -- Windows: : (C:\)
                -- Linux/macOS: Unix does not have drive letters, everything is under one root /

        */
        Console.WriteLine("\n=== Separator Characters (Cross-Platform) ===");
        Console.WriteLine($"DirectorySeparatorChar: '{Path.DirectorySeparatorChar}'");     // \ on Windows, / on Linux/macOS
        Console.WriteLine($"AltDirectorySeparatorChar: '{Path.AltDirectorySeparatorChar}'"); // / on Windows, / on Unix
        Console.WriteLine($"PathSeparator: '{Path.PathSeparator}'");                        // ; on Windows, : on Unix (used in PATH env var)
        Console.WriteLine($"VolumeSeparatorChar: '{Path.VolumeSeparatorChar}'");            // : on Windows
    }
}