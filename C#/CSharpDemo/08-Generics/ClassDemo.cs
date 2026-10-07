using System;
using System.Linq;
using System.Collections.Generic;
namespace CSharpDemo.Generics;

/*
Generic Classes
*/

/*
Without generics, we might separate classes for different types:
class IntBox {
    int value;
}
class StringBox { 
    string value;
}
class DoubleBox { 
    double value;
}

The classes are doing exactly the same thing. The only difference is the type of value. Generics solves this problem.
*/



/*
Instead of creating three classes, we create one generic class.
class Box<T> {
    T value;
}

Here, T is a type parameter. It means: "I dont know actual type yet. The person using this class will specify it."

Example:
Box<int> b1 = new Box<int>();
b1.value = 10;

Box<string> b2 = new Box<string>();
b2.value = "hello";
*/



/*
Generic constraints restrict what types can be used for T. "T must satisfy some condition."

1. where T : class
This means T must be a reference type.
(string allowed, int not allowed)

class Box<T> where T : class {
    public T value;
}


2. where T : struct
This means T must be a value type.
(int allowed, string not allowed)

class Box<T> where T : struct {
    public T value;
}


3. where T : SomeInterface
T is guaranteed to implement some interface. 

interface IEntity {
    int Id { get; }
}

class Article : IEntity {
    public int Id { get; set; }
}

class User : IEntity { 
    public int Id { get; set; }
}

class Repository<T> where T : IEntity {

    public void PrintId(T entity) {
        Console.WriteLine(entity.Id);
    }
}

Both Repository<Article> and Repository<User> are valid because they implement IEntity.
*/

public class Article
{
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}

public class Page<T>
{
    public List<T> Items { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalItems { get; }
    public int TotalPages { get; }

    public Page(List<T> items, int pageNumber, int pageSize, int totalItems)
    {
        Items = items;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalItems = totalItems;
        TotalPages = (totalItems + pageSize - 1) / pageSize;
    }
}

public static class ClassDemoGen
{
    public static void Run()
    {

        Console.WriteLine("=== GENERIC CLASSES DEMO ===");

        var articles = new List<Article>
        {
            new Article { Title = "Getting Started with SQL", Author = "Asha", CreatedAt = new DateTime(2026, 9, 20) },
            new Article { Title = "Understanding Primary and Foreign Keys", Author = "Ravi", CreatedAt = new DateTime(2026, 9, 22) },
            new Article { Title = "A Practical Guide to Database Indexes", Author = "Mina", CreatedAt = new DateTime(2026, 9, 25) },
            new Article { Title = "Using Transactions to Protect Data", Author = "Noah", CreatedAt = new DateTime(2026, 9, 27) },
            new Article { Title = "How SQL Joins Work", Author = "Asha", CreatedAt = new DateTime(2026, 9, 29) },
            new Article { Title = "Writing Useful Database Views", Author = "Ravi", CreatedAt = new DateTime(2026, 10, 1) },
            new Article { Title = "Working with Stored Procedures", Author = "Mina", CreatedAt = new DateTime(2026, 10, 2) },
            new Article { Title = "Filtering and Sorting Query Results", Author = "Noah", CreatedAt = new DateTime(2026, 10, 3) },
            new Article { Title = "Introduction to Database Normalization", Author = "Asha", CreatedAt = new DateTime(2026, 10, 4) },
            new Article { Title = "Backing Up and Restoring a Database", Author = "Ravi", CreatedAt = new DateTime(2026, 10, 5) },
            new Article { Title = "Understanding Aggregate Functions", Author = "Mina", CreatedAt = new DateTime(2026, 10, 6) },
            new Article { Title = "Pagination with LIMIT and OFFSET", Author = "Noah", CreatedAt = new DateTime(2026, 10, 7) }
        };

        int pageSize = 4;
        int totalPages = (articles.Count + pageSize - 1) / pageSize;

        for (int pageNumber = 1; pageNumber <= totalPages; pageNumber++)
        {
            Page<Article> page = GetArticlePage(articles, pageNumber, pageSize);

            Console.WriteLine(
                $"\nPage {page.PageNumber} of {page.TotalPages} " +
                $"({page.TotalItems} articles total)");

            foreach (Article article in page.Items)
            {
                Console.WriteLine(
                    $"{article.Title} | {article.Author} | {article.CreatedAt:yyyy-MM-dd}");
            }
        }
    }

    private static Page<Article> GetArticlePage(
        List<Article> articles,
        int pageNumber,
        int pageSize)
    {
        if (pageNumber < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageNumber));
        }

        if (pageSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize));
        }

        List<Article> pageItems = articles
            .OrderByDescending(article => article.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new Page<Article>(
            pageItems,
            pageNumber,
            pageSize,
            articles.Count);
    }
}