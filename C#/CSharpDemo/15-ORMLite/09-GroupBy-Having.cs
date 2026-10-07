using ServiceStack.OrmLite;

namespace CSharpDemo.OrmLiteDemo;

public sealed class ArticleCountByAuthor
{
    public byte[] AuthorId { get; set; } = null!;
    public long TotalArticles { get; set; }
}

public sealed class TagCountByArticle
{
    public byte[] ArticleId { get; set; } = null!;
    public long TotalTags { get; set; }
}

public sealed class CommentCountByUser
{
    public byte[] UserId { get; set; } = null!;
    public long TotalComments { get; set; }
}

public static class GroupByHavingDemo
{
    public static void Run(System.Data.IDbConnection db)
    {
        Console.WriteLine("\n=== GROUP BY AND HAVING DEMO ===");

        Console.WriteLine("\n--- Article count by author ---");
        SqlExpression<Article> articleCountQuery = db
            .From<Article>()
            .Select(article => new
            {
                article.AuthorId,
                TotalArticles = Sql.Count("*")
            })
            .GroupBy(article => article.AuthorId);

        List<ArticleCountByAuthor> articleCounts =
            db.Select<ArticleCountByAuthor>(articleCountQuery);

        if (articleCounts.Count == 0)
        {
            Console.WriteLine("No articles found.");
        }
        else
        {
            foreach (ArticleCountByAuthor item in articleCounts)
            {
                Console.WriteLine(
                    $"Author {Convert.ToHexString(item.AuthorId)}: {item.TotalArticles} articles");
            }
        }

        Console.WriteLine("\n--- Tag count by article ---");
        SqlExpression<ArticleTag> tagCountQuery = db
            .From<ArticleTag>()
            .Select(articleTag => new
            {
                articleTag.ArticleId,
                TotalTags = Sql.Count("*")
            })
            .GroupBy(articleTag => articleTag.ArticleId);

        List<TagCountByArticle> tagCounts =
            db.Select<TagCountByArticle>(tagCountQuery);

        if (tagCounts.Count == 0)
        {
            Console.WriteLine("No article-tag links found.");
        }
        else
        {
            foreach (TagCountByArticle item in tagCounts)
            {
                Console.WriteLine(
                    $"Article {Convert.ToHexString(item.ArticleId)}: {item.TotalTags} tags");
            }
        }

        Console.WriteLine("\n--- Users with more than 10 comments (GROUP BY + HAVING) ---");
        SqlExpression<UserComment> commentCountQuery = db
            .From<UserComment>()
            .Select(comment => new
            {
                comment.UserId,
                TotalComments = Sql.Count("*")
            })
            .GroupBy(comment => comment.UserId)
            .Having("COUNT(*) > 10");

        List<CommentCountByUser> commentCounts =
            db.Select<CommentCountByUser>(commentCountQuery);

        if (commentCounts.Count == 0)
        {
            Console.WriteLine("No user has more than 10 comments.");
        }
        else
        {
            foreach (CommentCountByUser item in commentCounts)
            {
                Console.WriteLine(
                    $"User {Convert.ToHexString(item.UserId)}: {item.TotalComments} comments");
            }
        }
    }
}