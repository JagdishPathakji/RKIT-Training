using System;
using System.IO;
namespace CSharpDemo.FileDirectory;

// add commenting as per guidelines

/*
Path.Combine and Path.Join both build a path from parts, but they handle a later rooted path differently.


1. Path.Combine:- Combine treats D:\DB as a complete path, so it discards the earlier part.

Path.Combine(@"C:\Training", @"DB");
// C:\Training\DB

Path.Combine(@"C:\Training", @"D:\DB");
// D:\DB

2. Path.Join:- Join concatenates the parts; it doesn’t treat the second rooted path as a replacement.

Path.Join(@"C:\Training", @"D:\DB");
// C:\Training\D:\DB
*/

public class DirectoryDemo
{
    public void Run()
    {
        ConsoleHelper.Clear();

        Console.WriteLine("=== DIRECTORY OPERATIONS DEMO ===");

        string directoryPath = "Articles";
        string renamedPath = "KnowledgeBase";
        string movedPath = Path.Combine("Data", "KnowledgeBase");

        // 1. Create
        Directory.CreateDirectory(directoryPath);
        Console.WriteLine("\n1. Directory created.");

        // 2. Exists
        Console.WriteLine(
            $"2. Directory exists: " +
            $"{Directory.Exists(directoryPath)}"
        );

        // 3. Rename
        Directory.Move(directoryPath, renamedPath);
        Console.WriteLine("3. Directory renamed.");

        // 4. Move
        Directory.CreateDirectory("Data");

        Directory.Move(renamedPath, movedPath);
        Console.WriteLine("4. Directory moved.");

        Console.WriteLine(
            $"   Moved directory exists: " +
            $"{Directory.Exists(movedPath)}"
        );

        // 5. Delete
        Directory.Delete(movedPath);
        Console.WriteLine("5. Directory deleted.");

        Console.WriteLine(
            $"   Directory exists: " +
            $"{Directory.Exists(movedPath)}"
        );

        Console.WriteLine("\nDirectory methods used:");
        Console.WriteLine("Directory.CreateDirectory()");
        Console.WriteLine("Directory.Exists()");
        Console.WriteLine("Directory.Move()");
        Console.WriteLine("Directory.Delete()");

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey(true);
    }
}