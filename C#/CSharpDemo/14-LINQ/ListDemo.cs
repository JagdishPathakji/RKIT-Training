using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpDemo.LINQList;

public class Article
{
    public int ArticleId { get; set; }
    public string Title { get; set; }
    public int StatusId { get; set; }      // 5 = Published, 1 = Draft
    public int AuthorId { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Summary { get; set; }
    public List<string> Tags { get; set; } = new List<string>();
}

public class Author
{
    public int AuthorId { get; set; }
    public string Name { get; set; }
}

public class ArticleDto
{
    public string Title { get; set; }
    public string AuthorName { get; set; }
}

public static class ListDemoLinq
{
    static List<Article> articles = new List<Article>
    {
        new Article { ArticleId = 1, Title = "Intro to SQL",    StatusId = 5, AuthorId = 101, CreatedDate = new DateTime(2024,1,10), Summary = "Learn SQL basics",     Tags = new List<string>{ "sql", "database" } },
        new Article { ArticleId = 2, Title = "Advanced LINQ",   StatusId = 5, AuthorId = 102, CreatedDate = new DateTime(2024,3,15), Summary = null,                   Tags = new List<string>{ "linq", "csharp" } },
        new Article { ArticleId = 3, Title = "Draft Notes",     StatusId = 1, AuthorId = 101, CreatedDate = new DateTime(2024,2,20), Summary = "Not ready yet",        Tags = new List<string>() },
        new Article { ArticleId = 4, Title = "SQL Performance", StatusId = 5, AuthorId = 101, CreatedDate = new DateTime(2024,4,5),  Summary = "Tuning SQL queries",   Tags = new List<string>{ "sql", "performance" } },
        new Article { ArticleId = 5, Title = "C# Basics",       StatusId = 5, AuthorId = 103, CreatedDate = new DateTime(2024,5,1),  Summary = null,                   Tags = new List<string>{ "csharp" } },
        new Article { ArticleId = 6, Title = "SQL Joins",       StatusId = 1, AuthorId = 102, CreatedDate = new DateTime(2024,6,12), Summary = "Inner vs outer joins", Tags = new List<string>{ "sql" } },
    };

    static List<Author> authors = new List<Author>
    {
        new Author { AuthorId = 101, Name = "Riya" },
        new Author { AuthorId = 102, Name = "Karan" },
        new Author { AuthorId = 103, Name = "Meera" },
        // Note: AuthorId 104 exists here on purpose but has NO articles -> used in GroupJoin/left-join examples
        new Author { AuthorId = 104, Name = "Zoya" },
    };

    public static void Run()
    {
        Console.WriteLine("=== LINQ WITH LIST DEMO ===");

        Lesson3_Where();
        Lesson4_SelectAndSelectMany();
        Lesson5_OrderBy();
        Lesson6_SkipTake();
        Lesson7_FirstSingleLast();
        Lesson8_AnyAllContains();
        Lesson9_Aggregates();
        Lesson10_AggregateFunc();
        Lesson11_SetOperations();
        Lesson12_Join();

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey(true);
    }

    // Small helper just to keep printing consistent everywhere below
    static void Print(string label, System.Collections.IEnumerable items)
    {
        Console.WriteLine(label);
        foreach (var item in items)
            Console.WriteLine("  " + item);
        Console.WriteLine();
    }


