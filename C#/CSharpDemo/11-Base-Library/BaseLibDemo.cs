using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace CSharpDemo.BaseLibrary;

// 1. What is the Base Class Library in C# ?
//
// The Base Class Library is the collection of standard types and functionality provided by .NET so you don't have to write common operations yourself.
//
// It is not just a collection of base classes. It includes classes, interfaces, structs, enums and other types.
//
//

// 2. Why do we need it ?
//
// C# Programs commonly use it for everyday tasks like printing output, working with lists, handling dates, searching data, reading files, and handling errors.
//
// We usually don't install it separately. A .NET project references the standard .NET libraries automatically. using directives let our source code refer to type in a namespace more conveniently.
//

// 3. Commonly used BCL
//
// 1. System namespace (Console, DateTime, Guid, Exception)
// 2. System.Collections.Generic (List, Dictionary, Queue)
// 3. System.Linq (Where, OrderBy, FirstOrDefault)
// 4. System.IO (file, directory operations)
// 5. System.Text (string builder)
//


/// <summary>Represents the BaseLibDemo type.</summary>
public class BaseLibDemo {

    /// <summary>Runs the demonstration.</summary>
    public static void Run() {

        Console.WriteLine("=== BASE LIBARIES  DEMO ===");

        // System: Console, Guid, DateTime
        Guid articleId = Guid.NewGuid();
        DateTime submittedAt = DateTime.Now;

        // Collections.Generic: List, Dictionary, Queue
        List<string> articleTitles = new()
        {
            "Getting Started with SQL",
            "Understanding SQL Joins",
            "Using Database Transactions"
        };

        Dictionary<Guid, string> articles = new()
        {
            { articleId, articleTitles[0] }
        };

        Queue<string> reviewQueue = new();
        reviewQueue.Enqueue("Understanding SQL Joins");
        reviewQueue.Enqueue("Using Database Transactions");

        // LINQ: filter, sort, and find the first matching article.
        string? firstSqlArticle = articleTitles
            .Where(title => title.Contains("SQL"))
            .OrderBy(title => title)
            .FirstOrDefault();

        // System.Text: build a small report.
        StringBuilder report = new();
        report.AppendLine("Knowledge Base Article Report");
        report.AppendLine($"Article ID: {articleId}");
        report.AppendLine($"Submitted: {submittedAt:g}");
        report.AppendLine($"First matching title: {firstSqlArticle}");

        // System.IO: save and read the report.
        // Path.GetTempPath() returns the path to the operating system’s temporary-files folder.
        string filePath = Path.Combine(Path.GetTempPath(), "article-report.txt");

        try {
            File.WriteAllText(filePath, report.ToString());
            Console.WriteLine(File.ReadAllText(filePath));
        }
        catch(IOException exception) {
            Console.WriteLine($"Could not access the report file: {exception.Message}");
        }
    }
}
