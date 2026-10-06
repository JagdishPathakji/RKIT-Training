using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

public class Article {

    public int Id {get; set;}
    public string Title {get; set;} = string.Empty;
    public string Status {get; set;} = string.Empty;
    public decimal Rating {get; set;}
};

public class Program {

    public static void Main() {

        List<Article> articles = new List<Article>
        {
            new Article
            {
                Id = 1,
                Title = "Introduction to C#",
                Status = "Published",
                Rating = 4.5m
            },

            new Article
            {
                Id = 2,
                Title = "Learning Serialization",
                Status = "Draft",
                Rating = 4.0m
            }
        };


        // JSON Serialization
        string json = SerializeToJson(articles);
        Console.WriteLine("JSON:");
        Console.WriteLine(json);

        // JSON Deserialization
        List<Article>? articlesFromJson = DeserializeFromJson(json);
        Console.WriteLine();
        Console.WriteLine("Articles restored from JSON:");

        if (articlesFromJson != null) {
            PrintArticles(articlesFromJson);
        }


        // XML Serialization
        string xml = SerializeToXml(articles);

        Console.WriteLine();
        Console.WriteLine("XML:");
        Console.WriteLine(xml);

        // XML Deserialization
        List<Article>? articlesFromXml = DeserializeFromXml(xml);

        Console.WriteLine();
        Console.WriteLine("Articles restored from XML:");

        if (articlesFromXml != null)
        {
            PrintArticles(articlesFromXml);
        }
    }

    public static string SerializeToJson(
    List<Article> articles) {

        JsonSerializerOptions options =
            new JsonSerializerOptions();

        options.PropertyNamingPolicy =
            JsonNamingPolicy.CamelCase;

        options.WriteIndented = true;
   
        string json =
            JsonSerializer.Serialize(
                articles,
                options);

        return json;
    }

    public static List<Article>? DeserializeFromJson(
    string json) {

        JsonSerializerOptions options =
            new JsonSerializerOptions();

        options.PropertyNamingPolicy =
            JsonNamingPolicy.CamelCase;

        List<Article>? articles =
            JsonSerializer.Deserialize<List<Article>>(
                json,
                options);

        return articles;
    }

    public static string SerializeToXml(
        List<Article> articles) {
        XmlSerializer serializer =
            new XmlSerializer(
                typeof(List<Article>));

        StringWriter writer =
            new StringWriter();

        serializer.Serialize(
            writer,
            articles);

        string xml =
            writer.ToString();

        writer.Close();

        return xml;
    }

    public static List<Article>? DeserializeFromXml(
        string xml) {
        XmlSerializer serializer =
            new XmlSerializer(
                typeof(List<Article>));

        StringReader reader =
            new StringReader(xml);

        List<Article>? articles =
            (List<Article>?)serializer.Deserialize(
                reader);

        reader.Close();

        return articles;
    }

    public static void PrintArticles(
        List<Article> articles) {
        foreach (Article article in articles)
        {
            Console.WriteLine(
                $"Id: {article.Id}");

            Console.WriteLine(
                $"Title: {article.Title}");

            Console.WriteLine(
                $"Status: {article.Status}");

            Console.WriteLine(
                $"Rating: {article.Rating}");

            Console.WriteLine();
        }
    }

}

/*
Why use StringWriter?
XmlSerializer.Serialize() needs a writable destination.

StringWriter writer = new StringWriter();
serializer.Serialize(writer, articles);
string xml = writer.ToString();

StringWriter creates an empty in-memory writing area. The serializer writes XML into it.
*/

/*
XmlSerializer.Deserialize() needs a readable source.

StringReader reader =
    new StringReader(xml);

List<Article>? articles =
    (List<Article>?)serializer.Deserialize(reader);

Here, xml already contains the XML text. We pass it to StringReader so the serializer can read from it.

StringReader reads XML from a string.
StreamReader reads text from a file or stream.
Both are readers, but their sources are different.
*/