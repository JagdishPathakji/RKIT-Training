=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
LESSON 1 -- WHY LINQ ?
=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

Before learning LINQ, let us understand the problem it solves. 

Suppose our KnowledgeBase application has:
    List<Article> articles;

Each Article contains:
    ArticleId
    Title
    StatusId
    AuthorId

Now suppose we want:
    "Give me all published articles"

Wihout LINQ, we can easily write:
    List<Article> published = new List<Article>();

    foreach(Article article in articles) {
        if(article.StatusId == 5) {
            published.Add(article);
        }
    }

This works. So the question is:
    "WHY DO WE NEED LINQ?"

Because applications constantly need to perform operations on data.
For example:
    1. find all published articles
    2. find articles written by particular author
    3. find articles whose title contains "SQL"
    4. sort articles by title
    5. find the first article with status "published"
    6. count articles

Without LINQ, we frequently write loops to perform these operations.
However, In each problem, the exact requirement changes, but the basic process is repeated:
    COLLECTION --> Examine each element --> Apply some logic --> Produce a result

LINQ gives us a standard way to express these operations. The important idea is:
    LINQ = Language Integrated Query
    It is a set of C#/.NET features used to query and manipulate data.
    It provides a shorter way to query data instead of writing loop everytime.

Example:
    articles.Where(a => a.StatusId == 5);

The advantage of LINQ is that it gives us reusable operations for common data-processing tasks and lets us compose those operations.

For example, eventually we can express:
    articles
        .Where(a => a.StatusId == 5)
        .Where(a => a.AuthorId == 101)
        .OrderBy(a => a.Title)
        .Select(a => a.Title)
    
Each operation produces something that can be passed to the next operation. That ability to combine operations is one of the most important ideas in LINQ.

SQL is primarily used to query databases. LINQ is part of C#/.NET and can work with many kinds of data.




=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
LESSON 2 -- IEnumerable<T> and LINQ Hierarchy
=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
In Lesson 1 we saw:
    articles.Where(a => a.StatusId == 5);

The question is: 
    How does .Where() work on List<Article> ?

List<T> does not define a method called Where(). So where does it come from ?

The answer is: LINQ methods are "Extension Methods" defined on IEnumerable<T>. Since List<Article> implements IEnumerable<Article>, it automatically gets access to Where(), Select(), OrderBy(), Count(), and other LINQ methods. They all live in the static class `System.Linq.Enumerable` and target `IEnumerable<T>`.

This is why understanding IEnumerable<T> is the real foundation of LINQ.
Everything else is built on top of it.

Now let's see where List<T> and other collections fit in.

    IEnumerable            (non-generic, old .NET 1.0 style, works with 'object')
        |
    IEnumerable<T>         (generic version, type-safe, this is what LINQ targets)
        |
    ICollection<T>         (adds Count, Add, Remove, Contains)
        |
    IList<T>               (adds indexer: list[i], Insert, RemoveAt)
        |
    List<T>                (the concrete class you actually use)

So when you declare:

    List<Article> articles;

articles is-a IList<Article>, which is-a ICollection<Article>,
which is-a IEnumerable<Article>.

Because of this chain, EVERY LINQ method that expects an IEnumerable<T>
happily accepts a List<T>, an Article[], a HashSet<Article>,
a Dictionary<T,V>.Values, or literally anything that can be walked
element-by-element.

This is the whole point: LINQ doesn't care what kind of collection you
have. It only cares that it can be enumerated.


Going for LINQ with DataTable:
    DataTable does NOT implement IEnumerable<T> directly usable for LINQ.

    DataTable has a property:
        DataTable.Rows --> DataRowCollection

DataRowCollection implements the OLD non-generic IEnumerable (not IEnumerable<T>). So this will NOT compile:
    dataTable.Rows.Where(r => ...);   // ERROR

Because Where() needs IEnumerable<T>, and DataRowCollection only gives
IEnumerable (of plain object, non-typed).

To fix this, .NET gives us an extension method
    dataTable.AsEnumerable().Where(r => ...)

AsEnumerable() converts DataRowCollection into IEnumerable<DataRow>,
which THEN allows every LINQ method to work.

"LINQ methods need IEnumerable<T>. DataTable's row collection isn't
    one by default, so we must convert it first."

    IEnumerable<T>  -->  "LINQ to Objects" — works on in-memory data
                         (List<T>, Array, DataTable rows after AsEnumerable)
                         Runs actual C# code, in memory, line by line.

    IQueryable<T>   -->  used for LINQ to SQL / Entity Framework
                         Translates your LINQ into SQL and runs it on
                         the database server instead of in memory.


 


