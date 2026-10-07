using System.Data;
using ServiceStack.OrmLite;

namespace CSharpDemo.OrmLiteDemo;

/// <summary>Demonstrates deleting tags by object, condition, one ID, or multiple IDs.</summary>
public static class DeleteDemo
{
    // Demonstrates delete methods using temporary tags and user-entered tag IDs.
    /// <summary>Runs the demonstration.</summary>
    /// <param name="db">An open connection to the knowledge_base database.</param>
    public static void Run(IDbConnection db)
    {
        DisplayTags(db);

        Tag singleTag = CreateDemoTag(db, "Delete(object)");
        Console.WriteLine($"Delete(object): {db.Delete(singleTag)} row deleted by primary key.");
        DisplayTags(db);

        Tag whereTag = CreateDemoTag(db, "Delete with WHERE");
        Console.WriteLine(
            $"Delete with WHERE: {db.Delete<Tag>(x => x.Name == whereTag.Name)} row deleted.");
        DisplayTags(db);

        Tag byIdTag = CreateDemoTag(db, "DeleteById");
        Console.WriteLine(
            $"DeleteById: {db.DeleteById<Tag>(byIdTag.TagId)} row deleted by primary key.");
        DisplayTags(db);

        Console.Write("Enter tag IDs to delete, separated by commas: ");
        string? input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("No tag IDs entered; DeleteByIds was skipped.");
            return;
        }

        var tagIds = new List<int>();
        foreach (string value in input.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
        {
            if (!int.TryParse(value, out int tagId))
            {
                Console.WriteLine($"'{value}' is not a valid tag ID; DeleteByIds was skipped.");
                return;
            }

            tagIds.Add(tagId);
        }

        if (tagIds.Count == 0)
        {
            Console.WriteLine("No tag IDs entered; DeleteByIds was skipped.");
            return;
        }

        int deletedCount = db.DeleteByIds<Tag>(tagIds.ToArray());
        Console.WriteLine($"DeleteByIds: {deletedCount} rows deleted by their primary keys.");
        DisplayTags(db);

        // Do not run these: they would delete every tag or remove the real tag table.
        // db.DeleteAll<Tag>();
        // db.DropTable<Tag>();
    }

    // Prompts for a tag name and inserts a tag for the selected delete example.
    private static Tag CreateDemoTag(IDbConnection db, string example)
    {
        Console.Write($"Enter a unique tag name for {example}: ");
        string? name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidOperationException("Tag name cannot be empty.");
        }

        var tag = new Tag { Name = name.Trim() };
        tag.TagId = Convert.ToInt32(db.Insert(tag, selectIdentity: true));
        return tag;
    }

    // Prints the current contents of the tag table.
    private static void DisplayTags(IDbConnection db)
    {
        Console.WriteLine("\nCurrent tag table:");
        List<Tag> tags = db.Select<Tag>();
        if (tags.Count == 0)
        {
            Console.WriteLine("(no tags)");
            return;
        }

        foreach (Tag tag in tags)
        {
            Console.WriteLine($"{tag.TagId}: {tag.Name}");
        }
    }
}
