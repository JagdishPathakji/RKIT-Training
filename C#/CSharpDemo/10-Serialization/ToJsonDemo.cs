// JSON Serialization in C#
//

// 1. The problem serialization solves
//
// While an application is running, it represents data as .NET objects in memory. Example:
    // Article object
        // ArticleId: a Guid
        // Versions: a list
        // Tags: another list
// Other systems don't have direct access to that in-memory object. To store or transmit its data, the application needs a portable representation.
//
// Serialization converts an object into a format that can be stored or sent, such as JSON.
// Deserialization reads that format and reconstructs an object.
//
// .NET object --serialize--> JSON text --store or transmit-->
// JSON text --deserialize--> .NET object
//
// JSON is a human-readable text with objects, properties, arrays and value. For example:
    // {
        // "title": "Understanding SQL Joins",
        // "versionNumber": 1,
        // "tags": ["sql", "database"]
    // }
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CSharpDemo.Serialization;

/// <summary>Represents the ArticleExport type.</summary>
public class ArticleExport
{
    /// <summary>Gets or sets the article id value.</summary>
    public Guid ArticleId { get; set; }
    /// <summary>Gets or sets the author id value.</summary>
    public Guid AuthorId { get; set; }
    /// <summary>Gets or sets the published version id value.</summary>
    public Guid? PublishedVersionId { get; set; }
    /// <summary>Gets or sets the created at value.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Gets or sets the versions value.</summary>
    public List<ArticleVersionExport> Versions { get; set; } = new();
    /// <summary>Gets or sets the tags value.</summary>
    public List<string> Tags { get; set; } = new();
}

/// <summary>Represents the ArticleVersionExport type.</summary>
public class ArticleVersionExport
{
    /// <summary>Gets or sets the version id value.</summary>
    public Guid VersionId { get; set; }
    /// <summary>Gets or sets the version number value.</summary>
    public int VersionNumber { get; set; }
    /// <summary>Gets or sets the title value.</summary>
    public string Title { get; set; } = "";
    /// <summary>Gets or sets the status value.</summary>
    public ArticleStatus Status { get; set; }
    /// <summary>Gets or sets the created at value.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Gets or sets the content blocks value.</summary>
    public List<ContentBlockExport> ContentBlocks { get; set; } = new();
}

/// <summary>Represents the ContentBlockExport type.</summary>
public class ContentBlockExport
{
    /// <summary>Gets or sets the type value.</summary>
    public ContentBlockType Type { get; set; }
    /// <summary>Gets or sets the sequence order value.</summary>
    public int SequenceOrder { get; set; }
    /// <summary>Gets or sets the content value.</summary>
    public string Content { get; set; } = "";
}

/// <summary>Represents the ArticleStatus type.</summary>
public enum ArticleStatus
{
    /// <summary>The draft value.</summary>
    Draft,
    /// <summary>The pending review value.</summary>
    PendingReview,
    /// <summary>The published value.</summary>
    Published,
    /// <summary>The rejected value.</summary>
    Rejected
}

/// <summary>Represents the ContentBlockType type.</summary>
public enum ContentBlockType
{
    /// <summary>The heading value.</summary>
    Heading,
    /// <summary>The paragraph value.</summary>
    Paragraph,
    /// <summary>The code value.</summary>
    Code
}