=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
LESSON 3 -- FILTERING DEEP DIVE (WHERE)
=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

----- THE SIGNATURE -----
Where() has TWO overloads in System.Linq.Enumerable:

    <!-- overload 1 -->
    public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)

    Example:
        List<Article> published = articles
                                    .Where(a => a.StatusId == 5)
                                    .ToList();

    Note: Where() does NOT create a new List<Article> by itself.
    It returns IEnumerable<Article>. Calling .ToList() at the end is what actually forces it to run and collect results into a real List<Article>.

    <!-- overload 2 -->
    public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)

    Example:
        List<Article> everyOther = articles
                                    .Where((a, index) => index % 2 == 0)
                                    .ToList();
    Here:
        a      = the current Article
        index  = its position in the sequence (0-based)

Both take a "predicate" — a function that looks at an element and
returns true (keep it) or false (throw it away).



----- FILTERING WITH NULL CHECKS -----
A classic mistake is forgetting that a property might be null,
and crashing with a NullReferenceException.

Suppose Article has an optional field:
    string? Summary;

WRONG (crashes if Summary is null for any article):

    var withSql = articles
        .Where(a => a.Summary.Contains("SQL"))
        .ToList();

CORRECT (guard the null first):

    var withSql = articles
        .Where(a => a.Summary != null && a.Summary.Contains("SQL"))
        .ToList();

Alternative using the null-conditional operator:

    var withSql = articles
        .Where(a => a.Summary?.Contains("SQL") == true)
        .ToList();


----- COMMON POINTS ABOUT WHERE() -----
1. Where() does not modify the original list.
2. Assuming Where() runs immediately.
    Example:
        var query = articles.Where(a => a.StatusId == 5);
        articles.Clear();                 // clears the original list!
        var list = query.ToList();        // list will now be EMPTY
    Working:
        1. var query = articles.Where(a => a.StatusId == 5); 
        Nothing gets filtered here.
    
        2. This line just creates a "recipe" that says "whenever someone actually asks me for values, go loop through articles and check StatusId == 5." query is just that unfired recipe, pointing at the articles list.
    
        3. articles.Clear();
        Now the actual articles list is emptied — 0 elements in it. The query variable still exists, but it never "took a copy" of the data — it only stored instructions referring to articles.
    
        4. var list = query.ToList();
        THIS is the moment the recipe actually runs. Only now does it go "ok, let me loop through articles and check the condition." But articles is empty at this point (because of Step 2), so there's nothing to loop through. Result: list has 0 elements.

        Anything that needs a concrete, final answer right now will force the loop to run immediately. .ToList() is only one member of that family.
        Other options:
            - ToList()
            - ToArray()
            - ToDictionary
            - ToHashSet
            - Count()
            - First()
            - Any()
            - Sum()
            - Average()


=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
LESSON 4 -- PROJECTION (SELECT and SELECTMANY)
=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
Select() keeps EVERY element, but TRANSFORMS each one into something else. This is called PROJECTION.

Think of it like this:
    Where()   -->  "which items do I want?"
    Select()  -->  "what shape do I want each item in?"

----- THE SIGNATURE -----
    public static IEnumerable<TResult> Select<TSource, TResult>(
        this IEnumerable<TSource> source,
        Func<TSource, TResult> selector)

Notice there are TWO generic type parameters here — TSource and
TResult. That's the important difference from Where().

Where()'s signature only had ONE type parameter (TSource), because
filtering never changes the type — an IEnumerable<Article> filtered
is still an IEnumerable<Article>.

Select() can go IN as one type and come OUT as a completely
different type. IEnumerable<Article> in, IEnumerable<string> out.
Or IEnumerable<Article> in, IEnumerable<SomeCustomType> out.

Same number of elements go in as come out — Select() never removes
anything. If you want to both filter AND transform, you combine
Where() and Select() together (we'll see this below).

----------------------------------------
VARIATION 1 -- PROJECTING TO A SINGLE PROPERTY
----------------------------------------

If all you need is titles, not full Article objects:

    List<string> titlesOnly = articles
        .Select(a => a.Title)
        .ToList();

`a => a.Title` is the selector: "given an Article, give me back just
its Title." The result type is List<string> — NOT List<Article>
anymore. This is the core idea to absorb: Select() can completely
change what type you're working with.

----------------------------------------
VARIATION 2 -- PROJECTING INTO AN ANONYMOUS TYPE
----------------------------------------

Sometimes you want more than one field, but not the WHOLE object:

    var titleAndAuthor = articles
        .Select(a => new { a.Title, a.AuthorId })
        .ToList();

`new { a.Title, a.AuthorId }` builds a small throwaway object holding
just those two fields. You didn't define a class for it — C# infers
the shape automatically. This is called an ANONYMOUS TYPE.

Anonymous types are great for:
    - quickly shaping data for display
    - passing a "slice" of data somewhere without creating a full class
    - intermediate steps in a LINQ chain

Limitation: you can't return an anonymous type from a method (its
type has no name outside that method), and you can't easily pass it
around as a parameter type either.

