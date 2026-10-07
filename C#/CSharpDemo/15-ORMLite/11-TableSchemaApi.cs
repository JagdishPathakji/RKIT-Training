using System.Data;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;

namespace CSharpDemo.OrmLiteDemo;

[Alias("ormlite_schema_api_demo")]
public class SchemaApiDemoTable
{
    [PrimaryKey]
    [AutoIncrement]
    [Alias("demo_id")]
    public int DemoId { get; set; }

    [Alias("original_name")]
    [StringLength(100)]
    public string? OriginalName { get; set; }

    [Alias("altered_name")]
    [StringLength(200)]
    public string? AlteredName { get; set; }

    [Alias("added_name")]
    [StringLength(100)]
    public string? AddedName { get; set; }
}

public static class TableSchemaApiDemo
{
    // Demonstrates schema changes on a temporary table and removes it afterward.
    public static void Run(IDbConnection db)
    {
        if (db.TableExists<SchemaApiDemoTable>())
        {
            Console.WriteLine("The demo table already exists; leaving it untouched.");
            return;
        }

        var tableCreated = false;
        try
        {
            db.CreateTable<SchemaApiDemoTable>();
            tableCreated = true;

            Console.WriteLine($"Table exists: {db.TableExists<SchemaApiDemoTable>()}");
            Console.WriteLine($"original_name exists: {db.ColumnExists<SchemaApiDemoTable>(x => x.OriginalName)}");

            Console.WriteLine("\nTables in the current database:");
            foreach (var name in db.GetTableNames())
                Console.WriteLine($"- {name}");

            Console.WriteLine("\nRename original_name, then restore its name:");
            db.RenameColumn<SchemaApiDemoTable>("original_name", "temporary_name");
            db.RenameColumn<SchemaApiDemoTable>("temporary_name", "original_name");
            Console.WriteLine($"original_name exists again: {db.ColumnExists<SchemaApiDemoTable>(x => x.OriginalName)}");

            Console.WriteLine("\nAlter altered_name to match its mapped VARCHAR(200) definition:");
            /*
            The raw SQL temporarily changes the scratch table’s column to VARCHAR(100).
            AlterColumn(...) reads the POCO mapping and changes the database column back to VARCHAR(200).
            */
            db.ExecuteSql("ALTER TABLE `ormlite_schema_api_demo` MODIFY `altered_name` VARCHAR(100) NULL");
            db.AlterColumn<SchemaApiDemoTable>(x => x.AlteredName);
            Console.WriteLine("altered_name is now VARCHAR(200).");

            Console.WriteLine("\nAdd and drop added_name:");
            db.DropColumn<SchemaApiDemoTable>(x => x.AddedName);
            db.AddColumn<SchemaApiDemoTable>(x => x.AddedName);
            Console.WriteLine($"added_name exists: {db.ColumnExists<SchemaApiDemoTable>(x => x.AddedName)}");
            db.DropColumn<SchemaApiDemoTable>(x => x.AddedName);
            Console.WriteLine($"added_name exists after drop: {db.ColumnExists<SchemaApiDemoTable>(x => x.AddedName)}");
        }
        finally
        {
            if (tableCreated)
            {
                db.DropTable<SchemaApiDemoTable>();
                Console.WriteLine("\nTemporary demo table dropped; the knowledge-base schema is unchanged.");
            }
        }
    }
}
