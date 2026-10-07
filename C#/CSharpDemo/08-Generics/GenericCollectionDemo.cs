using System;
using System.Linq;
using System.Collections.Generic;
namespace CSharpDemo.Generics;

// IEnumerable<T> provides an enumerator for moving through a sequence one item at a time.
//
// If a class implements IEnumerable<T>, it makes a simple promise: "You can loop through my items one by one".
//
// IEnumerable<T> is the base interface in this collection hierarchy. It provides iteration, but does not expose methods to add, remove, or access items by index, or a Count property.
//



// ICollection<T> inherits from IEnumerable<T>. That means ICollection<T> can do everything an IEnumerable<T> can do but it adds a whole new set of rules.
//
// ICollection<T> provides methods for adding and removing items, plus a Count property. Read-only implementations may reject changes.
//
// By using ICollection<T>, we gain access to essential methods and properties:
// -- .Count: tells how many items are in the collection
// -- .Add(item): puts a new item into the collection
// -- Remove(item): takes a specific item out
// -- Clear(): wipes the entire collection clean
// -- .Contains(item): checks if a specific item exists inside the collection and returns true or false
//
// The Limitation: No Index Based Access
// ICollection<T> still does not understand the concept of position. It treats data like a bag of marbles. You know how many marbles are in the bag, and you can take a red marble out, but you cannot ask for "the 3rd marble".
//



// IList<T> inherits from ICollection<T>. This means an IList<T> is an IEnumerable<T> and an ICollection<T>. It does everything what parent classes do.
//
// If a class implements IList<T>, it promises: "I keep items in a specific order, and you can access or change an item by its index."
//
// By using IList<T>, we have following additional methods:
// -- [index]: can access items via index
// -- .Insert(index,item): inserts a new item into a specific position and pushing
// everything else down
// -- .RemoveAt(index): deletes whatever is at that index
// -- .IndexOf(item): searches for an item and tells you its exact index number or return -1 if it is not present in the IList<T>.
//



// IDictionary<TKey,TValue> defines key-value collection behavior. It inherits ICollection<KeyValuePair<TKey,TValue>> and indirectly IEnumerable<KeyValuePair<TKey,TValue>>.
//
// If a class signs the IDictionary<TKey,TValue> contract, it promises: "I map unique keys to values, and you can look up a value using its key."
//
// It forces the class to implement:
// -- [key]: the key-based access like index
// -- .Add(key,value): adds a new pair
// -- .Contains(key): check if key is present
// -- .Keys and .Values: provide collections for iterating over the keys or values.
// -- .Remove(key): removes key and value
//
// If you want to add or remove items, you must do it through the IDictionary itself (using dict.Add() or dict.Remove()). The collections returned by .Keys and .Values are strictly for looking at the data or looping through it.
//



// ISet<T> does not define ordering or index access. It rejects duplicate items and provides set operations.
//
// Properties of ISet<T>:
// -- Uniqueness: Adding a duplicate does not add another item; Add returns false for the duplicate.
//
// -- No Ordering Contract or Index: ISet<T> does not promise an item order or provide access by numeric index.
//
// -- Contains(): List<T> checks items one by one (O(N)); HashSet<T> lookup is typically O(1) on average, but can be slower in the worst case.
//



// Stack<T>: Last In First Out. It provides Push() and Pop() operations at the top of the stack.
//
// Queue<T>: First In First Out. Enqueue() adds at the back and Dequeue() removes from the front of the queue.
//



// IEnumerable<T>  (The base: "I can loop through items")
// │
// ├── ICollection<T>  (The Modifier: "I have a .Count, and can Add/Remove")
// │    │
// │    ├── IList<T>  (The Indexer: "I maintain order, access by [index]")
// │    │    └── List<T>
// │    │
// │    ├── IDictionary<TKey, TValue>  (The Lookup: "I map Keys to Values")
// │    │    └── Dictionary<TKey, TValue>
// │    │
// │    ├── ISet<T>  (The Unique Collection: "I allow no duplicate items")
// │    │    └── HashSet<T>
// │    ├── Stack<T>  (LIFO: Last-In, First-Out)
// │    └── Queue<T>  (FIFO: First-In, First-Out)
//

