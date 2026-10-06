/*
Constants in Interfaces:=
A constant is a value that cannot be changed after it is declared.
1. constants are automatically public
2. the class does not need to implement it
3. you cannot make it private
4. constants belongs to an interface rather than a particular class
that's why access using InterfaceName.ConstantName
5. Inside implementing class as well the constant is accessed as:-
InterfaceName.ConstantName
6. constants cannot be changed after declared
7. You don't create an object to access a constant (static-like)
*/

using System;

interface IKnowledgeContent
{
    const int Draft = 1;
    const int PendingEditorReview = 2;
    const int NeedsImprovement = 3;
    const int Rejected = 4;
    const int Published = 5;

    int ContentId { get; }
    string Title { get; set; }
    int StatusId { get; set; }
}

class Article : IKnowledgeContent
{
    public int ContentId { get; }

    public string Title { get; set; }

    public int StatusId { get; set; }

    public Article(int contentId, string title)
    {
        ContentId = contentId;
        Title = title;
        StatusId = IKnowledgeContent.Draft;
    }

    public void Publish()
    {
        StatusId = IKnowledgeContent.Published;
    }

    public void ShowStatus()
    {
        if (StatusId == IKnowledgeContent.Draft)
        {
            Console.WriteLine("Draft");
        }
        else if (StatusId == IKnowledgeContent.PendingEditorReview)
        {
            Console.WriteLine("Pending Editor Review");
        }
        else if (StatusId == IKnowledgeContent.NeedsImprovement)
        {
            Console.WriteLine("Needs Improvement");
        }
        else if (StatusId == IKnowledgeContent.Rejected)
        {
            Console.WriteLine("Rejected");
        }
        else if (StatusId == IKnowledgeContent.Published)
        {
            Console.WriteLine("Published");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Article article = new Article(101,"Understanding C# Interfaces");

        Console.WriteLine("Content ID: " + article.ContentId);
        Console.WriteLine("Title: " + article.Title);

        Console.Write("Initial Status: ");
        article.ShowStatus();

        article.Publish();

        Console.Write("After Publishing: ");
        article.ShowStatus();

        Console.WriteLine();

        Console.WriteLine("Published Status ID: " +IKnowledgeContent.Published);
    }
}