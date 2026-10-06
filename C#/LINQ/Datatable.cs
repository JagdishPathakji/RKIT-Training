using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;

public class Article
{
    public int ArticleId { get; set; }
    public string Title { get; set; }
    public int StatusId { get; set; }
    public int AuthorId { get; set; }
}

public class Program
{
    public static void Main()
    {
        DataTable dataTable = BuildSampleDataTable();

        Section1_AsEnumerableBasics(dataTable);
        Section2_FieldAndDBNull(dataTable);
        Section3_WhereAndSelect(dataTable);
        Section4_ProjectToTypedClass(dataTable);
        Section5_OrderBy(dataTable);
        Section6_CopyBackToDataTable(dataTable);
    }

    // Build a sample DataTable manually, the way it might come back from a DB query
    static DataTable BuildSampleDataTable()
    {
        DataTable table = new DataTable("Articles");

        table.Columns.Add("ArticleId", typeof(int));
        table.Columns.Add("Title", typeof(string));
        table.Columns.Add("StatusId", typeof(int));
        table.Columns.Add("AuthorId", typeof(int));
        table.Columns.Add("Summary", typeof(string));   // this column will contain DBNull for some rows

        table.Rows.Add(1, "Intro to SQL", 5, 101, "Learn SQL basics");
        table.Rows.Add(2, "Advanced LINQ", 5, 102, DBNull.Value);   // no summary provided
        table.Rows.Add(3, "Draft Notes", 1, 101, "Not ready yet");
        table.Rows.Add(4, "SQL Performance", 5, 101, "Tuning SQL queries");
        table.Rows.Add(5, "C# Basics", 5, 103, DBNull.Value);

        return table;
    }


    // ================================================================
    // SECTION 1 -- AsEnumerable() basics -- required before any LINQ works
    // ================================================================
    static void Section1_AsEnumerableBasics(DataTable dataTable)
    {
        Console.WriteLine("========== SECTION 1 -- AsEnumerable() basics ==========");

        // dataTable.Rows.Where(...) would NOT compile -- DataRowCollection is not IEnumerable<T>.
        // AsEnumerable() converts it into IEnumerable<DataRow>, unlocking every LINQ method.
        IEnumerable<DataRow> rows = dataTable.AsEnumerable();

        int totalRows = rows.Count();
        Console.WriteLine($"Total rows via AsEnumerable().Count(): {totalRows}");
        Console.WriteLine();
    }


    // ================================================================
    // SECTION 2 -- Field<T>() and handling DBNull
    // ================================================================
    static void Section2_FieldAndDBNull(DataTable dataTable)
    {
        Console.WriteLine("========== SECTION 2 -- Field<T>() and DBNull ==========");

        foreach (DataRow row in dataTable.AsEnumerable())
        {
            // Field<int>() is safe here because ArticleId never contains DBNull in this table
            int articleId = row.Field<int>("ArticleId");

            // Field<string>() naturally returns C# null when the cell is DBNull -- no crash
            string summary = row.Field<string>("Summary");

            // If summary is not null, use summary; otherwise use "(no summary)".
            string summaryDisplay = summary ?? "(no summary)";
            Console.WriteLine($"  ArticleId {articleId}: {summaryDisplay}");
        }
        Console.WriteLine();

        // If StatusId column COULD contain DBNull, you must read it as int? (nullable), not int:
        //   int? statusId = row.Field<int?>("StatusId");
        // Reading a nullable-capable column with Field<int>() (non-nullable) throws at runtime
        // if it actually hits a DBNull value.
    }


    // ================================================================
    // SECTION 3 -- Where() and Select() directly on DataRows
    // ================================================================
    static void Section3_WhereAndSelect(DataTable dataTable)
    {
        Console.WriteLine("========== SECTION 3 -- Where() / Select() on DataRows ==========");

        // Filter: published articles only (StatusId == 5)
        var publishedTitles = dataTable.AsEnumerable()
            .Where(row => row.Field<int>("StatusId") == 5)
            .Select(row => row.Field<string>("Title"))
            .ToList();

        Console.WriteLine("Published article titles:");
        foreach (var title in publishedTitles)
            Console.WriteLine($"  {title}");
        Console.WriteLine();

        // Filter with null-safe check on Summary
        var withSummary = dataTable.AsEnumerable()
            .Where(row => row.Field<string>("Summary") != null)
            .Select(row => row.Field<string>("Title"))
            .ToList();

        Console.WriteLine("Articles that HAVE a summary:");
        foreach (var title in withSummary)
            Console.WriteLine($"  {title}");
        Console.WriteLine();
    }


    // ================================================================
    // SECTION 4 -- projecting DataRows into a proper typed class
    // ================================================================
    static void Section4_ProjectToTypedClass(DataTable dataTable)
    {
        Console.WriteLine("========== SECTION 4 -- Project DataRows into Article objects ==========");

        // This is the recommended pattern: convert once, work with typed objects afterward
        List<Article> articles = dataTable.AsEnumerable()
            .Select(row => new Article
            {
                ArticleId = row.Field<int>("ArticleId"),
                Title = row.Field<string>("Title"),
                StatusId = row.Field<int>("StatusId"),
                AuthorId = row.Field<int>("AuthorId")
            })
            .ToList();

        // From here on, this is a PLAIN List<Article> 
        var publishedByAuthor101 = articles
            .Where(a => a.StatusId == 5 && a.AuthorId == 101)
            .Select(a => a.Title)
            .ToList();

        Console.WriteLine("Published articles by AuthorId 101 (using normal List<Article> LINQ now):");
        foreach (var title in publishedByAuthor101)
            Console.WriteLine($"  {title}");
        Console.WriteLine();
    }


    // ================================================================
    // SECTION 5 -- OrderBy() and GroupBy() on DataRows
    // ================================================================
    static void Section5_OrderBy(DataTable dataTable)
    {
        Console.WriteLine("========== SECTION 5 -- OrderBy() / GroupBy() on DataRows ==========");

        // Sort by Title
        var sortedTitles = dataTable.AsEnumerable()
            .OrderBy(row => row.Field<string>("Title"))
            .Select(row => row.Field<string>("Title"))
            .ToList();

        Console.WriteLine("Titles sorted alphabetically:");
        foreach (var title in sortedTitles)
            Console.WriteLine($"  {title}");
        Console.WriteLine();
    }


    // ================================================================
    // SECTION 6 -- going back to a DataTable with CopyToDataTable()
    // ================================================================
    static void Section6_CopyBackToDataTable(DataTable dataTable)
    {
        Console.WriteLine("========== SECTION 6 -- CopyToDataTable() ==========");
        // Case A -- query HAS matching rows -- CopyToDataTable() works fine
        var publishedRows = dataTable.AsEnumerable().Where(row => row.Field<int>("StatusId") == 5);

        DataTable publishedTable = publishedRows.Any()
            ? publishedRows.CopyToDataTable()
            : dataTable.Clone();   // fallback: schema only, zero rows

        Console.WriteLine($"Case A -- publishedTable row count: {publishedTable.Rows.Count}");

        // Case B -- query has ZERO matching rows -- CopyToDataTable() would THROW here,
        // so we guard with .Any() first, same pattern as above
        var noMatchRows = dataTable.AsEnumerable().Where(row => row.Field<int>("StatusId") == 999);

        DataTable emptyResultTable = noMatchRows.Any()
            ? noMatchRows.CopyToDataTable()
            : dataTable.Clone();

        Console.WriteLine($"Case B -- emptyResultTable row count: {emptyResultTable.Rows.Count} (schema preserved, zero rows, no crash)");
        Console.WriteLine();
    }
}