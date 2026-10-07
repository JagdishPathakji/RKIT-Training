// XmlWriter writes XML and lets you customize formatting, such as indentation. XmlSerializer can write through an XmlWriter, but it can also write directly to a stream or text writer.
//
// StringBuilder collected the XML text in memory so the program could print it and save it afterward.
//
// StringReader was used to give that XML string to the deserializer. It wasn’t a builder; it just lets an API read from a string.
//

using System;
using System.Collections.Generic;
using System.IO;
// Provides StringBuilder
using System.Text;
// Provides XML-writing types such as XmlWriter and XmlWriterSettings.
using System.Xml;
// Provides XmlSerializer and XML mapping attributes such as [XmlRoot] and [XmlElement].
using System.Xml.Serialization;

namespace CSharpDemo.Serialization;

/// <summary>Represents the XmlArticleExport type.</summary>
[XmlRoot("article")]
public class XmlArticleExport
{
    /// <summary>Gets or sets the article id value.</summary>
    [XmlElement("articleId")]
    public Guid ArticleId { get; set; }

    /// <summary>Gets or sets the author id value.</summary>
    [XmlElement("authorId")]
    public Guid AuthorId { get; set; }

    /// <summary>Gets or sets the published version id value.</summary>
    [XmlElement("publishedVersionId", IsNullable = true)]
    public Guid? PublishedVersionId { get; set; }

    /// <summary>Gets or sets the created at value.</summary>
    [XmlElement("createdAt")]
    public DateTime CreatedAt { get; set; }

    /// <summary>Gets or sets the versions value.</summary>
    [XmlArray("versions")]
    [XmlArrayItem("version")]
    public List<XmlArticleVersionExport> Versions { get; set; } = new();

    /// <summary>Gets or sets the tags value.</summary>
    [XmlArray("tags")]
    [XmlArrayItem("tag")]
    public List<string> Tags { get; set; } = new();
}

/// <summary>Represents the XmlArticleVersionExport type.</summary>
public class XmlArticleVersionExport
{
    /// <summary>Gets or sets the version id value.</summary>
    [XmlElement("versionId")]
    public Guid VersionId { get; set; }

    /// <summary>Gets or sets the version number value.</summary>
    [XmlElement("versionNumber")]
    public int VersionNumber { get; set; }

    /// <summary>Gets or sets the title value.</summary>
    [XmlElement("title")]
    public string Title { get; set; } = "";

    /// <summary>Gets or sets the status value.</summary>
    [XmlElement("status")]
    public XmlArticleStatus Status { get; set; }

    /// <summary>Gets or sets the created at value.</summary>
    [XmlElement("createdAt")]
    public DateTime CreatedAt { get; set; }

    /// <summary>Gets or sets the content blocks value.</summary>
    [XmlArray("contentBlocks")]
    [XmlArrayItem("contentBlock")]
    public List<XmlContentBlockExport> ContentBlocks { get; set; } = new();
}

/// <summary>Represents the XmlContentBlockExport type.</summary>
public class XmlContentBlockExport
{
    /// <summary>Gets or sets the type value.</summary>
    [XmlElement("type")]
    public XmlContentBlockType Type { get; set; }

    /// <summary>Gets or sets the sequence order value.</summary>
    [XmlElement("sequenceOrder")]
    public int SequenceOrder { get; set; }

    /// <summary>Gets or sets the content value.</summary>
    [XmlElement("content")]
    public string Content { get; set; } = "";
}

/// <summary>Represents the XmlArticleStatus type.</summary>
public enum XmlArticleStatus
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

/// <summary>Represents the XmlContentBlockType type.</summary>
public enum XmlContentBlockType
{
    /// <summary>The heading value.</summary>
    Heading,
    /// <summary>The paragraph value.</summary>
    Paragraph,
    /// <summary>The code value.</summary>
    Code
}

