using System;
using System.Data;
using System.Linq;

namespace CSharpDemo.DataTableDemo;

public class DataTableDemo
{
    public void Run()
    {
        ConsoleHelper.Clear();

        Console.WriteLine("=== DATATABLE DEMO ===");

        DataTable articleVersions = CreateArticleVersionsTable();
        DataTable statuses = CreateStatusTable();

        AddStatuses(statuses);
        AddArticleVersions(articleVersions);

        DisplayArticleVersions(articleVersions);
        UpdateVersionStatus(articleVersions);
        DisplayArticleVersions(articleVersions);

        FindPublishedVersions(articleVersions);
        SortVersions(articleVersions);
        JoinVersionsWithStatuses(articleVersions, statuses);

        DeleteVersion(articleVersions);
        DisplayArticleVersions(articleVersions);

        AddVersionUsingDataRow(articleVersions);
        DisplayArticleVersions(articleVersions);

        ShowTableInformation(articleVersions);

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey(true);

        DataRowStatesDemo statesDemo = new DataRowStatesDemo();
        statesDemo.Run();

        DataTableToJson.Run();
    }

    // This demo uses the columns needed for these examples from ARTICLE_VERSION.
    // The database stores its BINARY(16) UUID values as Guid values in memory.
    private DataTable CreateArticleVersionsTable()
    {
        DataTable table = new DataTable("ARTICLE_VERSION");

        DataColumn versionIdColumn = table.Columns.Add("version_id", typeof(Guid));
        table.Columns.Add("article_id", typeof(Guid));

        DataColumn versionNumberColumn =
            table.Columns.Add("version_number", typeof(int));
        versionNumberColumn.Unique = true;

        table.Columns.Add("title", typeof(string));
        table.Columns.Add("status_id", typeof(int));

        table.PrimaryKey = new[] { versionIdColumn };

        Console.WriteLine("\nARTICLE_VERSION table created.");

        return table;
    }

    // These columns match the STATUS lookup table in the database schema.
    private DataTable CreateStatusTable()
    {
        DataTable table = new DataTable("STATUS");

        DataColumn statusIdColumn = table.Columns.Add("status_id", typeof(int));
        table.Columns.Add("status_name", typeof(string));

        table.PrimaryKey = new[] { statusIdColumn };

        Console.WriteLine("STATUS table created.");

        return table;
    }

    private void AddStatuses(DataTable table)
    {
        table.Rows.Add(1, "Draft");
        table.Rows.Add(2, "Pending Editor Review");
        table.Rows.Add(3, "Needs Improvement");
        table.Rows.Add(4, "Rejected");
        table.Rows.Add(5, "Published");

        Console.WriteLine("Status rows added.");
    }

    private void AddArticleVersions(DataTable table)
    {
        table.Rows.Add(
            Guid.Parse("BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB"),
            Guid.Parse("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA"),
            1,
            "My Awesome SQL Guide",
            1
        );

        table.Rows.Add(
            Guid.Parse("DDDDDDDD-DDDD-DDDD-DDDD-DDDDDDDDDDDD"),
            Guid.Parse("CCCCCCCC-CCCC-CCCC-CCCC-CCCCCCCCCCCC"),
            2,
            "Understanding Database Indexes",
            2
        );

        table.Rows.Add(
            Guid.Parse("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF"),
            Guid.Parse("EEEEEEEE-EEEE-EEEE-EEEE-EEEEEEEEEEEE"),
            3,
            "Getting Started with SQL JOINs",
            5
        );

        Console.WriteLine("Article version rows added.");
    }

    private void DisplayArticleVersions(DataTable table)
    {
        Console.WriteLine("\nArticle versions:");

        foreach (DataRow row in table.Rows)
        {
            Console.WriteLine(
                $"{row["version_number"]} | " +
                $"{row["title"]} | " +
                $"Status ID: {row["status_id"]}"
            );
        }
    }

    private void UpdateVersionStatus(DataTable table)
    {
        DataRow? row = table.Rows.Find(
            Guid.Parse("BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB")
        );

        if (row is not null)
        {
            row["status_id"] = 2;
            Console.WriteLine("\nArticle version status updated.");
        }
    }

    private void FindPublishedVersions(DataTable table)
    {
        DataRow[] rows = table.Select("status_id = 5");

        Console.WriteLine("\nPublished article versions:");

        foreach (DataRow row in rows)
        {
            Console.WriteLine(
                $"{row["version_number"]} | {row["title"]}"
            );
        }
    }

    private void SortVersions(DataTable table)
    {
        DataRow[] rows = table.Select("", "title ASC");

        Console.WriteLine("\nArticle versions sorted by title:");

        foreach (DataRow row in rows)
        {
            Console.WriteLine(row["title"]);
        }
    }

    // Join ARTICLE_VERSION.status_id to STATUS.status_id.
    private void JoinVersionsWithStatuses(
        DataTable articleVersions,
        DataTable statuses)
    {
        var joinedRows =
            from version in articleVersions.AsEnumerable()
            join status in statuses.AsEnumerable()
                on version.Field<int>("status_id")
                equals status.Field<int>("status_id")
            select new
            {
                VersionNumber = version.Field<int>("version_number"),
                Title = version.Field<string>("title"),
                StatusName = status.Field<string>("status_name")
            };

        Console.WriteLine("\nArticle versions with their statuses:");

        foreach (var row in joinedRows)
        {
            Console.WriteLine(
                $"{row.VersionNumber} | {row.Title} | {row.StatusName}"
            );
        }
    }
    
    // datatable to json

    private void DeleteVersion(DataTable table)
    {
        DataRow? row = table.Rows.Find(
            Guid.Parse("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF")
        );

        if (row is not null)
        {
            row.Delete();
            table.AcceptChanges();
            Console.WriteLine("\nArticle version deleted from the in-memory table.");
        }
    }

    private void AddVersionUsingDataRow(DataTable table)
    {
        DataRow row = table.NewRow();

        row["version_id"] =
            Guid.Parse("77777777-7777-7777-7777-777777777777");
        row["article_id"] =
            Guid.Parse("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA");
        row["version_number"] = 4;
        row["title"] = "My Awesome SQL Guide";
        row["status_id"] = 1;

        table.Rows.Add(row);

        Console.WriteLine("\nArticle version added to the in-memory table.");
    }

    private void ShowTableInformation(DataTable table)
    {
        Console.WriteLine("\nARTICLE_VERSION table information:");
        Console.WriteLine($"Table name: {table.TableName}");
        Console.WriteLine($"Columns: {table.Columns.Count}");
        Console.WriteLine($"Rows: {table.Rows.Count}");
    }
}