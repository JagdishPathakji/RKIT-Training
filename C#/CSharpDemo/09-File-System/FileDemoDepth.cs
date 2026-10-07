using System;
using System.IO;
using System.Text;

namespace CSharpDemo.FileSystem;

/// <summary>Represents the FileDemoDepth type.</summary>
public static class FileDemoDepth
{
    /// <summary>Runs the demonstration.</summary>
    public static void Run()
    {
        string workingDirectory = Directory.GetCurrentDirectory();

        Console.WriteLine("=== FILE HANDLING DEMO ===");
        Console.WriteLine($"Working directory: {workingDirectory}");

        try
        {
            WholeFileTextMethods(workingDirectory);
            LineBasedMethods(workingDirectory);
            FileInfo(workingDirectory);
            StreamReaderAndWriter(workingDirectory);
            FileReadWriteMethods(workingDirectory);
            FileStream(workingDirectory);
            ExpectedFileError(workingDirectory);

            Console.WriteLine("\nDemo complete. The example files are in the working directory.");
        }
        catch (UnauthorizedAccessException exception)
        {
            Console.WriteLine($"Permission denied: {exception.Message}");
        }
        catch (IOException exception)
        {
            Console.WriteLine($"File operation failed: {exception.Message}");
        }
        catch (ArgumentException exception)
        {
            Console.WriteLine($"Invalid file path or argument: {exception.Message}");
        }
        catch (NotSupportedException exception)
        {
            Console.WriteLine($"File operation is not supported: {exception.Message}");
        }
    }

    private static void WholeFileTextMethods(string workingDirectory)
    {
        Console.WriteLine("\n--- File: whole-file text methods ---");

        string filePath = GetFilePath(workingDirectory, "article-text.txt");

        File.WriteAllText(
            filePath,
            "Understanding SQL Joins",
            Encoding.UTF8);

        File.AppendAllText(
            filePath,
            $"{Environment.NewLine}Status: Published",
            Encoding.UTF8);

        string contents = File.ReadAllText(filePath, Encoding.UTF8);

        Console.WriteLine("WriteAllText + AppendAllText + ReadAllText:");
        Console.WriteLine(contents);
    }

    private static void LineBasedMethods(string workingDirectory)
    {
        Console.WriteLine("\n--- File: line-based text methods ---");

        string filePath = GetFilePath(workingDirectory, "article-lines.txt");

        string[] initialLines =
        {
            "Understanding SQL Joins",
            "Author: Asha",
            "Status: Published"
        };

        File.WriteAllLines(filePath, initialLines, Encoding.UTF8);
        File.AppendAllLines(
            filePath,
            new[] { "Tag: database" },
            Encoding.UTF8);

        Console.WriteLine("ReadAllLines reads all lines into an array:");
        string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);

        foreach (string line in lines)
        {
            Console.WriteLine(line);
        }