/// <summary>Represents the KnowledgeBaseArticle type.</summary>
public class KnowledgeBaseArticle
{
    // ARTICLE table
    /// <summary>Gets or sets the article id value.</summary>
    public Guid ArticleId { get; set; }
    /// <summary>Gets or sets the author id value.</summary>
    public Guid AuthorId { get; set; }
    /// <summary>Gets or sets the current published version id value.</summary>
    public Guid? CurrentPublishedVersionId { get; set; }
    /// <summary>Gets or sets the created at value.</summary>
    public DateTime CreatedAt { get; set; }

    // ARTICLE_VERSION rows belonging to this article.
    // Concrete class used: List<ArticleVersion>
    /// <summary>Gets or sets the versions value.</summary>
    public IList<ArticleVersion> Versions { get; set; }
        = new List<ArticleVersion>();

    // TAG IDs linked through ARTICLE_TAG.
    // Concrete class used: HashSet<int>
    /// <summary>Gets or sets the tag ids value.</summary>
    public ISet<int> TagIds { get; set; }
        = new HashSet<int>();
}

/// <summary>Represents the ArticleVersion type.</summary>
public class ArticleVersion
{
    // ARTICLE_VERSION table
    /// <summary>Gets or sets the version id value.</summary>
    public Guid VersionId { get; set; }
    /// <summary>Gets or sets the article id value.</summary>
    public Guid ArticleId { get; set; }
    /// <summary>Gets or sets the version number value.</summary>
    public int VersionNumber { get; set; }
    /// <summary>Gets or sets the title value.</summary>
    public string Title { get; set; } = "";
    /// <summary>Gets or sets the status id value.</summary>
    public int StatusId { get; set; }
    /// <summary>Gets or sets the created by value.</summary>
    public Guid CreatedBy { get; set; }
    /// <summary>Gets or sets the created at value.</summary>
    public DateTime CreatedAt { get; set; }
}

