using System;
using System.IO;


// ReadOnly → "don't let anyone write to this"
// Hidden → "don't show this in normal folder views"
// System → "this belongs to the OS, be careful"
// Archive → "this has changed since the last backup"
// Directory → "this isn't a file at all, it's a folder"

class FileMetadataDemo
{
    static void Main()
    {
        string path = "demo.txt";
        File.WriteAllText(path, "Some content");

        FileInfo fi = new FileInfo(path);

        // ---- Timestamps ----
        Console.WriteLine($"Created:       {fi.CreationTime}");
        Console.WriteLine($"Last Modified: {fi.LastWriteTime}");
        Console.WriteLine($"Last Accessed: {fi.LastAccessTime}");

        // ---- Size ----
        Console.WriteLine($"Size: {fi.Length} bytes");

        // ---- Attributes ----
        Console.WriteLine($"Attributes: {fi.Attributes}");
        Console.WriteLine($"Is ReadOnly: {fi.IsReadOnly}");
        Console.WriteLine($"Is Hidden: {fi.Attributes.HasFlag(FileAttributes.Hidden)}");

        // Setting an attribute (ADD, keeping others)
        fi.Attributes |= FileAttributes.Hidden;
        Console.WriteLine($"\nAfter hiding: {fi.Attributes}");

        // Removing an attribute (keep others)
        fi.Attributes &= ~FileAttributes.Hidden;
        Console.WriteLine($"After unhiding: {fi.Attributes}");

        // ReadOnly toggle (built-in property, easier than bitwise)
        fi.IsReadOnly = true;
        Console.WriteLine($"Is ReadOnly now: {fi.IsReadOnly}");
        Console.WriteLine($"Attributes: {fi.Attributes}");
        fi.IsReadOnly = false;

        // Setting a timestamp manually
        fi.CreationTime = new DateTime(2024, 1, 1);
        Console.WriteLine($"\nNew CreationTime: {fi.CreationTime}");

        File.Delete(path);
    }
}