----------------------------------------
VARIATION 3 -- PROJECTING INTO A DTO (a real class)
----------------------------------------

DTO = "Data Transfer Object" — a small class whose only job is
carrying a shaped subset of data from one place to another (e.g.
from your business logic to a UI screen, or to an API response).

    public class ArticleDto
    {
        public string Title { get; set; }
        public string AuthorName { get; set; }
    }

    List<ArticleDto> dtos = articles
        .Select(a => new ArticleDto
        {
            Title = a.Title,
            AuthorName = "Author #" + a.AuthorId   // pretend lookup
        })
        .ToList();

Same idea as the anonymous type version, but now it has a real,
reusable, nameable type — you CAN return this from a method, pass it
around, use it anywhere.

Rule of thumb:
    - Anonymous type -> quick, throwaway, stays local to one method
    - DTO class -> reusable, named, crosses method/API boundaries


----------------------------------------
VARIATION 4 -- INDEXED Select()
----------------------------------------

Just like Where() had an indexed overload, Select() does too:

    public static IEnumerable<TResult> Select<TSource, TResult>(
        this IEnumerable<TSource> source,
        Func<TSource, int, TResult> selector)

Example — numbering each item based on its position:

    var numbered = articles
        .Select((a, index) => $"{index + 1}. {a.Title}")
        .ToList();

The index is the POSITION IN THE SEQUENCE at enumeration time — not a stored property like ArticleId.

----------------------------------------
COMBINING Where() AND Select()
----------------------------------------

This is the most common real-world pattern: filter first, then shape
the output. Order matters for readability (filter early = less work
per item during the shaping step):

    var publishedTitles = articles
        .Where(a => a.StatusId == 5)
        .Select(a => a.Title)
        .ToList();

Read it left to right: "start with all articles, keep only published
ones, then give me just their titles."

----------------------------------------
SELECTMANY()
----------------------------------------
The core problem SelectMany solves

Say you have a list of lists:

csharp
List<List<int>> numberGroups = new List<List<int>>
{
    new List<int> { 1, 2, 3 },
    new List<int> { 4, 5 },
    new List<int> { 6 }
};

You want ONE flat list: 1, 2, 3, 4, 5, 6.

If you use Select():

csharp
var result = numberGroups.Select(group => group).ToList();

This gives you back... the same List<List<int>>. Nothing flattened. Because Select() is 1-to-1 — one group in, one group out, still nested.

If you use SelectMany():

csharp
var result = numberGroups.SelectMany(group => group).ToList();

This gives you: [1, 2, 3, 4, 5, 6] — one flat List<int>.

What's happening: for each group (a List<int>), SelectMany reaches inside it and pulls out all its individual numbers, dumping them into one single sequence. That's it. That's the whole trick — "look inside each item's inner collection, and merge everything into one flat result."

Select() is ONE-TO-ONE: one input element produces exactly one output
element. SelectMany() is ONE-TO-MANY, FLATTENED: each input element can
produce a WHOLE COLLECTION, and all those collections get merged into
one single flat sequence.
This matters when your objects contain NESTED collections.

Suppose Article now has tags:

    public class Article
    {
        ...
        public List<string> Tags { get; set; }
    }

    articles[0].Tags = new List<string> { "sql", "database" };
    articles[1].Tags = new List<string> { "linq", "csharp" };
    articles[3].Tags = new List<string> { "sql", "performance" };

Now — "give me every tag used across every article, as one flat
list."

WRONG TOOL (Select() alone):

    var wrong = articles.Select(a => a.Tags).ToList();

This gives you List<List<string>> — a list OF lists. Each article's
tag-list stays nested and separate. Not what we want.

RIGHT TOOL (SelectMany()):

    var allTags = articles
        .SelectMany(a => a.Tags)
        .ToList();

This gives you a single, FLAT List<string>:
    ["sql", "database", "linq", "csharp", "sql", "performance"]



-------------------------------------------
Deferred vs Immediate execution (deep dive)
-------------------------------------------
