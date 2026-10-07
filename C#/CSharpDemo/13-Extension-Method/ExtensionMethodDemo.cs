using System;
using System.Collections.Generic;

namespace CSharpDemo.ExtensionMethod;


// 1. Domain Classes
/// <summary>Represents the EnmArticleStatus type.</summary>
public enum EnmArticleStatus { Draft, Published }

/// <summary>Represents the Article type.</summary>
public class Article {
    /// <summary>Gets or sets the title value.</summary>
    public string Title { get; set; }
    /// <summary>Gets or sets the status value.</summary>
    public EnmArticleStatus Status { get; set; }
}

// 2. Extension Class (MUST BE STATIC)
/// <summary>Represents the ArticleExtensions type.</summary>
public static class ArticleExtensions  {
        
    // 3. Extension Method Structure (this Article article)
    /// <summary>Checks whether an article has been published.</summary>
    /// <param name="article">The article to process.</param>
    /// <returns><see langword="true"/> if the article is published; otherwise, <see langword="false"/>.</returns>
    public static bool IsPublished(this Article article) 
    {
        return article.Status == EnmArticleStatus.Published;
    }

    // 4. Passing Additional Parameters
    /// <summary>Returns the article title shortened to the requested maximum length.</summary>
    /// <param name="article">The article to process.</param>
    /// <param name="maxLength">The maximum allowed title length.</param>
    /// <returns>The original title or a shortened title ending in an ellipsis.</returns>
    public static string GetShortTitle(this Article article, int maxLength) {
        if (article.Title.Length <= maxLength) return article.Title;
        return article.Title.Substring(0, maxLength) + "...";
    }

    // 5. Extending Collections
    /// <summary>Counts the published articles in a sequence.</summary>
    /// <param name="articles">The articles to process.</param>
    /// <returns>The number of published articles.</returns>
    public static int CountPublished(this IEnumerable<Article> articles) {
        int count = 0;
    
        foreach (var article in articles)  {
            if (article.IsPublished()) count++;
        }
    
        return count;
    }

    // 6. Passing a Lambda to an Extension Method
    /// <summary>Returns articles that satisfy the supplied condition.</summary>
    /// <param name="articles">The articles to process.</param>
    /// <param name="condition">The condition used to select articles.</param>
    /// <returns>A list containing only matching articles.</returns>
    public static List<Article> Filter(this List<Article> articles, Predicate<Article> condition) {
        return articles.FindAll(condition);
    }
}


/// <summary>Represents the ExtensionMethodDemo type.</summary>
public class ExtensionMethodDemo {
    
    /// <summary>Runs the demonstration.</summary>
    public static void Run() {

        Console.WriteLine("=== EXTENSION METHODS DEMO ===");
        
        List<Article> articles = new List<Article> {
            new Article { Title = "C# Extension Methods Guide", Status = EnmArticleStatus.Published },
            new Article { Title = "LINQ Basics", Status = EnmArticleStatus.Draft },
            new Article { Title = "Advanced C# Architecture", Status = EnmArticleStatus.Published }
        };

        // 7. Calling an Extension Method
        Article firstArticle = articles[0];
        Console.WriteLine($"Is First Published? {firstArticle.IsPublished()}");
            
        // 8. Calling with additional parameter
        Console.WriteLine($"Short Title: {firstArticle.GetShortTitle(10)}");

        // 9. Calling on a collection
        Console.WriteLine($"Total Published: {articles.CountPublished()}");

        // 10. Passing lambda to extension method
        List<Article> drafts = articles.Filter(article => article.Status == EnmArticleStatus.Draft);
        Console.WriteLine($"Total Drafts Found: {drafts.Count}");

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey(true);

    }
}
