using System;
using System.Collections.Generic;

namespace CSharpDemo.LambdaExpression;

/// <summary>Represents the EnmArticleStatus type.</summary>
public enum EnmArticleStatus {

    /// <summary>The draft value.</summary>
    Draft,
    /// <summary>The pending review value.</summary>
    PendingReview,
    /// <summary>The needs improvement value.</summary>
    NeedsImprovement,
    /// <summary>The rejected value.</summary>
    Rejected,
    /// <summary>The published value.</summary>
    Published
}

/// <summary>Represents the EnmRole type.</summary>
public enum EnmRole {

    /// <summary>The author value.</summary>
    Author,
    /// <summary>The reviewer value.</summary>
    Reviewer,
    /// <summary>The editor value.</summary>
    Editor
}

/// <summary>Represents the User type.</summary>
public class User {

    /// <summary>Gets or sets the id value.</summary>
    public int Id { get; set; }
    /// <summary>Gets or sets the name value.</summary>
    public string Name { get; set; }
    /// <summary>Gets or sets the role value.</summary>
    public EnmRole Role { get; set; }
}

/// <summary>Represents the Article type.</summary>
public class Article {

    /// <summary>Gets or sets the id value.</summary>
    public int Id { get; set; }
    /// <summary>Gets or sets the title value.</summary>
    public string Title { get; set; }
    /// <summary>Gets or sets the author id value.</summary>
    public int AuthorId { get; set; }
    /// <summary>Gets or sets the status value.</summary>
    public EnmArticleStatus Status { get; set; }
    /// <summary>Gets or sets the view count value.</summary>
    public int ViewCount { get; set; }
}

/// <summary>Represents the LambdaExpressionDemo type.</summary>
public class LambdaExpressionDemo {

    /// <summary>Runs the demonstration.</summary>
    public static void Run() {

        Console.WriteLine("=== LAMBDA EXPRESSIONS DEMO ===");

        List<Article> articles = new List<Article> {

            new Article { 
                Id = 1,
                Title = "C# Basics",
                AuthorId = 101,
                Status = EnmArticleStatus.Published,
                ViewCount = 1500
            },
            new Article { 
                Id = 2,
                Title = "OOP Guide",
                AuthorId = 101,
                Status = EnmArticleStatus.Draft,
                ViewCount = 0
            },
            new Article { 
                Id = 3,
                Title = "Advanced SQL",
                AuthorId = 102,
                Status = EnmArticleStatus.PendingReview,
                ViewCount = 0
            },
            new Article { 
                Id = 4,
                Title = "APIs in C#",
                AuthorId = 103,
                Status = EnmArticleStatus.Published,
                ViewCount = 500
            }
        };

        User currentEditor = new User {
            Id = 999,
            Name = "Alice",
            Role = EnmRole.Editor
        };

        Console.WriteLine("=== 1. FILTERING (Predicate) ===");
        List<Article> publishedArticles = articles.FindAll(article => article.Status == EnmArticleStatus.Published);
        Console.WriteLine($"Found {publishedArticles.Count} published articles.");
        
        Console.WriteLine("\n=== 2. FINDING SINGLE ITEM (Predicate) ===");
        // Find returns the first match or null
        Article sqlArticle = articles.Find(article => article.Title.Contains("SQL"));
        Console.WriteLine($"Found SQL Article ID: {sqlArticle?.Id}");

        Console.WriteLine("\n=== 3. CHECKING EXISTENCE (Predicate) ===");
        bool hasDrafts = articles.Exists(article => article.Status == EnmArticleStatus.Draft);
        Console.WriteLine($"System has drafts: {hasDrafts}");

        Console.WriteLine("\n=== 4. DISPLAYING (Action) ===");
        // ForEach requires an Action<Article> (returns void)
        publishedArticles.ForEach(article => Console.WriteLine($"- {article.Title} ({article.ViewCount} views)"));

        Console.WriteLine("\n=== 5. VARIABLE CAPTURE (Closure) ===");
        int searchAuthorId = 101; 
        // The lambda captures searchAuthorId
        List<Article> author101Articles = articles.FindAll(article => article.AuthorId == searchAuthorId);
        Console.WriteLine($"Author 101 has {author101Articles.Count} articles.");

        Console.WriteLine("\n=== 6. CALCULATION (Func) ===");
        // Func<Article, double> takes an Article and returns a double
        Func<Article, double> calculateAdRevenue = article => article.ViewCount * 0.05;
        Console.WriteLine($"Revenue for '{articles[0].Title}': ${calculateAdRevenue(articles[0])}");

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey(true);
    }
}
