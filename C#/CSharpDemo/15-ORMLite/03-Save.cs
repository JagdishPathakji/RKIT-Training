using ServiceStack.OrmLite;

namespace CSharpDemo.OrmLiteDemo;

public static class SaveDemo
{
    public static void Run(System.Data.IDbConnection db)
    {
        Console.WriteLine("1. Save a new tag");
        Console.WriteLine("2. Save changes to an existing tag");
        Console.Write("Choose: ");

        switch (Console.ReadLine())
        {
            case "1":
                SaveNewTag(db);
                break;
            case "2":
                SaveExistingTag(db);
                break;
            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }

    private static void SaveNewTag(System.Data.IDbConnection db)
    {
        Console.Write("Enter the new tag name: ");
        string name = ReadRequiredInput();

        db.Save(new Tag { Name = name });
        Console.WriteLine("Tag saved. With no tag_id, Save inserts a new row.");
    }

    private static void SaveExistingTag(System.Data.IDbConnection db)
    {
        List<Tag> tags = db.Select<Tag>();
        if (tags.Count == 0)
        {
            Console.WriteLine("There are no tags to update. Save a new tag first.");
            return;
        }

        Console.WriteLine("Existing tags:");
        foreach (Tag tag in tags)
        {
            Console.WriteLine($"{tag.TagId}. {tag.Name}");
        }

        Console.Write("Enter the tag_id to update: ");
        if (!int.TryParse(Console.ReadLine(), out int tagId))
        {
            Console.WriteLine("Please enter a valid numeric tag_id.");
            return;
        }

        Tag? existingTag = db.SingleById<Tag>(tagId);
        if (existingTag is null)
        {
            Console.WriteLine($"No tag exists with tag_id {tagId}.");
            return;
        }

        Console.Write($"Enter the new name for '{existingTag.Name}': ");
        existingTag.Name = ReadRequiredInput();

        db.Save(existingTag);
        Console.WriteLine("Tag saved. With an existing tag_id, Save updates the row.");
    }

    private static string ReadRequiredInput()
    {
        string? value = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException("Tag name cannot be empty.");
        }

        return value.Trim();
    }
}