using System;

namespace KnowledgeBase {

    public class Article {
        public bool IsPublished {get; set;}
    }

    class Program {

        public delegate bool ArticleFilter(Article article);

        static void Main() {

            Article article = new Article {
                IsPublished = true
            };

            ArticleFilter filter = delegate(Article article) {
                return article.IsPublished == true;
            };

            Console.WriteLine($"Is published ? {filter(article)}");
        }
    }
}