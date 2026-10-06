using System;
namespace CSharpDemo.DateMathString;

public class StringDemo
{
    public void Run()
    {
        ConsoleHelper.Clear();

        Console.WriteLine("=== STRINGS DEMO ===");

        string title = "  Getting Started with SQL JOINs  ";
        string cleanTitle = title.Trim();
        // title[2] = 'a';
        // title = "aaa";
        
        // string builder class

        Console.WriteLine("\nOriginal and basic information:");
        Console.WriteLine($"Original title: '{title}'");
        Console.WriteLine($"Length: {title.Length} characters");
        Console.WriteLine($"Trimmed title: '{cleanTitle}'");
        Console.WriteLine($"Trim start: '{title.TrimStart()}'");
        Console.WriteLine($"Trim end: '{title.TrimEnd()}'");

        Console.WriteLine("\nChanging case:");
        Console.WriteLine($"Uppercase: {cleanTitle.ToUpper()}");
        Console.WriteLine($"Lowercase: {cleanTitle.ToLower()}");
        Console.WriteLine($"Original is unchanged: '{title}'");

        Console.WriteLine("\nSearching and comparing:");
        Console.WriteLine(
            $"Contains 'sql' (ignore case): " +
            $"{cleanTitle.Contains("sql", StringComparison.OrdinalIgnoreCase)}"
        );
        Console.WriteLine(
            $"Starts with 'Getting': " +
            $"{cleanTitle.StartsWith("Getting", StringComparison.Ordinal)}"
        );
        Console.WriteLine(
            $"Ends with 'joins' (ignore case): " +
            $"{cleanTitle.EndsWith("joins", StringComparison.OrdinalIgnoreCase)}"
        );
        Console.WriteLine(
            $"Position of 'SQL': " +
            $"{cleanTitle.IndexOf("SQL", StringComparison.OrdinalIgnoreCase)}"
        );
        Console.WriteLine(
            $"Position of last 's' (ignore case): " +
            $"{cleanTitle.LastIndexOf('s')}"
        );
        Console.WriteLine(
            $"Usernames match (ignore case): " +
            $"{string.Equals("jagdish", "JAGDISH", StringComparison.OrdinalIgnoreCase)}"
        );

        Console.WriteLine("\nExtracting and changing text:");
        Console.WriteLine($"First word: {cleanTitle.Substring(0, 7)}");
        Console.WriteLine(
            $"Replace 'JOINs' with 'joins': " +
            $"{cleanTitle.Replace("JOINs", "joins", StringComparison.OrdinalIgnoreCase)}"
        );

        string tags = "SQL, Indexing, JOIN";
        string[] tagList = tags.Split(", ", StringSplitOptions.RemoveEmptyEntries);

        Console.WriteLine("\nSplit tags into separate values:");
        foreach (string tag in tagList)
        {
            Console.WriteLine(tag);
        }

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey(true);
    }
}