/// <summary>Represents the ToJsonDemo type.</summary>
public static class ToJsonDemo
{
    /// <summary>Runs the demonstration.</summary>
    public static void Run()
    {
        ArticleExport article = CreateArticle();

        JsonSerializerOptions jsonOptions = new()
        {
            // WriteIndented = true — formats JSON with line breaks and indentation, making it easier to read. Setting it to false produces more compact JSON.
            WriteIndented = true,
            // PropertyNamingPolicy = JsonNamingPolicy.CamelCase — writes C# property names like ArticleId as JSON names like "articleId".
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            // DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull — leaves out properties whose value is null when serializing. Non-null values are still written.
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            // Converters = { new JsonStringEnumConverter() } — writes enum values such as ArticleStatus.Published as "Published" instead of a number.
            Converters = { new JsonStringEnumConverter() }
        };

        try
        {
            // Convert the .NET article object into JSON text.
            string json = JsonSerializer.Serialize(article, jsonOptions);

            Console.WriteLine("=== Serialized article JSON ===");
            Console.WriteLine(json);

            // Save the JSON file in the current working directory.
            string filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "knowledge-base-article.json");

            File.WriteAllText(filePath, json);

            Console.WriteLine($"\nJSON saved to: {filePath}");

            // Read the JSON text from the file and convert it back into an object.
            string savedJson = File.ReadAllText(filePath);

            ArticleExport restoredArticle =
                JsonSerializer.Deserialize<ArticleExport>(savedJson, jsonOptions)
                ?? throw new JsonException("The JSON did not contain an article.");

            Console.WriteLine("\n=== Deserialized article summary ===");
            Console.WriteLine($"Article ID: {restoredArticle.ArticleId}");
            Console.WriteLine($"Author ID: {restoredArticle.AuthorId}");
            Console.WriteLine($"Created: {restoredArticle.CreatedAt:yyyy-MM-dd}");
            Console.WriteLine($"Tags: {string.Join(", ", restoredArticle.Tags)}");

            foreach (ArticleVersionExport version in restoredArticle.Versions)
            {
                Console.WriteLine(
                    $"\nVersion {version.VersionNumber}: {version.Title}");
                Console.WriteLine($"Status: {version.Status}");

                foreach (ContentBlockExport block in version.ContentBlocks)
                {
                    Console.WriteLine(
                        $"  {block.SequenceOrder}. [{block.Type}] {block.Content}");
                }
            }
        }
        catch (JsonException exception)
        {
            Console.WriteLine($"JSON processing failed: {exception.Message}");
        }
        catch (IOException exception)
        {
            Console.WriteLine($"File operation failed: {exception.Message}");
        }
        catch (UnauthorizedAccessException exception)
        {
            Console.WriteLine($"Permission denied: {exception.Message}");
        }
    }

    private static ArticleExport CreateArticle()
    {
        Guid authorId = Guid.NewGuid();
        Guid articleId = Guid.NewGuid();
        Guid publishedVersionId = Guid.NewGuid();
        Guid draftVersionId = Guid.NewGuid();

        return new ArticleExport
        {
            ArticleId = articleId,
            AuthorId = authorId,
            PublishedVersionId = publishedVersionId,
            CreatedAt = new DateTime(
                2026, 10, 1, 8, 30, 0, DateTimeKind.Utc),

            Tags = new List<string>
            {
                "sql",
                "database",
                "tutorial"
            },

            Versions = new List<ArticleVersionExport>
            {
                new ArticleVersionExport
                {
                    VersionId = publishedVersionId,
                    VersionNumber = 1,
                    Title = "Understanding SQL Joins",
                    Status = ArticleStatus.Published,
                    CreatedAt = new DateTime(
                        2026, 10, 1, 8, 30, 0, DateTimeKind.Utc),

                    ContentBlocks = new List<ContentBlockExport>
                    {
                        new ContentBlockExport
                        {
                            Type = ContentBlockType.Heading,
                            SequenceOrder = 1,
                            Content = "What is a join?"
                        },
                        new ContentBlockExport
                        {
                            Type = ContentBlockType.Paragraph,
                            SequenceOrder = 2,
                            Content = "A join combines related rows from database tables."
                        },
                        new ContentBlockExport
                        {
                            Type = ContentBlockType.Code,
                            SequenceOrder = 3,
                            Content = "SELECT * FROM USER_ROLE;"
                        }
                    }
                },

                new ArticleVersionExport
                {
                    VersionId = draftVersionId,
                    VersionNumber = 2,
                    Title = "Understanding SQL Joins: Updated",
                    Status = ArticleStatus.PendingReview,
                    CreatedAt = new DateTime(
                        2026, 10, 5, 10, 0, 0, DateTimeKind.Utc),

                    ContentBlocks = new List<ContentBlockExport>
                    {
                        new ContentBlockExport
                        {
                            Type = ContentBlockType.Heading,
                            SequenceOrder = 1,
                            Content = "What is a join?"
                        },
                        new ContentBlockExport
                        {
                            Type = ContentBlockType.Paragraph,
                            SequenceOrder = 2,
                            Content = "This revised version includes more examples."
                        }
                    }
                }
            }
        };
    }
}