/// <summary>Represents the GenericCollectionDemo type.</summary>
public static class GenericCollectionDemo
{
    /// <summary>Runs the demonstration.</summary>
    public static void Run()
    {

        Console.WriteLine("=== GENERIC COLLECTIONS DEMO ===");

        List<KnowledgeBaseArticle> articles = CreateArticles();

        // IEnumerable<T> — browse articles without needing to modify the collection.
        // Concrete class: List<KnowledgeBaseArticle>
        IEnumerable<KnowledgeBaseArticle> articleFeed = articles;

        Console.WriteLine("=== IEnumerable: Browse published articles ===");

        foreach (KnowledgeBaseArticle article in articleFeed)
        {
            ArticleVersion? publishedVersion = GetPublishedVersion(article);

            if (publishedVersion != null)
            {
                Console.WriteLine(
                    $"{publishedVersion.Title} | Article ID: {article.ArticleId}");
            }
        }

        // ICollection<T> — add, remove, and count an article's versions.
        // Concrete class: List<ArticleVersion>
        KnowledgeBaseArticle firstArticle = articles[0];
        ICollection<ArticleVersion> versionCollection =
            new List<ArticleVersion>(firstArticle.Versions);

        ArticleVersion draftVersion = new ArticleVersion
        {
            VersionId = Guid.NewGuid(),
            ArticleId = firstArticle.ArticleId,
            VersionNumber = 5,
            Title = "Draft: Improving SQL Query Performance",
            StatusId = 2, // Example status lookup ID; not written to the database.
            CreatedBy = firstArticle.AuthorId,
            CreatedAt = new DateTime(2026, 10, 6)
        };

        Console.WriteLine("\n=== ICollection: Manage article versions ===");
        Console.WriteLine($"Versions before adding draft: {versionCollection.Count}");

        versionCollection.Add(draftVersion);
        Console.WriteLine($"Versions after adding draft: {versionCollection.Count}");

        versionCollection.Remove(draftVersion);
        Console.WriteLine($"Versions after removing draft: {versionCollection.Count}");

        // IList<T> — preserve version order and access a version by index.
        // Concrete class: List<ArticleVersion>
        IList<ArticleVersion> orderedVersions =
            firstArticle.Versions
                .OrderBy(version => version.VersionNumber)
                .ToList();

        Console.WriteLine("\n=== IList: Access ordered article versions ===");

        for (int index = 0; index < orderedVersions.Count; index++)
        {
            ArticleVersion version = orderedVersions[index];
            Console.WriteLine($"Version {version.VersionNumber}: {version.Title}");
        }

        // IDictionary<TKey, TValue> — find an article using its primary key.
        // Concrete class: Dictionary<Guid, KnowledgeBaseArticle>
        IDictionary<Guid, KnowledgeBaseArticle> articlesById =
            articles.ToDictionary(article => article.ArticleId);

        Console.WriteLine("\n=== IDictionary: Find an article by ID ===");

        Guid searchArticleId = articles[1].ArticleId;

        if (articlesById.TryGetValue(searchArticleId, out KnowledgeBaseArticle? foundArticle))
        {
            ArticleVersion? publishedVersion = GetPublishedVersion(foundArticle);
            Console.WriteLine(
                publishedVersion == null
                    ? "The article has no published version."
                    : $"Found: {publishedVersion.Title}");
        }

        // ISet<T> — keep an article's linked tag IDs unique.
        // Concrete class: HashSet<int>
        ISet<int> tagIds = new HashSet<int>(firstArticle.TagIds);

        Console.WriteLine("\n=== ISet: Keep article tags unique ===");
        Console.WriteLine($"Added tag ID 4: {tagIds.Add(4)}");
        Console.WriteLine($"Added tag ID 4 again: {tagIds.Add(4)}");
        Console.WriteLine($"Unique tag IDs: {string.Join(", ", tagIds)}");

        // Stack<T> — undo version changes in reverse order.
        // Concrete class: Stack<ArticleVersion>
        Stack<ArticleVersion> versionUndoStack = new Stack<ArticleVersion>();

        foreach (ArticleVersion version in orderedVersions)
        {
            versionUndoStack.Push(version);
        }

        Console.WriteLine("\n=== Stack: Undo newest version first ===");

        if (versionUndoStack.Count > 0)
        {
            ArticleVersion latestVersion = versionUndoStack.Pop();
            Console.WriteLine(
                $"Most recent version to undo: {latestVersion.VersionNumber} - {latestVersion.Title}");
        }

        // Queue<T> — send pending versions for editorial review in submission order.
        // Concrete class: Queue<ArticleVersion>
        Queue<ArticleVersion> editorialReviewQueue = new Queue<ArticleVersion>();

        foreach (KnowledgeBaseArticle article in articles)
        {
            foreach (ArticleVersion version in article.Versions)
            {
                if (version.VersionId != article.CurrentPublishedVersionId)
                {
                    editorialReviewQueue.Enqueue(version);
                }
            }
        }

        Console.WriteLine("\n=== Queue: Review pending versions in submission order ===");

        while (editorialReviewQueue.Count > 0)
        {
            ArticleVersion nextVersion = editorialReviewQueue.Dequeue();
            Console.WriteLine(
                $"Reviewing: {nextVersion.Title} (version {nextVersion.VersionNumber})");
        }
    }

    private static ArticleVersion? GetPublishedVersion(KnowledgeBaseArticle article)
    {
        if (article.CurrentPublishedVersionId == null)
        {
            return null;
        }

        return article.Versions.FirstOrDefault(
            version => version.VersionId == article.CurrentPublishedVersionId);
    }

