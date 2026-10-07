using System;
using System.Data;

namespace CSharpDemo.DataTableDemo;

/// <summary>Represents the DataRowStatesDemo type.</summary>
public class DataRowStatesDemo
{
    /// <summary>Runs the demonstration.</summary>
    public void Run()
    {
        ConsoleHelper.Clear();

        Console.WriteLine("=== DATAROW STATES DEMO ===");

        DataTable articleVersions = CreateArticleVersionsTable();
        DataRow version = articleVersions.NewRow();

        // NewRow creates a row, but it is not part of the table yet.
        ShowState(
            "1. After NewRow()",
            version,
            articleVersions,
            "Detached: the row exists, but has not been added to the table."
        );

        version["version_id"] = Guid.Parse("11111111-1111-1111-1111-111111111111");
        version["article_id"] = Guid.Parse("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA");
        version["version_number"] = 1;
        version["title"] = "My Awesome SQL Guide";
        version["status_id"] = 1;

        // Filling in values does not attach a detached row to its table.
        ShowState(
            "2. After setting the row values",
            version,
            articleVersions,
            "Still Detached: the row has values, but is not in the table."
        );

        articleVersions.Rows.Add(version);

        // A newly added row is pending; it has not been accepted as the baseline yet.
        ShowState(
            "3. After Rows.Add()",
            version,
            articleVersions,
            "Added: the row is in the table and has not been accepted yet."
        );

        // AcceptChanges accepts all pending changes in this table.
        // The added row becomes the unchanged baseline.
        articleVersions.AcceptChanges();
        ShowState(
            "4. After AcceptChanges()",
            version,
            articleVersions,
            "Unchanged: the current values are now the accepted baseline."
        );

        version["title"] = "My Updated SQL Guide";
        version["status_id"] = 2;

        // Editing an unchanged row makes it Modified.
        ShowState(
            "5. After changing the title and status",
            version,
            articleVersions,
            "Modified: the row differs from its last accepted values."
        );

        // AcceptChanges accepts the edits and establishes a new baseline.
        articleVersions.AcceptChanges();
        ShowState(
            "6. After AcceptChanges() again",
            version,
            articleVersions,
            "Unchanged: the edited values are now the accepted baseline."
        );

        version.Delete();

        // A deleted row remains in the table until changes are accepted.
        ShowState(
            "7. After Delete()",
            version,
            articleVersions,
            "Deleted: marked for removal, but still in the table for now."
        );
        Console.WriteLine(
            $"Original title is still available: " +
            $"{version["title", DataRowVersion.Original]}"
        );

        // AcceptChanges permanently applies the deletion and removes the row.
        // The row object still exists, but it is now detached from the table.
        articleVersions.AcceptChanges();
        ShowState(
            "8. After AcceptChanges() on the deleted row",
            version,
            articleVersions,
            "Detached: the deleted row has been removed from the table."
        );

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey(true);
    }

    private DataTable CreateArticleVersionsTable()
    {
        DataTable table = new DataTable("ARTICLE_VERSION");

        DataColumn versionIdColumn = table.Columns.Add("version_id", typeof(Guid));
        table.Columns.Add("article_id", typeof(Guid));
        table.Columns.Add("version_number", typeof(int));
        table.Columns.Add("title", typeof(string));
        table.Columns.Add("status_id", typeof(int));

        table.PrimaryKey = new[] { versionIdColumn };

        return table;
    }

    private void ShowState(
        string step,
        DataRow row,
        DataTable table,
        string meaning)
    {
        Console.WriteLine($"\n{step}");
        Console.WriteLine($"RowState: {row.RowState}");
        Console.WriteLine($"Rows in table: {table.Rows.Count}");
        Console.WriteLine(meaning);
    }
}
