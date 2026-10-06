// A file allows us to store data permanently.

using System;
using System.IO;

/*
Important classes:

1. File
2. FileInfo
3. Directory
4. DirectoryInfo
5. Path
6. StreamReader
7. StreamWriter
8. FileStream
*/

// File.Create("data.txt"); to only create a file

class Program
{
    static void Main()
    {
        Console.WriteLine("========== FILE OPERATIONS ==========\n");

        // 1. Current Working Directory

        Console.WriteLine("CWD:");
        Console.WriteLine(Directory.GetCurrentDirectory());


        // 2. File.WriteAllText()

        Console.WriteLine("\n--- WriteAllText ---");
        string filePath = "data.txt";
        File.WriteAllText(filePath, "Hello from C#");
        Console.WriteLine("File created/written successfully.");


        // 3. File.ReadAllText()

        Console.WriteLine("\n--- ReadAllText ---");
        string content = File.ReadAllText(filePath);
        Console.WriteLine("Content:");
        Console.WriteLine(content);


        // 4. File.AppendAllText()

        Console.WriteLine("\n--- AppendAllText ---");
        File.AppendAllText(filePath, "\nHello from Jagdish");
        content = File.ReadAllText(filePath);
        Console.WriteLine("Content after append:");
        Console.WriteLine(content);


        // 5. File.Exists()

        Console.WriteLine("\n--- Exists ---");
        bool exists = File.Exists(filePath);
        Console.WriteLine("Does data.txt exist? " + exists);


        // 6. File.Copy()
        Console.WriteLine("\n--- Copy ---");
        string backupPath = "backup.txt";
        File.Copy(filePath, backupPath, true);
        Console.WriteLine("data.txt copied to backup.txt");


        // 7. File.Move()
        Console.WriteLine("\n--- Move / Rename ---");
        string renamedPath = "backup2.txt";
        File.Move(backupPath, renamedPath, true);
        Console.WriteLine("backup.txt renamed/moved to backup2.txt");


        // 8. FileInfo
        Console.WriteLine("\n========== FILEINFO ==========\n");
        FileInfo file = new FileInfo(filePath);

        Console.WriteLine("Name          : " + file.Name);
        Console.WriteLine("Full Name     : " + file.FullName);
        Console.WriteLine("Extension     : " + file.Extension);
        Console.WriteLine("Length        : " + file.Length + " bytes");
        Console.WriteLine("Exists        : " + file.Exists);
        Console.WriteLine("Creation Time : " + file.CreationTime);
        Console.WriteLine("Last Write    : " + file.LastWriteTime);


        // 9. Directory

        Console.WriteLine("\n========== DIRECTORY ==========\n");
        string directoryPath = "MyFolder";
        Directory.CreateDirectory(directoryPath);
        Console.WriteLine("Directory created: " + directoryPath);
        Console.WriteLine("Directory exists: " +
                          Directory.Exists(directoryPath));


        // Create files inside directory
        File.WriteAllText(
            Path.Combine(directoryPath, "file1.txt"),
            "This is file 1"
        );

        File.WriteAllText(
            Path.Combine(directoryPath, "file2.txt"),
            "This is file 2"
        );

        Console.WriteLine("\nFiles inside MyFolder:");

        string[] files = Directory.GetFiles(directoryPath);

        foreach (string currentFile in files)
        {
            Console.WriteLine(currentFile);
        }


        // 10. DirectoryInfo

        Console.WriteLine("\n========== DIRECTORYINFO ==========\n");

        DirectoryInfo directory =
            new DirectoryInfo(directoryPath);

        Console.WriteLine("Name      : " + directory.Name);
        Console.WriteLine("Full Name : " + directory.FullName);
        Console.WriteLine("Exists    : " + directory.Exists);

        Console.WriteLine("\nFiles using DirectoryInfo:");

        FileInfo[] fileInfos = directory.GetFiles();

        foreach (FileInfo currentFile in fileInfos)
        {
            Console.WriteLine(
                currentFile.Name +
                " - " +
                currentFile.Length +
                " bytes"
            );
        }


        // 11. Delete Files

        Console.WriteLine("\n========== DELETE ==========\n");

        File.Delete(filePath);

        Console.WriteLine(
            "data.txt deleted: " +
            !File.Exists(filePath)
        );


        // 12. Delete Directory

        Console.WriteLine("\nDeleting MyFolder and its contents...");

        // The second parameter controls whether the directory should be deleted recursively.
        // it should be true if directory is not empty
        Directory.Delete(directoryPath, true);

        Console.WriteLine(
            "MyFolder deleted: " +
            !Directory.Exists(directoryPath)
        );


        // 13. Check remaining backup file

        Console.WriteLine("\n--- Remaining Files ---");

        Console.WriteLine(
            "backup2.txt exists: " +
            File.Exists(renamedPath)
        );


        // Clean up final backup file
        File.Delete(renamedPath);

        Console.WriteLine(
            "backup2.txt deleted: " +
            !File.Exists(renamedPath)
        );


        Console.WriteLine("\n========== DONE ==========");
    }
}