    private static List<KnowledgeBaseArticle> CreateArticles()
    {
        Guid authorAsha = Guid.Parse("a1111111-1111-4111-8111-111111111111");
        Guid authorRavi = Guid.Parse("b2222222-2222-4222-8222-222222222222");

        Guid articleId1 = Guid.Parse("c3333333-3333-4333-8333-333333333333");
        Guid articleId2 = Guid.Parse("d4444444-4444-4444-8444-444444444444");
        Guid articleId3 = Guid.Parse("e5555555-5555-4555-8555-555555555555");

        Guid article1Version1 = Guid.Parse("f6666666-6666-4666-8666-666666666661");
        Guid article1Version2 = Guid.Parse("f6666666-6666-4666-8666-666666666662");
        Guid article2Version1 = Guid.Parse("f6666666-6666-4666-8666-666666666663");
        Guid article2Version2 = Guid.Parse("f6666666-6666-4666-8666-666666666664");
        Guid article3Version1 = Guid.Parse("f6666666-6666-4666-8666-666666666665");

        return new List<KnowledgeBaseArticle>
        {
            new KnowledgeBaseArticle
            {
                ArticleId = articleId1,
                AuthorId = authorAsha,
                CurrentPublishedVersionId = article1Version1,
                CreatedAt = new DateTime(2026, 9, 20),
                TagIds = new HashSet<int> { 1, 2 },
                Versions = new List<ArticleVersion>
                {
                    new ArticleVersion
                    {
                        VersionId = article1Version1,
                        ArticleId = articleId1,
                        VersionNumber = 1,
                        Title = "Getting Started with SQL",
                        StatusId = 1,
                        CreatedBy = authorAsha,
                        CreatedAt = new DateTime(2026, 9, 20)
                    },
                    new ArticleVersion
                    {
                        VersionId = article1Version2,
                        ArticleId = articleId1,
                        VersionNumber = 2,
                        Title = "Getting Started with SQL: Revised Edition",
                        StatusId = 2,
                        CreatedBy = authorAsha,
                        CreatedAt = new DateTime(2026, 10, 2)
                    }
                }
            },
            new KnowledgeBaseArticle
            {
                ArticleId = articleId2,
                AuthorId = authorRavi,
                CurrentPublishedVersionId = article2Version1,
                CreatedAt = new DateTime(2026, 9, 23),
                TagIds = new HashSet<int> { 2, 3 },
                Versions = new List<ArticleVersion>
                {
                    new ArticleVersion
                    {
                        VersionId = article2Version1,
                        ArticleId = articleId2,
                        VersionNumber = 3,
                        Title = "Understanding SQL Joins",
                        StatusId = 1,
                        CreatedBy = authorRavi,
                        CreatedAt = new DateTime(2026, 9, 23)
                    },
                    new ArticleVersion
                    {
                        VersionId = article2Version2,
                        ArticleId = articleId2,
                        VersionNumber = 4,
                        Title = "Understanding SQL Joins with Examples",
                        StatusId = 2,
                        CreatedBy = authorRavi,
                        CreatedAt = new DateTime(2026, 10, 4)
                    }
                }
            },
            new KnowledgeBaseArticle
            {
                ArticleId = articleId3,
                AuthorId = authorAsha,
                CurrentPublishedVersionId = article3Version1,
                CreatedAt = new DateTime(2026, 9, 28),
                TagIds = new HashSet<int> { 1, 4 },
                Versions = new List<ArticleVersion>
                {
                    new ArticleVersion
                    {
                        VersionId = article3Version1,
                        ArticleId = articleId3,
                        VersionNumber = 5,
                        Title = "Using Database Transactions",
                        StatusId = 1,
                        CreatedBy = authorAsha,
                        CreatedAt = new DateTime(2026, 9, 28)
                    }
                }
            }
        };
    }
}
