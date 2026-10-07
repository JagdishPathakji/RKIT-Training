using ServiceStack.OrmLite;

namespace CSharpDemo.OrmLiteDemo;

/// <summary>Demonstrates parameterized raw SQL CRUD operations on tags.</summary>
public static class RawSqlCrudDemo
{
    /// <summary>Runs the demonstration.</summary>
    /// <param name="db">An open connection to the knowledge_base database.</param>
    public static void Run(System.Data.IDbConnection db)
    {
        Console.WriteLine("\n=== RAW SQL CRUD: TAG ===");
        Console.WriteLine("1. Insert tag");
        Console.WriteLine("2. Update tag");
        Console.WriteLine("3. Delete tag");
        Console.WriteLine("4. Select tags");
        Console.Write("Choose: ");

        switch (Console.ReadLine())
        {
            case "1":
                InsertTag(db);
                break;
            case "2":
                UpdateTag(db);
                break;
            case "3":
                DeleteTag(db);
                break;
            case "4":
                SelectTags(db);
                break;
            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }

    private static void InsertTag(System.Data.IDbConnection db)
    {
        Console.Write("Enter the tag name: ");
        string name = ReadRequiredText("Tag name");

        const string sql = "INSERT INTO tag (name) VALUES (@name)";
        int rowsAffected = db.ExecuteSql(sql, new { name });

        Console.WriteLine($"Rows inserted: {rowsAffected}");
    }

    private static void UpdateTag(System.Data.IDbConnection db)
    {
        Console.Write("Enter the tag_id to update: ");
        if (!int.TryParse(Console.ReadLine(), out int tagId))
        {
            Console.WriteLine("Enter a valid numeric tag_id.");
            return;
        }

        Console.Write("Enter the new tag name: ");
        string name = ReadRequiredText("Tag name");

        const string sql = "UPDATE tag SET name = @name WHERE tag_id = @tagId";
        int rowsAffected = db.ExecuteSql(sql, new { name, tagId });

        Console.WriteLine($"Rows updated: {rowsAffected}");
    }

    private static void DeleteTag(System.Data.IDbConnection db)
    {
        Console.Write("Enter the tag_id to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int tagId))
        {
            Console.WriteLine("Enter a valid numeric tag_id.");
            return;
        }

        Console.Write($"Delete tag_id {tagId}? Type YES to confirm: ");
        if (!string.Equals(Console.ReadLine(), "YES", StringComparison.Ordinal))
        {
            Console.WriteLine("Delete cancelled.");
            return;
        }

        const string sql = "DELETE FROM tag WHERE tag_id = @tagId";
        int rowsAffected = db.ExecuteSql(sql, new { tagId });

        Console.WriteLine($"Rows deleted: {rowsAffected}");
    }

    private static void SelectTags(System.Data.IDbConnection db)
    {
        const string sql = "SELECT tag_id, name FROM tag ORDER BY tag_id";
        List<Tag> tags = db.SqlList<Tag>(sql);

        if (tags.Count == 0)
        {
            Console.WriteLine("No tags found.");
            return;
        }

        Console.WriteLine("\nTags:");
        foreach (Tag tag in tags)
        {
            Console.WriteLine($"{tag.TagId}. {tag.Name}");
        }
    }

    private static string ReadRequiredText(string fieldName)
    {
        string? value = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"{fieldName} cannot be empty.");
        }

        return value.Trim();
    }
}
