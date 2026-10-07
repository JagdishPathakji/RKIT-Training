using System;
namespace CSharpDemo.Enums;

/// <summary>Represents the ArticleStatus type.</summary>
public enum ArticleStatus
{
    /// <summary>No article status has been assigned.</summary>
    None = 0,
    /// <summary>The article is a draft.</summary>
    Draft = 1,
    /// <summary>The article is awaiting editor review.</summary>
    PendingEditorReview = 2,
    /// <summary>The article needs changes before approval.</summary>
    NeedsImprovement = 3,
    /// <summary>The article was rejected.</summary>
    Rejected = 4,
    /// <summary>The article is published.</summary>
    Published = 5
}


/// <summary>Represents the Role type.</summary>
public enum Role
{
    /// <summary>No role has been assigned.</summary>
    None = 0,
    /// <summary>The user reviews articles.</summary>
    Reviewer = 1,
    /// <summary>The user writes articles.</summary>
    Author = 2,
    /// <summary>The user edits and approves articles.</summary>
    Editor = 3
}


/// <summary>Represents the BlockType type.</summary>
public enum BlockType
{
    /// <summary>No block type has been assigned.</summary>
    None = 0,
    /// <summary>The block contains regular text.</summary>
    Text = 1,
    /// <summary>The block contains code.</summary>
    Code = 2
}

/// <summary>Represents the EnumsDemo type.</summary>
public class EnumsDemo
{
    /// <summary>Runs the demonstration.</summary>
    public void Run()
    {
        ConsoleHelper.Clear();

        Console.WriteLine("=== ENUMERATIONS DEMO ===");


        // 1. Declare and use enum
        ArticleStatus status = ArticleStatus.Draft;
        Console.WriteLine($"\nArticle Status: {status}");


        // 2. Enum comparison
        status = ArticleStatus.Published;
        if(status == ArticleStatus.Published)
        {
            Console.WriteLine("Article is published.");
        }


        // 3. Enum to integer
        int statusValue = (int)status;
        Console.WriteLine($"Status value: {statusValue}");


        // 4. Integer to enum
        int databaseValue = 2;
        ArticleStatus convertedStatus = (ArticleStatus)databaseValue;
        Console.WriteLine($"Database value {databaseValue} = {convertedStatus}");


        // 5. Validate external value
        int externalValue = 99;
        Console.WriteLine(
            $"Is {externalValue} valid? " +
            $"{Enum.IsDefined(typeof(ArticleStatus), externalValue)}"
        );


        // 6. Switch with enum
        status = ArticleStatus.NeedsImprovement;
        switch (status)
        {
            case ArticleStatus.Draft:
                Console.WriteLine("Article is being written.");
                break;

            case ArticleStatus.PendingEditorReview:
                Console.WriteLine("Waiting for editor review.");
                break;

            case ArticleStatus.NeedsImprovement:
                Console.WriteLine("Article needs improvement.");
                break;

            case ArticleStatus.Rejected:
                Console.WriteLine("Article was rejected.");
                break;

            case ArticleStatus.Published:
                Console.WriteLine("Article is published.");
                break;
        }


        // 7. Other Knowledge Base enums
        Role role = Role.Author;
        BlockType blockType = BlockType.Code;

        Console.WriteLine($"\nUser Role: {role}");
        Console.WriteLine($"Block Type: {blockType}");


        // 8. Get all enum values
        Console.WriteLine("\nArticle Statuses:");
        foreach (ArticleStatus item in Enum.GetValues<ArticleStatus>())
        {
            Console.WriteLine($"{item} = {(int)item}");
        }

        // 9. Get all enum names
        Console.WriteLine("\nAll enum names:");
        foreach (string name in
                 Enum.GetNames<ArticleStatus>())
        {
            Console.WriteLine(name);
        }

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey(true);
    }
}
