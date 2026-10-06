using System;
using System.Collections.Generic;
using System.Linq;

public class Article
{
    public int ArticleId { get; set; }
    public string Title { get; set; }
    public int StatusId { get; set; }
}

public class Program
{
    public static void Main()
    {
        Pitfall1_SourceChangesBeforeEnumeration();
        Pitfall2_MultipleEnumeration();
        Pitfall4_ClosureCapturesVariableNotValue();
        GoodUse_ShortCircuiting();
    }
!
    // ================================================================
    // PITFALL 1 -- source changes AFTER building the query, BEFORE running it
    // ================================================================
    static void Pitfall1_SourceChangesBeforeEnumeration()
    {
        Console.WriteLine("========== PITFALL 1 -- source changes before enumeration ==========");

        List<Article> articles = new List<Article>
        {
            new Article { ArticleId = 1, Title = "Intro to SQL", StatusId = 5 },
            new Article { ArticleId = 2, Title = "Draft Notes",  StatusId = 1 },
        };

        var query = articles.Where(a => a.StatusId == 5);   // nothing runs yet

        articles.Add(new Article { ArticleId = 3, Title = "New Published Article", StatusId = 5 });

        var result = query.ToList();   // runs NOW -- sees the newly added article too

        Console.WriteLine("Result includes the article added AFTER building the query:");
        foreach (var a in result)
            Console.WriteLine($"  {a.Title}");
        Console.WriteLine();
    }


    // ================================================================
    // PITFALL 2 -- enumerating the same lazy query more than once
    // ================================================================
    static void Pitfall2_MultipleEnumeration()
    {
        Console.WriteLine("========== PITFALL 2 -- multiple enumeration ==========");

        List<Article> articles = new List<Article>
        {
            new Article { ArticleId = 1, Title = "Intro to SQL", StatusId = 5 },
            new Article { ArticleId = 2, Title = "Draft Notes",  StatusId = 1 },
            new Article { ArticleId = 3, Title = "SQL Perf",     StatusId = 5 },
        };

        int callCount = 0;

        // We attach a side effect (callCount++) inside the predicate just so we can PROVE
        // how many times the filter logic actually runs -- this is only for demonstration.
        IEnumerable<Article> query = articles.Where(a =>
        {
            callCount++;
            return a.StatusId == 5;
        });

        int count = query.Count();          // enumeration #1 -- loops through all 3 articles
        List<Article> list = query.ToList(); // enumeration #2 -- loops through all 3 articles AGAIN

        Console.WriteLine($"Predicate was evaluated {callCount} times (3 articles x 2 enumerations = 6)");
        Console.WriteLine($"Count() result: {count}, ToList() result count: {list.Count}");
        Console.WriteLine();

        // ---- THE FIX ----
        callCount = 0;
        List<Article> materialized = articles.Where(a =>
        {
            callCount++;
            return a.StatusId == 5;
        }).ToList();   // runs ONCE here

        int countFromList = materialized.Count;       // just reads a property, no re-looping
        List<Article> sameList = materialized;          // just reusing the same list, no re-looping

        Console.WriteLine("After fix (materialize once with ToList(), then reuse):");
        Console.WriteLine($"Predicate was evaluated {callCount} times (only 3, one pass)");
        Console.WriteLine();
    }


    // ================================================================
    // PITFALL 4 -- lambda captures the VARIABLE, not the value at write-time
    // ================================================================
    static void Pitfall4_ClosureCapturesVariableNotValue()
    {
        Console.WriteLine("========== PITFALL 4 -- closure captures variable, not value ==========");

        List<Article> articles = new List<Article>
        {
            new Article { ArticleId = 1, Title = "Status 1", StatusId = 1 },
            new Article { ArticleId = 2, Title = "Status 5", StatusId = 5 },
            new Article { ArticleId = 3, Title = "Status 10", StatusId = 10 },
        };

        int minStatus = 5;
        var query = articles.Where(a => a.StatusId >= minStatus);   // built when minStatus == 5

        minStatus = 10;   // changed AFTER building the query, BEFORE enumerating it

        var result = query.ToList();   // uses minStatus == 10 now, NOT 5!

        Console.WriteLine("Query built when minStatus was 5, but ran after minStatus became 10:");
        foreach (var a in result)
            Console.WriteLine($"  {a.Title}");   // only "Status 10" shows up, not "Status 5" too

        // ---- THE FIX -- freeze the value into its own local copy ----
        int minStatusFixed = 5;
        int frozenValue = minStatusFixed;   // snapshot taken right here
        var fixedQuery = articles.Where(a => a.StatusId >= frozenValue);

        minStatusFixed = 10;   // no longer affects fixedQuery, because frozenValue already locked in 5

        var fixedResult = fixedQuery.ToList();

        Console.WriteLine("\nAfter fix (value frozen into its own variable before building the query):");
        foreach (var a in fixedResult)
            Console.WriteLine($"  {a.Title}");   // now correctly shows both "Status 5" and "Status 10"
        Console.WriteLine();
    }


    // ================================================================
    // GOOD USE OF LAZINESS -- short-circuiting with First()
    // ================================================================
    static void GoodUse_ShortCircuiting()
    {
        Console.WriteLine("========== GOOD USE -- short-circuiting ==========");

        List<Article> articles = new List<Article>
        {
            new Article { ArticleId = 1, Title = "First Published", StatusId = 5 },
            new Article { ArticleId = 2, Title = "Draft",            StatusId = 1 },
            new Article { ArticleId = 3, Title = "Second Published", StatusId = 5 },
        };

        int itemsChecked = 0;

        var firstMatch = articles.Where(a =>
        {
            itemsChecked++;
            return a.StatusId == 5;
        }).First();   // stops as soon as the FIRST match is found

        Console.WriteLine($"First match: {firstMatch.Title}");
        Console.WriteLine($"Items actually checked before stopping: {itemsChecked}");
        // itemsChecked will be 1, NOT 3 -- proving Where()+First() didn't process
        // the whole list, it stopped the moment article #1 matched.
        Console.WriteLine();
    }
}