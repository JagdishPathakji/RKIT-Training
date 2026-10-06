using System;
namespace BaseLib;

public class Program
{
    public static async Task Main()
    {
        Console.WriteLine("=== 1. Collections ===");
        CollectionsDemo.Run();

        Console.WriteLine("\n=== 2. File I/O ===");
        FileIODemo.Run();

        Console.WriteLine("\n=== 3. JSON ===");
        JsonDemo.Run();
    }
}