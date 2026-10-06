using System;
using System.IO;

namespace BaseLib;

public static class FileIODemo
{
    public static void Run()
    {
        // Use case 1: create a text file
        string folder = Path.Combine(Environment.CurrentDirectory, "DemoFiles");
        Directory.CreateDirectory(folder);

        string filePath = Path.Combine(folder, "notes.txt");
        File.WriteAllText(filePath, "C# learning\nBase Class Libraries\nGood practice");

        Console.WriteLine("File created: " + filePath);

        // Use case 2: read file content
        string content = File.ReadAllText(filePath);
        Console.WriteLine("File content:\n" + content);

        // Use case 3: work with path + directory info
        string nestedPath = Path.Combine(folder, "subfolder", "sample.txt");
        string directory = Path.GetDirectoryName(nestedPath)!;
        string fileName = Path.GetFileName(nestedPath);

        Console.WriteLine("Directory: " + directory);
        Console.WriteLine("File name: " + fileName);
    }
}