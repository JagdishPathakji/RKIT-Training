using System;

namespace KnowledgeBase {
    public class Article {
        public int AuthorId { get; set; }
    }

    class Program {
        static void Main() {
            Article article = new Article {
                AuthorId = 102
            };

            int targetAuthorId = 101;

            // lambda uses surrounding variable (closure)
            // takes capture instead of copy
            Predicate<Article> filter =
                a => a.AuthorId == targetAuthorId;

            Console.WriteLine(filter(article));

            targetAuthorId = 102;

            // lambda expression sees the latest value (capture)
            Console.WriteLine(filter(article));
        }
    }
}