using System;
using System.Collections.Generic;

namespace KnowledgeBase 
{
    // 1. Domain Classes
    public enum EnmArticleStatus { Draft, Published }

    public class Article 
    {
        public string Title { get; set; }
        public EnmArticleStatus Status { get; set; }
    }

    // 2. Extension Class (MUST BE STATIC)
    public static class ArticleExtensions 
    {
        // 3. Extension Method Structure (this Article article)
        public static bool IsPublished(this Article article) 
        {
            return article.Status == EnmArticleStatus.Published;
        }

        // 6. Passing Additional Parameters
        public static string GetShortTitle(this Article article, int maxLength) 
        {
            if (article.Title.Length <= maxLength) return article.Title;
            return article.Title.Substring(0, maxLength) + "...";
        }

        // 8. Extending Collections
        public static int CountPublished(this IEnumerable<Article> articles) 
        {
            int count = 0;
            foreach (var article in articles) 
            {
                if (article.IsPublished()) count++;
            }
            return count;
        }

        // 9. Passing a Lambda to an Extension Method
        public static List<Article> Filter(this List<Article> articles, Predicate<Article> condition) 
        {
            return articles.FindAll(condition);
        }
    }

    // Main Program
    class Program 
    {
        static void Main(string[] args) 
        {
            List<Article> articles = new List<Article> {
                new Article { Title = "C# Extension Methods Guide", Status = EnmArticleStatus.Published },
                new Article { Title = "LINQ Basics", Status = EnmArticleStatus.Draft },
                new Article { Title = "Advanced C# Architecture", Status = EnmArticleStatus.Published }
            };

            // 5. Calling an Extension Method
            Article firstArticle = articles[0];
            Console.WriteLine($"Is First Published? {firstArticle.IsPublished()}");
            
            // 6. Calling with additional parameter
            Console.WriteLine($"Short Title: {firstArticle.GetShortTitle(10)}");

            // 8. Calling on a collection
            Console.WriteLine($"Total Published: {articles.CountPublished()}");

            // 7. Passing lambda to extension method
            List<Article> drafts = articles.Filter(article => article.Status == EnmArticleStatus.Draft);
            Console.WriteLine($"Total Drafts Found: {drafts.Count}");

            Console.ReadLine();
        }
    }
}