    // ================================================================
    // LESSON 3 -- Where() variations
    // ================================================================
    static void Lesson3_Where()
    {
        Console.WriteLine("========== LESSON 3 -- Where() ==========\n");

        // 1) Basic Where() -- simple condition
        var published = articles.Where(a => a.StatusId == 5).Select(a => a.Title).ToList();
        Print("1) Published only:", published);

        // 2) Indexed Where() -- uses position in the sequence
        var everyOther = articles.Where((a, index) => index % 2 == 0).Select(a => a.Title).ToList();
        Print("2) Every other article (by position):", everyOther);

        // 3) Multiple conditions in ONE Where() using &&
        var publishedByAuthor101 = articles
            .Where(a => a.StatusId == 5 && a.AuthorId == 101)
            .Select(a => a.Title)
            .ToList();
        Print("3) Published AND AuthorId 101:", publishedByAuthor101);

        // 4) Multiple conditions using ||
        var author102Or103 = articles
            .Where(a => a.AuthorId == 102 || a.AuthorId == 103)
            .Select(a => a.Title)
            .ToList();
        Print("4) AuthorId 102 OR 103:", author102Or103);

        // 5) Chaining multiple Where() calls -- same result as combining with &&
        var chained = articles
            .Where(a => a.StatusId == 5)
            .Where(a => a.AuthorId == 101)
            .Select(a => a.Title)
            .ToList();
        Print("5) Chained Where() (Published, then AuthorId 101):", chained);

        // 6) Null-safe filtering -- guard before calling a method on a possibly-null property
        var summariesWithSql = articles
            .Where(a => a.Summary != null && a.Summary.Contains("SQL"))
            .Select(a => a.Title)
            .ToList();
        Print("6) Summary contains 'SQL' (null-safe):", summariesWithSql);

        // 7) Optional / conditional filters -- typical search-screen pattern
        int? statusId = 5;
        int? authorId = 101;
        string titleContains = null;

        IEnumerable<Article> query = articles;

        if (statusId.HasValue)
            query = query.Where(a => a.StatusId == statusId.Value);

        if (authorId.HasValue)
            query = query.Where(a => a.AuthorId == authorId.Value);

        if (!string.IsNullOrWhiteSpace(titleContains))
            query = query.Where(a => a.Title.Contains(titleContains, StringComparison.OrdinalIgnoreCase));

        var searchResult = query.Select(a => a.Title).ToList();
        Print("7) Optional filters applied (statusId=5, authorId=101):", searchResult);
    }


    // ================================================================
    // LESSON 4 -- Select() and SelectMany() variations
    // ================================================================
    static void Lesson4_SelectAndSelectMany()
    {
        Console.WriteLine("========== LESSON 4 -- Select() / SelectMany() ==========\n");

        // 1) Select() into a single property
        var titlesOnly = articles.Select(a => a.Title).ToList();
        Print("1) Just the titles:", titlesOnly);

        // 2) Select() into an anonymous type -- quick, local-only shape
        var titleAndAuthor = articles.Select(a => new { a.Title, a.AuthorId }).ToList();
        Print("2) Title + AuthorId (anonymous type):",
            titleAndAuthor.Select(x => $"{x.Title} - Author {x.AuthorId}"));

        // 3) Select() into a DTO class -- reusable, nameable shape
        var dtos = articles.Select(a => new ArticleDto
        {
            Title = a.Title,
            AuthorName = "Author #" + a.AuthorId
        }).ToList();
        Print("3) Title + AuthorName (DTO):", dtos.Select(d => $"{d.Title} - {d.AuthorName}"));

        // 4) Indexed Select() -- uses position in the sequence
        var numbered = articles.Select((a, index) => $"{index + 1}. {a.Title}").ToList();
        Print("4) Numbered titles:", numbered);

        // 5) Where() + Select() combined -- filter first, then shape
        var publishedTitles = articles
            .Where(a => a.StatusId == 5)
            .Select(a => a.Title)
            .ToList();
        Print("5) Titles of published articles only:", publishedTitles);

        // 6) SelectMany() -- flatten a nested collection (Tags per article) into one flat list
        var allTags = articles.SelectMany(a => a.Tags).ToList();
        Print("6) All tags, flattened:", allTags);
    }


