using System;

namespace KnowledgeBase {

    public class Article {

        public string Title {get; set;}
        public bool IsPublished {get; set;}
    }

    class Program {

        // DEFINE THE DELEGATE.
        // ANY METHOD THAT TAKES AN ARTICLE AND RETURNS A BOOL CAN BE STORED HERE.
        public delegate bool ArticleFilterDelegate(Article article);

        // CREATE A MATCHING METHOD
        static bool CheckIfPublished(Article article) {
            return article.IsPublished == true;
        } 

        static void Main() {

            Article article  = new Article {
                Title = "C# Basics",
                IsPublished = true
            };

            // ASSIGN METHOD TO DELEGATE VARIABLE
            ArticleFilterDelegate filter = CheckIfPublished;

            // EXECUTE THE VARIABLE
            bool result = filter(article);
            Console.WriteLine($"Is it published ? {result}");
        }

    }

}