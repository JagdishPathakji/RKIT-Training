// Multiple inheritance in interfaces

using System;

interface IAuthorReview
{
    void Review();
}

interface IEditorReview
{
    void Review();
}

class Article : IAuthorReview, IEditorReview
{
    public int ArticleId { get; }

    public string Title { get; set; }

    public Article(int articleId, string title)
    {
        ArticleId = articleId;
        Title = title;
    }

    // can't access them using article object directly, we must use interface reference
    void IAuthorReview.Review()
    {
        Console.WriteLine(
            "Author review: checking article content and correctness."
        );
    }

    // can't access them using article object directly, we must use interface reference
    void IEditorReview.Review()
    {
        Console.WriteLine(
            "Editor review: checking article quality and publication readiness."
        );
    }
}

class Program
{
    static void Main(string[] args)
    {
        Article article = new Article(
            101,
            "Understanding C# Interfaces"
        );

        IAuthorReview authorReview = article;
        IEditorReview editorReview = article;

        Console.WriteLine("Article ID: " + article.ArticleId);
        Console.WriteLine("Title: " + article.Title);

        Console.WriteLine();
        authorReview.Review();

        Console.WriteLine();
        editorReview.Review();
    }
}