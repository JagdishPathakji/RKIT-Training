using System;
using System.Collections.Generic;
using CSharpDemo.NamespaceAndLibraries.Helpers;
namespace CSharpDemo.NamespaceAndLibraries;

/// <summary>Represents the NamespaceDemo type.</summary>
public class NamespaceDemo
{
    /// <summary>Runs the demonstration.</summary>
    public void Run()
    {
        ConsoleHelper.Clear();

        Console.WriteLine("=== NAMESPACES AND LIBRARIES DEMO ===");

        // 1. Class from our own namespace
        ArticleHelper helper = new ArticleHelper();
        string title = helper.GetTitle();
        Console.WriteLine($"\nArticle: {title}");

        // 3. .NET library - List<T>
        List<string> tags = new List<string>
        {
            "C#",
            ".NET",
            "MySQL"
        };

        Console.WriteLine("\nTags:");
        foreach (string tag in tags)
        {
            Console.WriteLine(tag);
        }

        // 4. Fully qualified class name
        System.DateTime publishedAt = System.DateTime.Now;
        Console.WriteLine(
            $"\nPublished: {publishedAt}"
        );

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey(true);
    }
}
