using ServiceStack.OrmLite;

namespace CSharpDemo.OrmLiteDemo;

public static class InsertDemo
{
    public static void RunInsert(System.Data.IDbConnection db)
    {
        Console.Write("Enter a new tag name: ");
        string name = ReadRequiredInput();

        var tag = new Tag { Name = name };
        db.Insert(tag);

        Console.WriteLine("Tag inserted.");
    }

    public static void RunInsertOnly(System.Data.IDbConnection db)
    {
        Console.Write("Enter a new tag name for InsertOnly: ");
        string name = ReadRequiredInput();

        var tag = new Tag { Name = name };

        // Insert only the Name column. MySQL generates tag_id.
        db.InsertOnly(tag, x => x.Name);

        Console.WriteLine("Tag inserted with InsertOnly.");
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