    // ================================================================
    // LESSON 5 -- OrderBy() / OrderByDescending() / ThenBy() variations
    // ================================================================
    static void Lesson5_OrderBy()
    {
        Console.WriteLine("========== LESSON 5 -- OrderBy() ==========\n");

        // 1) OrderBy() -- ascending sort by one key
        var byTitleAsc = articles.OrderBy(a => a.Title).Select(a => a.Title).ToList();
        Print("1) Sorted by Title (ascending):", byTitleAsc);

        // 2) OrderByDescending() -- descending sort by one key
        var byDateDesc = articles.OrderByDescending(a => a.CreatedDate)
            .Select(a => $"{a.Title} ({a.CreatedDate:yyyy-MM-dd})").ToList();
        Print("2) Sorted by CreatedDate (newest first):", byDateDesc);

        // 3) ThenBy() -- secondary sort key, used AFTER OrderBy()
        // Sort by AuthorId first, then by Title within each author
        var byAuthorThenTitle = articles
            .OrderBy(a => a.AuthorId)
            .ThenBy(a => a.Title)
            .Select(a => $"Author {a.AuthorId}: {a.Title}")
            .ToList();
        Print("3) Sorted by AuthorId, then Title:", byAuthorThenTitle);

        // 4) ThenByDescending() -- secondary key, but descending
        var byStatusThenNewest = articles
            .OrderBy(a => a.StatusId)
            .ThenByDescending(a => a.CreatedDate)
            .Select(a => $"Status {a.StatusId}: {a.Title} ({a.CreatedDate:yyyy-MM-dd})")
            .ToList();
        Print("4) Sorted by StatusId, then CreatedDate descending:", byStatusThenNewest);

        // IMPORTANT: don't chain OrderBy().OrderBy() to add a second key 
        // the second OrderBy() call REPLACES the first sort instead of adding to it.
        // Always use ThenBy()/ThenByDescending() after the first OrderBy()/OrderByDescending().
    }


    // ================================================================
    // LESSON 6 -- Skip() / Take() / paging variations
    // ================================================================
    static void Lesson6_SkipTake()
    {
        Console.WriteLine("========== LESSON 6 -- Skip() / Take() ==========\n");

        // 1) Take() -- first N elements
        var firstThree = articles.Take(3).Select(a => a.Title).ToList();
        Print("1) First 3 articles:", firstThree);

        // 2) Skip() -- skip the first N elements, keep the rest
        var skipFirstTwo = articles.Skip(2).Select(a => a.Title).ToList();
        Print("2) Skip first 2 articles:", skipFirstTwo);

        // 3) Skip() + Take() together -- classic PAGINATION pattern
        int pageNumber = 2;   // 1-based page number
        int pageSize = 2;
        var page2 = articles
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(a => a.Title)
            .ToList();
        Print("3) Page 2 (pageSize=2):", page2);

        // 4) TakeWhile() -- take elements only WHILE a condition is true, stop at first failure
        // (order matters here -- based on original list order, not sorted)
        var takeWhilePublished = articles
            .TakeWhile(a => a.StatusId == 5)
            .Select(a => a.Title)
            .ToList();
        Print("4) TakeWhile StatusId==5 (stops at first non-match):", takeWhilePublished);

        // 5) SkipWhile() -- skip elements WHILE a condition is true, then take everything after
        var skipWhilePublished = articles
            .SkipWhile(a => a.StatusId == 5)
            .Select(a => a.Title)
            .ToList();
        Print("5) SkipWhile StatusId==5 (skips until first non-match, keeps rest):", skipWhilePublished);
    }


