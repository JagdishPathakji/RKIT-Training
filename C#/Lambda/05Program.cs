using System;
using System.Collections.Generic;

namespace KnowledgeBase 
{
    public class Article { public string Title { get; set; } public bool IsDraft { get; set; } }

    class Program 
    {
        static void Main() 
        {
            List<Article> articles = new List<Article> {
                new Article { Title = "C# 101", IsDraft = true },
                new Article { Title = "SQL basics", IsDraft = false },
                new Article { Title = "LINQ Guide", IsDraft = true }
            };

            // 1. FindAll (Requires a Predicate. Returns a NEW LIST of all matches)
            List<Article> allDrafts = articles.FindAll(article => article.IsDraft == true);

            // 2. Find (Requires a Predicate. Returns the FIRST matching item, or null)
            Article firstDraft = articles.Find(article => article.IsDraft == true);

            // 3. Exists (Requires a Predicate. Returns true if AT LEAST ONE item matches)
            bool hasDrafts = articles.Exists(article => article.IsDraft == true);

            // 4. RemoveAll (Requires a Predicate. Deletes all matching items, returns count removed)
            int removedCount = articles.RemoveAll(article => article.Title.Contains("SQL"));

            // 5. ForEach (Requires an Action. Executes it on every item)
            articles.ForEach(article => Console.WriteLine(article.Title));
        }
    }
}