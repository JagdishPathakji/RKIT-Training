using System;
namespace CSharpDemo.Enums;

public enum ArticleStatus
{
    None = 0,
    Draft = 1,
    PendingEditorReview = 2,
    NeedsImprovement = 3,
    Rejected = 4,
    Published = 5
}


public enum Role
{
    None = 0,
    Reviewer = 1,
    Author = 2,
    Editor = 3
}


public enum BlockType
{
    None = 0,
    Text = 1,
    Code = 2
}

public class EnumsDemo
{
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