    // ================================================================
    // LESSON 7 -- First / FirstOrDefault / Single / SingleOrDefault / Last variations
    // ================================================================
    static void Lesson7_FirstSingleLast()
    {
        Console.WriteLine("========== LESSON 7 -- First / Single / Last ==========\n");

        // 1) First() -- returns the first match, THROWS if none found
        var first = articles.First(a => a.StatusId == 5);
        Console.WriteLine($"1) First published: {first.Title}");

        // 2) FirstOrDefault() -- returns the first match, or null (default) if none found -- SAFE
        var firstDraftOrNull = articles.FirstOrDefault(a => a.StatusId == 99); // no article has StatusId 99
        Console.WriteLine($"2) FirstOrDefault StatusId==99: {(firstDraftOrNull == null ? "null (not found)" : firstDraftOrNull.Title)}");

        // 3) Single() -- expects EXACTLY ONE match; throws if zero OR more than one match
        var singleAuthor103 = articles.Single(a => a.AuthorId == 103);
        Console.WriteLine($"3) Single article by AuthorId 103: {singleAuthor103.Title}");

        // 4) SingleOrDefault() -- same as Single(), but returns null instead of throwing when there are ZERO matches
        //    (still throws if there is MORE THAN ONE match)
        var singleOrDefaultNoMatch = articles.SingleOrDefault(a => a.AuthorId == 999);
        Console.WriteLine($"4) SingleOrDefault AuthorId==999: {(singleOrDefaultNoMatch == null ? "null (not found)" : singleOrDefaultNoMatch.Title)}");

        // 5) Last() -- returns the last match, THROWS if none found
        var last = articles.Last(a => a.StatusId == 5);
        Console.WriteLine($"5) Last published: {last.Title}");

        // 6) LastOrDefault() -- returns the last match, or null if none found -- SAFE
        var lastOrDefault = articles.LastOrDefault(a => a.StatusId == 99);
        Console.WriteLine($"6) LastOrDefault StatusId==99: {(lastOrDefault == null ? "null (not found)" : lastOrDefault.Title)}");
        Console.WriteLine();

        // RULE OF THUMB:
        //  - Use First/FirstOrDefault when you expect possibly-many matches and just want ANY one (usually the first).
        //  - Use Single/SingleOrDefault when your logic GUARANTEES at most one match (e.g. lookup by unique ID) --
        //    it acts as a safety check: if there's accidentally more than one, it throws instead of silently picking one.
        //  - Always prefer the "OrDefault" version unless you are certain a match must exist.
    }


    // ================================================================
    // LESSON 8 -- Any() / All() / Contains() variations
    // ================================================================
    static void Lesson8_AnyAllContains()
    {
        Console.WriteLine("========== LESSON 8 -- Any() / All() / Contains() ==========\n");

        // 1) Any() with no argument -- "does the collection have ANY elements at all?"
        bool hasAnyArticles = articles.Any();
        Console.WriteLine($"1) Has any articles at all: {hasAnyArticles}");

        // 2) Any() with a predicate -- "does AT LEAST ONE element match this condition?"
        bool hasAnyDrafts = articles.Any(a => a.StatusId == 1);
        Console.WriteLine($"2) Has any drafts: {hasAnyDrafts}");

        // 3) All() -- "do ALL elements match this condition?"
        bool allPublished = articles.All(a => a.StatusId == 5);
        Console.WriteLine($"3) Are all articles published: {allPublished}");

        // 4) All() on an empty filtered set -- returns TRUE (this surprises beginners!)
        var noResults = articles.Where(a => a.StatusId == 999);
        bool allTrueOnEmpty = noResults.All(a => a.StatusId == 5);
        Console.WriteLine($"4) All() on an empty sequence: {allTrueOnEmpty}");

        // 5) Contains() -- checks if a specific VALUE exists in a simple collection (e.g. list of ints/strings)
        var allTags = articles.SelectMany(a => a.Tags).ToList();
        bool hasSqlTag = allTags.Contains("sql");
        Console.WriteLine($"5) Tag list contains 'sql': {hasSqlTag}");
        Console.WriteLine();

        // NOTE: Contains() on a list of OBJECTS (like List<Article>) checks reference/equality,
        // not a specific property 
        
    }


