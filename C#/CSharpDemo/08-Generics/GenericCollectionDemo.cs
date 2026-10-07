using System;
using System.Linq;
using System.Collections.Generic;
namespace CSharpDemo.Generics;

/*
IEnumerable<T> provides an enumerator that starts at the begining of a collection and moves forward one step at a time until it reaches the end.

If a class implements `IEnumerable`, it makes a single simple promise: "You can loop through my items one by one".

Because `IEnumerable<T>` is at the very bottom of the capability ladder, it is highly restricted.
-- You cannot add items
-- You cannot remove items
-- You cannot access items by an index number
-- It does not know how many items it has
*/



/*
ICollection<T> inherits from IEnumerable<T>. That means ICollection<T> can do everything an IEnumerable<T> can do but it adds a whole new set of rules.

If a class implements ICollection<T>, it promises: "You can add items, remove items, and know how many items i am holding".

By using ICollection<T>, we gain access to essential methods and properties:
-- .Count: tells how many items are in the collection
-- .Add(item): puts a new item into the collection
-- Remove(item): takes a specific item out
-- Clear(): wipes the entire collection clean
-- .Contains(item): checks if a specific item exists inside the collection and returns true or false

The Limitation: No Index Based Access
ICollection<T> still does not understand the concept of position. It treats data like a bag of marbles. You know how many marbles are in the bag, and you can take a red marble out, but you cannot ask for "the 3rd marble".
*/



/*
IList<T> inherits from ICollection<T>. This means an IList<T> is an IEnumerable<T> and an ICollection<T>. It does everything what parent classes do. 

If a class implements IList<T>, it promises: "I keep all items in a specific order, and you can access or change any item instantly if you know its index number."

By using IList<T>, we have following additional methods:
-- [index]: can access items via index
-- .Insert(index,item): inserts a new item into a specific position and pushing 
everything else down
-- .RemoveAt(index): deletes whatever is at that index
-- .IndexOf(item): searches for an item and tells you its exact index number or return -1 if it is not present in the IList<T>.
*/



/*
IDictionary<TKey,TValue> dictates how any key-value collection must act. It also implements ICollection and indirectly IEnumerable.

If a class signs the IDictionary<TKey,TValue> contract, it promises: "I map unique key to values and you can instantly look up a value if you give me its key."

It forces the class to implement:
-- [key]: the key-based access like index
-- .Add(key,value): adds a new pair
-- .Contains(key): check if key is present
-- .Keys and .Values: lets you grab just the list of key or just list of values in form of ICollections. 
-- .Remove(key): removes key and value

If you want to add or remove items, you must do it through the IDictionary itself (using dict.Add() or dict.Remove()). The collections returned by .Keys and .Values are strictly for looking at the data or looping through it.
*/



/*
ISet<T> completely ignores order and indexing, If a class implements ISet<T>, it promises: "I absolutely will now allow duplicates, and I am optimized for math-like set operations".

Properties of ISet<T>:
-- Uniqueness: If you try to add "Apple" 100 times, it will only keep one "Apple". It silently ignores duplicates.

-- No Order, No Index: You cannot ask for item [0]. It treats items like a chaotic bag.

-- Blazing Fast .Contains(): Checking if a List contains an item requires checking every single item one by one (O(N) time). A ISet uses a "hash code" to instantly know if the item is there, no matter how big the set is (O(1) time).
*/



/*
Stack<T>: Last In First Out. It inherits IEnumerable<T> only. It has methods .Push() and .Pop() which works at top of stack.

Queue<T>: First In First Out. It inherits IEnumerable<T> only. It has methods .Enqueue() which works at back and .Dequeue() which works at front of Queue. 
*/



/*
IEnumerable<T>  (The Root: "I can loop through items")
│
├── ICollection<T>  (The Modifier: "I have a .Count, and can Add/Remove")
│    │
│    ├── IList<T>  (The Indexer: "I maintain order, access by [index]")
│    │    └── List<T>
│    │
│    ├── IDictionary<TKey, TValue>  (The Lookup: "I map Keys to Values")
│    │    └── Dictionary<TKey, TValue>
│    │
│    └── ISet<T>  (The Unique Collection: "I only allow unique items")
│         └── HashSet<T>
│
└── (Direct Implementers of IEnumerable)
    ├── Stack<T>  (LIFO: Last-In, First-Out)
    └── Queue<T>  (FIFO: First-In, First-Out)
*/

public class KnowledgeBaseArticle
{
    // ARTICLE table
    public Guid ArticleId { get; set; }
    public Guid AuthorId { get; set; }
    public Guid? CurrentPublishedVersionId { get; set; }
    public DateTime CreatedAt { get; set; }

    // ARTICLE_VERSION rows belonging to this article.
    // Concrete class used: List<ArticleVersion>
    public IList<ArticleVersion> Versions { get; set; }
        = new List<ArticleVersion>();

    // TAG IDs linked through ARTICLE_TAG.
    // Concrete class used: HashSet<int>
    public ISet<int> TagIds { get; set; }
        = new HashSet<int>();
}

public class ArticleVersion
{
    // ARTICLE_VERSION table
    public Guid VersionId { get; set; }
    public Guid ArticleId { get; set; }
    public int VersionNumber { get; set; }
    public string Title { get; set; } = "";
    public int StatusId { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
}

public static class GenericCollectionDemo
{
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