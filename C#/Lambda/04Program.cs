using System;

namespace KnowledgeBase {

    public class Article {
        public string Title {get; set;}
        public int ViewCount {get; set;}
    }

    class Program {

        static void Main() {

            Article myArticle = new Article {
                Title = "Understanding Delegates",
                ViewCount = 50
            };

        
            Predicate<Article> isPopular = article => article.ViewCount > 100;
            Console.WriteLine($"Is Popular? {isPopular(myArticle)}");

            Func<Article, string> formatTitle = article => $"[{article.Title.ToUpper()}]";
            Console.WriteLine($"Formatted: {formatTitle(myArticle)}");

            Action<Article> printDetails = article => Console.WriteLine($"Article {article.Title} has ViewCount {article.ViewCount}");        
            printDetails(myArticle);
        }
    }
}