using System;
using System.Collections.Generic;

interface IEntity
{
    int Id { get; }
}

class Article : IEntity
{
    public int Id { get; set; }
    public string Title { get; set; }

    public Article(int id, string title)
    {
        Id = id;
        Title = title;
    }
}

class Category : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }

    public Category(int id, string name)
    {
        Id = id;
        Name = name;
    }
}

class KnowledgeBase
{
    // Generic Method and Generic Collection
    public T FindById<T>(List<T> items, int id)
        where T : IEntity
    {
        foreach (T item in items)
        {
            if (item.Id == id)
            {
                return item;
            }
        }

        return default; // "return the zero/null/false value appropriate for this type."
    }
}

class Program
{
    static void Main()
    {
        List<Article> articles = new List<Article>
        {
            new Article(1, "C# Interfaces"),
            new Article(2, "C# Generics"),
            new Article(3, "C# Inheritance")
        };

        List<Category> categories = new List<Category>
        {
            new Category(1, "C#"),
            new Category(2, "Database"),
            new Category(3, "Programming")
        };

        KnowledgeBase kb = new KnowledgeBase();

        // Find an Article
        Article article = kb.FindById(articles, 2);
        Console.WriteLine(article.Title);

        // Find a Category
        Category category = kb.FindById(categories, 3);
        Console.WriteLine(category.Name);
    }
}