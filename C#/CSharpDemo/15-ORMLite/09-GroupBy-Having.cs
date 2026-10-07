using ServiceStack.OrmLite;

namespace CSharpDemo.OrmLiteDemo;

/// <summary>Holds an author's ID and the number of articles attributed to them.</summary>
public sealed class ArticleCountByAuthor
{
    /// <summary>Gets or sets the author id value.</summary>
    public byte[] AuthorId { get; set; } = null!;
    /// <summary>Gets or sets the total articles value.</summary>
    public long TotalArticles { get; set; }
}

/// <summary>Holds an article's ID and the number of linked tags.</summary>
public sealed class TagCountByArticle
{
    /// <summary>Gets or sets the article id value.</summary>
    public byte[] ArticleId { get; set; } = null!;
    /// <summary>Gets or sets the total tags value.</summary>
    public long TotalTags { get; set; }
}

/// <summary>Holds a user's ID and the number of comments they wrote.</summary>
public sealed class CommentCountByUser
{
    /// <summary>Gets or sets the user id value.</summary>
    public byte[] UserId { get; set; } = null!;
    /// <summary>Gets or sets the total comments value.</summary>
    public long TotalComments { get; set; }
}

/// <summary>Demonstrates grouped counts and a HAVING filter.</summary>
public static class GroupByHavingDemo
{
    /// <summary>Runs the demonstration.</summary>
    /// <param name="db">An open connection to the knowledge_base database.</param>
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
