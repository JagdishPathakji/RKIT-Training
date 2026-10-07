using System;
using System.IO;

namespace CSharpDemo.FileSystem;

/// <summary>Represents the DirDemoDepth type.</summary>
public static class DirDemoDepth
{
    /// <summary>Runs the demonstration.</summary>
    public static void Run()
    {
        string workingDirectory = Directory.GetCurrentDirectory();
        string demoRootPath = Path.Combine(
            workingDirectory,
            $"KnowledgeBaseDirectoryDemo_{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid():N}");
        Console.WriteLine("=== DIRECTORY HANDLING DEMO ===");
        Console.WriteLine($"Working directory: {workingDirectory}");

        try
        {
            DirectoryCreation(demoRootPath);
            InspectDirectoryInfo(demoRootPath);
            DirectoryEnumeration(demoRootPath);
            DirectoryMove(demoRootPath);
            DirectoryDeletion(demoRootPath);
            MissingDirectory(demoRootPath);

            Console.WriteLine("\nDemo complete.");
        }
        catch (UnauthorizedAccessException exception)
        {
            Console.WriteLine($"Permission denied: {exception.Message}");
        }
        catch (IOException exception)
        {
            Console.WriteLine($"Directory operation failed: {exception.Message}");
        }
        catch (ArgumentException exception)
        {
            Console.WriteLine($"Invalid directory path or argument: {exception.Message}");
        }
        catch (NotSupportedException exception)
        {
            Console.WriteLine($"Directory operation is not supported: {exception.Message}");
        }
    }

    private static void DirectoryCreation(string demoRootPath)
    {
        Console.WriteLine("\n--- Directory: create and check existence ---");

        string draftsPath = Path.Combine(demoRootPath, "DraftArticles");
        string publishedPath = Path.Combine(demoRootPath, "PublishedArticles");

        Directory.CreateDirectory(draftsPath);
        Directory.CreateDirectory(publishedPath);

        Console.WriteLine($"Created: {draftsPath}");
        Console.WriteLine($"Drafts directory exists: {Directory.Exists(draftsPath)}");
        Console.WriteLine($"Published directory exists: {Directory.Exists(publishedPath)}");

        // CreateDirectory is safe to call when the directory already exists.
        Directory.CreateDirectory(draftsPath);
        Console.WriteLine("Calling CreateDirectory again does not fail if it already exists.");

        // Sample files give the directory listing realistic knowledge-base data.
        File.WriteAllText(
            Path.Combine(draftsPath, "sql-joins-draft.txt"),
            "Draft article: Understanding SQL Joins");
        File.WriteAllText(
            Path.Combine(draftsPath, "transactions-draft.txt"),
            "Draft article: Using Database Transactions");
        File.WriteAllText(
            Path.Combine(publishedPath, "database-indexes.txt"),
            "Published article: A Practical Guide to Database Indexes");
    }

    private static void InspectDirectoryInfo(string demoRootPath)
    {
        Console.WriteLine("\n--- DirectoryInfo: inspect one directory ---");

        DirectoryInfo directoryInfo = new(
            Path.Combine(demoRootPath, "DraftArticles"));

        Console.WriteLine($"Name: {directoryInfo.Name}");
        Console.WriteLine($"Full path: {directoryInfo.FullName}");
        Console.WriteLine($"Exists: {directoryInfo.Exists}");
        Console.WriteLine($"Creation time: {directoryInfo.CreationTime}");
        Console.WriteLine($"Parent: {directoryInfo.Parent?.Name}");

        DirectoryInfo archiveInfo = directoryInfo.CreateSubdirectory("Archive");
        Console.WriteLine($"Created subdirectory: {archiveInfo.FullName}");

        // Refresh updates cached values if the directory changed after this object was created.
        directoryInfo.Refresh();
        Console.WriteLine($"Exists after refresh: {directoryInfo.Exists}");
    }

    private static void DirectoryEnumeration(string demoRootPath)
    {
        Console.WriteLine("\n--- Directory: list child directories and files ---");

        string draftsPath = Path.Combine(demoRootPath, "DraftArticles");

        Console.WriteLine("Child directories:");
        foreach (string childDirectory in Directory.GetDirectories(draftsPath))
        {
            // Path.GetFileName() isn't limited to files. It extracts the final component of a path.
            Console.WriteLine($"  {Path.GetFileName(childDirectory)}");
        }

        Console.WriteLine("Files as an array using GetFiles:");
        string[] filePaths = Directory.GetFiles(draftsPath, "*.txt");
        foreach (string filePath in filePaths)
        {
            Console.WriteLine($"  {Path.GetFileName(filePath)}");
        }

        Console.WriteLine("Files as they are enumerated using EnumerateFiles:");
        foreach (string filePath in Directory.EnumerateFiles(draftsPath, "*.txt"))
        {
            Console.WriteLine($"  {Path.GetFileName(filePath)}");
        }

        Console.WriteLine("FileInfo objects from DirectoryInfo:");
        DirectoryInfo directoryInfo = new(draftsPath);
        foreach (FileInfo fileInfo in directoryInfo.EnumerateFiles("*.txt"))
        {
            Console.WriteLine($"  {fileInfo.Name} | {fileInfo.Length} bytes");
        }
    }

    private static void DirectoryMove(string demoRootPath)
    {
        Console.WriteLine("\n--- Directory: move or rename ---");

        string draftsPath = Path.Combine(demoRootPath, "DraftArticles");
        string renamedPath = Path.Combine(demoRootPath, "ArticlesUnderReview");

        Directory.Move(draftsPath, renamedPath);

        Console.WriteLine($"Renamed/moved directory to: {renamedPath}");
        Console.WriteLine($"Old path exists: {Directory.Exists(draftsPath)}");
        Console.WriteLine($"New path exists: {Directory.Exists(renamedPath)}");
    }

    private static void DirectoryDeletion(string demoRootPath)
    {
        Console.WriteLine("\n--- DirectoryInfo: delete the demo data ---");

        // This is the unique demo root created for this run.
        DirectoryInfo demoRootInfo = new(demoRootPath);
        demoRootInfo.Delete(recursive: true);

        Console.WriteLine($"Deleted this demo's directory tree: {demoRootPath}");
        Console.WriteLine($"Demo directory still exists: {Directory.Exists(demoRootPath)}");
    }

    private static void MissingDirectory(string demoRootPath)
    {
        Console.WriteLine("\n--- Handling a missing directory ---");

        string missingPath = Path.Combine(demoRootPath, "DoesNotExist");

        try
        {
            Directory.GetFiles(missingPath);
        }
        catch (DirectoryNotFoundException exception)
        {
            Console.WriteLine(
                $"Expected error handled: directory was not found ({exception.Message})");
        }
    }
}