/// <summary>Represents the ToXmlDemo type.</summary>
public static class ToXmlDemo
{
    /// <summary>Runs the demonstration.</summary>
    public static void Run()
    {
        XmlArticleExport article = CreateArticle();

        // Creates an XML serializer configured to serialize and deserialize XmlArticleExport objects.
        // new(...) uses target-typed object creation; C# knows this must construct an XmlSerializer.
        XmlSerializer serializer = new(typeof(XmlArticleExport));

        XmlWriterSettings writerSettings = new()
        {
            Indent = true,
            IndentChars = "  ",
            // OmitXmlDeclaration = true omits the optional first line such as <?xml version="1.0"?>.
            OmitXmlDeclaration = true
        };

        string filePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "knowledge-base-article.xml");

        try
        {
            // Serialize the object into an XML string.
            StringBuilder xmlBuilder = new();

            using (XmlWriter writer = XmlWriter.Create(xmlBuilder, writerSettings))
            {
                serializer.Serialize(writer, article);
            }

            string xml = xmlBuilder.ToString();

            Console.WriteLine("=== Serialized article XML ===");
            Console.WriteLine(xml);

            // Save the XML in the current working directory.
            File.WriteAllText(filePath, xml, Encoding.UTF8);
            Console.WriteLine($"\nXML saved to: {filePath}");

            // Read the file and deserialize its XML into an object.
            string savedXml = File.ReadAllText(filePath);

            using StringReader reader = new(savedXml);

            if (serializer.Deserialize(reader) is not XmlArticleExport restoredArticle)
            {
                throw new InvalidOperationException(
                    "The XML did not contain a knowledge-base article.");
            }

            Console.WriteLine("\n=== Deserialized article summary ===");
            Console.WriteLine($"Article ID: {restoredArticle.ArticleId}");
            Console.WriteLine($"Author ID: {restoredArticle.AuthorId}");
            Console.WriteLine($"Created: {restoredArticle.CreatedAt:yyyy-MM-dd}");
            Console.WriteLine($"Tags: {string.Join(", ", restoredArticle.Tags)}");

            foreach (XmlArticleVersionExport version in restoredArticle.Versions)
            {
                Console.WriteLine(
                    $"\nVersion {version.VersionNumber}: {version.Title}");
                Console.WriteLine($"Status: {version.Status}");

                foreach (XmlContentBlockExport block in version.ContentBlocks)
                {
                    Console.WriteLine(
                        $"  {block.SequenceOrder}. [{block.Type}] {block.Content}");
                }
            }
        }
        catch (InvalidOperationException exception)
        {
            Console.WriteLine($"XML processing failed: {exception.Message}");
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

    private static XmlArticleExport CreateArticle()
    {
        Guid authorId = Guid.NewGuid();
        Guid articleId = Guid.NewGuid();
        Guid publishedVersionId = Guid.NewGuid();

        return new XmlArticleExport
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

            Versions = new List<XmlArticleVersionExport>
            {
                new XmlArticleVersionExport
                {
                    VersionId = publishedVersionId,
                    VersionNumber = 1,
                    Title = "Understanding SQL Joins",
                    Status = XmlArticleStatus.Published,
                    CreatedAt = new DateTime(
                        2026, 10, 1, 8, 30, 0, DateTimeKind.Utc),

                    ContentBlocks = new List<XmlContentBlockExport>
                    {
                        new XmlContentBlockExport
                        {
                            Type = XmlContentBlockType.Heading,
                            SequenceOrder = 1,
                            Content = "What is a join?"
                        },
                        new XmlContentBlockExport
                        {
                            Type = XmlContentBlockType.Paragraph,
                            SequenceOrder = 2,
                            Content = "A join combines related rows from database tables."
                        },
                        new XmlContentBlockExport
                        {
                            Type = XmlContentBlockType.Code,
                            SequenceOrder = 3,
                            Content = "SELECT * FROM USER_ROLE;"
                        }
                    }
                }
            }
        };
    }
}