    // ================================================================
    // LESSON 9 -- Count / Sum / Average / Min / Max variations
    // ================================================================
    static void Lesson9_Aggregates()
    {
        Console.WriteLine("========== LESSON 9 -- Count / Sum / Average / Min / Max ==========\n");

        // 1) Count() with no argument -- total number of elements
        int totalArticles = articles.Count();
        Console.WriteLine($"1) Total articles: {totalArticles}");

        // 2) Count() with a predicate -- count only matching elements
        int publishedCount = articles.Count(a => a.StatusId == 5);
        Console.WriteLine($"2) Published article count: {publishedCount}");

        // 3) Sum() -- add up a numeric property across all elements
        int totalTagCount = articles.Sum(a => a.Tags.Count);
        Console.WriteLine($"3) Total tags across all articles: {totalTagCount}");

        // 4) Average() -- average of a numeric property
        double averageTagsPerArticle = articles.Average(a => a.Tags.Count);
        Console.WriteLine($"4) Average tags per article: {averageTagsPerArticle:F2}");

        // 5) Min() / Max() on a simple numeric projection
        DateTime earliest = articles.Min(a => a.CreatedDate);
        DateTime latest = articles.Max(a => a.CreatedDate);
        Console.WriteLine($"5) Earliest: {earliest:yyyy-MM-dd}, Latest: {latest:yyyy-MM-dd}");

        // 6) MinBy() / MaxBy() -- get the WHOLE ELEMENT with the min/max key (not just the key value)
        //    (Available in .NET 6+)
        var oldestArticle = articles.MinBy(a => a.CreatedDate);
        var newestArticle = articles.MaxBy(a => a.CreatedDate);
        Console.WriteLine($"6) Oldest article: {oldestArticle.Title}, Newest article: {newestArticle.Title}");
        Console.WriteLine();

        // Sum()/Count() are safe on empty collections and give you 0.
        // Average()/Min()/Max() are NOT safe — they throw an exception on empty collections.
    }


    // ================================================================
    // LESSON 10 -- Aggregate() variations (custom accumulator / reduce-style)
    // ================================================================
    static void Lesson10_AggregateFunc()
    {
        Console.WriteLine("========== LESSON 10 -- Aggregate() ==========\n");

        // 1) Aggregate() WITHOUT a seed -- starts with the first element as the accumulator
        // Example: concatenate all titles into one string, comma-separated

        // suppose titles:- ["C#", "LINQ", "ASP", "MYSQL"]
        // for first:- accumulator: "C#", current: "LINQ"
        // ans:- "C#, LINQ, ASP, MYSQL"
        string allTitlesConcatenated = articles
            .Select(a => a.Title)
            .Aggregate((accumulator, current) => accumulator + ", " + current);
        Console.WriteLine($"1) All titles concatenated: {allTitlesConcatenated}");

        // 2) Aggregate() WITH a seed -- starts the accumulator at a known initial value
        // Example: sum of tag counts, starting from 0 (equivalent to Sum(), shown manually here)
        int totalTags = articles.Aggregate(0, (accumulator, article) => accumulator + article.Tags.Count);
        Console.WriteLine($"2) Total tags (via Aggregate with seed 0): {totalTags}");

        // 3) Aggregate() WITH a seed AND a result selector -- transform the final accumulated value
        // Example: build up total tag count, then format it into a sentence at the end
        string summarySentence = articles.Aggregate(
            0,                                                   // seed
            (accumulator, article) => accumulator + article.Tags.Count,  // accumulation step
            finalCount => $"There are {finalCount} tags in total."       // result selector (runs once, at the end)
        );
        Console.WriteLine($"3) {summarySentence}");
        Console.WriteLine();

        // WHEN TO USE Aggregate() vs Sum()/Count()/etc:
        // Use the built-in Sum/Count/Min/Max/Average when they already do what you need --
        // they are clearer and less error-prone. Reach for Aggregate() only when your
        // accumulation logic is CUSTOM and doesn't match any built-in method
        // (e.g. building a concatenated string, running a custom running total with rules, etc).
    }