        Console.WriteLine("ReadLines reads lines as they are enumerated:");
        foreach (string line in File.ReadLines(filePath, Encoding.UTF8))
        {
            Console.WriteLine($"  {line}");
        }
    }

    private static void FileInfo(string workingDirectory)
    {
        Console.WriteLine("\n--- FileInfo: one file and its metadata ---");

        string filePath = GetFilePath(workingDirectory, "article-file-info.txt");
        FileInfo fileInfo = new(filePath);

        // CreateText():
            // creates the file if it doesn't exist
            // replaces existing contents if it does exist
            // returns a StreamWriter
//
        using (StreamWriter writer = fileInfo.CreateText())
        {
            writer.WriteLine("Article: Database Indexes");
            writer.WriteLine("Status: Draft");
        }

        // FileInfo caches some file-system information.
        // Refresh() tells the FileInfo object:
            // "Go back to the file system and update your metadata."
//
        fileInfo.Refresh();

        Console.WriteLine($"Exists: {fileInfo.Exists}");
        Console.WriteLine($"Name: {fileInfo.Name}");
        Console.WriteLine($"Size: {fileInfo.Length} bytes");
        Console.WriteLine($"Last written: {fileInfo.LastWriteTime}");

        Console.WriteLine("Read the file through FileInfo.OpenText():");
        // OpenText() opens the file for text reading and returns a StreamReader.
//
//
        using (StreamReader reader = fileInfo.OpenText())
        {
            Console.WriteLine(reader.ReadToEnd());
        }

        // AppendText opens a writer that adds text at the end of the file.
        using (StreamWriter writer = fileInfo.AppendText())
        {
            writer.WriteLine("Tag: performance");
        }

        fileInfo.Refresh();
        Console.WriteLine($"Size after appending: {fileInfo.Length} bytes");
    }

    private static void StreamReaderAndWriter(string workingDirectory)
    {
        Console.WriteLine("\n--- StreamWriter and StreamReader: text streams ---");

        string filePath = GetFilePath(workingDirectory, "article-stream.txt");
        Encoding encoding = new UTF8Encoding();

        // append: false means create or overwrite the file.
        using (StreamWriter writer = new(filePath, append: false, encoding))
        {
            writer.WriteLine("Article: Transactions");
            writer.WriteLine("Version: 1");
            writer.WriteLine("Status: Pending Review");
        }

        // append: true means add these lines at the end.
        using (StreamWriter writer = new(filePath, append: true, encoding))
        {
            writer.WriteLine("Tag: database");
        }

        using (StreamReader reader = new(filePath, encoding))
        {
            Console.WriteLine($"First line, using ReadLine(): {reader.ReadLine()}");
            Console.WriteLine("Remaining content, using ReadToEnd():");
            Console.WriteLine(reader.ReadToEnd());
        }

        Console.WriteLine("Read the whole file one line at a time:");
        using (StreamReader reader = new(filePath, encoding))
        {
            string? line;

            while ((line = reader.ReadLine()) != null)
            {
                Console.WriteLine($"  {line}");
            }
        }
    }

    private static void FileReadWriteMethods(string workingDirectory)
    {
        Console.WriteLine("\n--- File: whole-file byte methods ---");

        string filePath = GetFilePath(workingDirectory, "article-bytes.bin");
        byte[] bytesToWrite = Encoding.UTF8.GetBytes(
            "Article export: Understanding SQL Joins");

        File.WriteAllBytes(filePath, bytesToWrite);

        byte[] bytesRead = File.ReadAllBytes(filePath);
        string textRead = Encoding.UTF8.GetString(bytesRead);

        Console.WriteLine($"Bytes written: {bytesToWrite.Length}");
        Console.WriteLine($"Bytes read: {bytesRead.Length}");
        Console.WriteLine($"Decoded text: {textRead}");
    }

    private static void FileStream(string workingDirectory)
    {
        Console.WriteLine("\n--- FileStream: direct byte access ---");

        string filePath = GetFilePath(workingDirectory, "article-filestream.bin");
        byte[] bytesToWrite = Encoding.UTF8.GetBytes(
            "Version 2: Article update pending review");
        
        // A FileStream provides a stream of bytes between your program and a file.
//
        using (FileStream stream = new(
                   filePath,
                   FileMode.Create, // Create a new file. If the file already exists, overwrite it.
                   FileAccess.ReadWrite, // Read and Write access are needed
                   FileShare.None // Don't allow other file handles/processes to share the file while this stream has it open.
                ))
        {
            // Write sends bytes to the file at the current stream position.
            // (byte array, starting index in array, number of bytes to write)
            stream.Write(bytesToWrite, 0, bytesToWrite.Length);

            // Flush asks the stream to send buffered data to the underlying file.
            stream.Flush();

            // Seek moves the current position back to the start before reading.
            stream.Seek(0, SeekOrigin.Begin);
            // checked: converts the long value returned by Length to int while checking for integer overflow.
            byte[] buffer = new byte[checked((int)stream.Length)];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string textRead = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            Console.WriteLine($"Bytes written: {bytesToWrite.Length}");
            Console.WriteLine($"Bytes read: {bytesRead}");
            Console.WriteLine($"Decoded text: {textRead}");
        }
    }

    private static void ExpectedFileError(string workingDirectory)
    {
        Console.WriteLine("\n--- Handling a missing file ---");

        string missingFilePath = GetFilePath(
            workingDirectory,
            "article-that-does-not-exist.txt");

        try
        {
            File.ReadAllText(missingFilePath, Encoding.UTF8);
        }
        catch (FileNotFoundException exception)
        {
            Console.WriteLine(
                $"Expected error handled: file was not found ({exception.FileName}).");
        }
    }

    private static string GetFilePath(string workingDirectory, string fileName)
    {
        return Path.Combine(workingDirectory, fileName);
    }
}
