using System;
using System.Collections.Generic;

namespace CSharpDemo.LambdaExpression;

public enum EnmArticleStatus {

    Draft,
    PendingReview,
    NeedsImprovement,
    Rejected,
    Published
}

public enum EnmRole {

    Author,
    Reviewer,
    Editor
}

public class User {

    public int Id { get; set; }
    public string Name { get; set; }
    public EnmRole Role { get; set; }
}

public class Article {

    public int Id { get; set; }
    public string Title { get; set; }
    public int AuthorId { get; set; }
    public EnmArticleStatus Status { get; set; }
    public int ViewCount { get; set; }
}

public class LambdaExpressionDemo {

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