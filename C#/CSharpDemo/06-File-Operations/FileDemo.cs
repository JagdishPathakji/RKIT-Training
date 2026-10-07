using System;
using System.IO;
namespace CSharpDemo.FileDirectory;

/// <summary>Represents the FileDemo type.</summary>
public class FileDemo
{
    /// <summary>Runs the demonstration.</summary>
    public void Run()
    {
        ConsoleHelper.Clear();

        Console.WriteLine("=== FILE OPERATIONS DEMO ===");

        string filePath = "article.txt";
        string renamedPath = "article-renamed.txt";
        string movedPath = Path.Combine("Files", "article-renamed.txt");

        // 1. Create + Write
        File.WriteAllText(filePath, "Introduction to C#");
        Console.WriteLine("\n1. File created and written.");

        // 2. Read
        string content = File.ReadAllText(filePath);
        Console.WriteLine($"2. Read: {content}");

        // @ interpolation

        // 3. Append
        File.AppendAllText(filePath, "\nLearning .NET");
        Console.WriteLine("3. Content appended.");
        Console.WriteLine(File.ReadAllText(filePath));

        // 4. Exists
        Console.WriteLine(
            $"4. File exists: {File.Exists(filePath)}"
        );

        // 5. Rename
        File.Move(filePath, renamedPath);
        Console.WriteLine("5. File renamed.");

        // 6. Move
        Directory.CreateDirectory("Files");
        File.Move(renamedPath, movedPath);
        Console.WriteLine("6. File moved.");

        Console.WriteLine($"   Moved file exists: {File.Exists(movedPath)}");

        // 7. Delete
        File.Delete(movedPath);
        Console.WriteLine("7. File deleted.");

        Console.WriteLine($"   File exists: {File.Exists(movedPath)}");

        // 8. File information
        Console.WriteLine("\nFile methods used:");
        Console.WriteLine("File.WriteAllText()");
        Console.WriteLine("File.ReadAllText()");
        Console.WriteLine("File.AppendAllText()");
        Console.WriteLine("File.Exists()");
        Console.WriteLine("File.Move()");
        Console.WriteLine("File.Delete()");

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey(true);
    }
}
