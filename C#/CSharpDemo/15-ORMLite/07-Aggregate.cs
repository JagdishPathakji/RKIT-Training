using ServiceStack.OrmLite;

namespace CSharpDemo.OrmLiteDemo;

public static class AggregateDemo
{
    public static void Run(System.Data.IDbConnection db)
    {
        Console.WriteLine("\n=== ORMLITE AGGREGATE DEMO: ARTICLE CONTENT SIZE ===");

        var countQuery = db
            .From<ContentBlock>()
            .Select(block => Sql.Count("*"));
        long blockCount = db.Scalar<long>(countQuery);
        Console.WriteLine($"Number of content blocks: {blockCount}");

        var sumQuery = db
            .From<ContentBlock>()
            // ! only suppresses the C# nullable warning; it does not add a runtime null check.
            .Select(block => Sql.Sum(block.ContentData!.Length));
        long? totalBytes = db.Scalar<long?>(sumQuery);
        Console.WriteLine($"Total content size: {totalBytes ?? 0} bytes");

        var averageQuery = db
            .From<ContentBlock>()
            .Select(block => Sql.Avg(block.ContentData!.Length));
        decimal? averageBytes = db.Scalar<decimal?>(averageQuery);
        Console.WriteLine(
            $"Average content block size: {averageBytes?.ToString("F2") ?? "no content blocks yet"} bytes");

        var minimumQuery = db
            .From<ContentBlock>()
            .Select(block => Sql.Min(block.ContentData!.Length));
        int? minimumBytes = db.Scalar<int?>(minimumQuery);
        Console.WriteLine(
            $"Smallest content block: {minimumBytes?.ToString() ?? "no content blocks yet"} bytes");

        var maximumQuery = db
            .From<ContentBlock>()
            .Select(block => Sql.Max(block.ContentData!.Length));
        int? maximumBytes = db.Scalar<int?>(maximumQuery);
        Console.WriteLine(
            $"Largest content block: {maximumBytes?.ToString() ?? "no content blocks yet"} bytes");

        var distinctVersionCountQuery = db
            .From<ContentBlock>()
            .Select(block => Sql.CountDistinct(block.VersionId));
        long versionCount = db.Scalar<long>(distinctVersionCountQuery);
        Console.WriteLine($"Article versions containing content: {versionCount}");
    }
}