    // ================================================================
    // LESSON 11 -- Distinct / Union / Intersect / Except variations
    // ================================================================
    static void Lesson11_SetOperations()
    {
        Console.WriteLine("========== LESSON 11 -- Distinct / Union / Intersect / Except ==========\n");

        // 1) Distinct() -- removes duplicate VALUES from a simple sequence
        var allTagsWithDuplicates = articles.SelectMany(a => a.Tags).ToList();
        var uniqueTags = allTagsWithDuplicates.Distinct().ToList();
        Print("1) All tags (with duplicates):", allTagsWithDuplicates);
        Print("   Unique tags (Distinct):", uniqueTags);

        // Sets used for the next three examples
        List<int> teamAAuthorIds = new List<int> { 101, 102, 103 };
        List<int> teamBAuthorIds = new List<int> { 102, 103, 104 };

        // 2) Union() -- combines two sequences, removing duplicates (like a mathematical "OR")
        var allAuthorIdsUnion = teamAAuthorIds.Union(teamBAuthorIds).ToList();
        Print("2) Union of Team A and Team B author IDs:", allAuthorIdsUnion);

        // 3) Intersect() -- only elements present in BOTH sequences (like a mathematical "AND")
        var commonAuthorIds = teamAAuthorIds.Intersect(teamBAuthorIds).ToList();
        Print("3) Intersect (authors in BOTH teams):", commonAuthorIds);

        // 4) Except() -- elements in the first sequence that are NOT in the second (set subtraction)
        var onlyInTeamA = teamAAuthorIds.Except(teamBAuthorIds).ToList();
        Print("4) Except (in Team A but NOT in Team B):", onlyInTeamA);

        // 5) Distinct() with objects -- needs a custom equality approach, since Article doesn't
        // define value-equality by default. Common workaround: project down to a comparable key first.
        var distinctStatusIds = articles.Select(a => a.StatusId).Distinct().ToList();
        Print("5) Distinct StatusId values used across articles:", distinctStatusIds);
        Console.WriteLine();
    }


    // ================================================================
    // LESSON 12 -- Join() / GroupJoin() variations
    // ================================================================
    static void Lesson12_Join()
    {
        Console.WriteLine("========== LESSON 12 -- Join() / GroupJoin() ==========\n");

        // 1) Join() -- INNER JOIN style: only matches where the key exists on BOTH sides
        var innerJoin = articles.Join(
            authors,                          // the collection to join with
            article => article.AuthorId,      // key selector from the LEFT side (articles)
            author => author.AuthorId,        // key selector from the RIGHT side (authors)
            (article, author) => new { article.Title, author.Name }   // how to combine a matched pair
        ).ToList();
        Console.WriteLine("1) Inner Join (article + matching author name):");
        foreach (var item in innerJoin)
            Console.WriteLine($"   {item.Title} -> {item.Name}");
        Console.WriteLine();
        // Note: Author "Zoya" (104) has no articles, so she never appears here --
        // that's expected inner-join behavior: unmatched rows on either side are dropped.

        // 2) GroupJoin() -- LEFT-JOIN style: keeps ALL authors, even ones with zero articles,
        // grouping their matching articles into a list (empty list if none match)
        var groupJoin = authors.GroupJoin(
            articles,
            author => author.AuthorId,
            article => article.AuthorId,
            (author, matchedArticles) => new { author.Name, Articles = matchedArticles.ToList() }
        ).ToList();
        Console.WriteLine("2) GroupJoin (every author, with their articles or an empty list):");
        foreach (var item in groupJoin)
        {
            string titles = item.Articles.Any()
                ? string.Join(", ", item.Articles.Select(a => a.Title))
                : "(no articles)";
            Console.WriteLine($"   {item.Name}: {titles}");
        }
        Console.WriteLine();
    }
}