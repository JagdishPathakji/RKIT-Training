using ServiceStack.OrmLite;

namespace CSharpDemo.OrmLiteDemo;

/// <summary>Holds article, title, author, and creation data from a join query.</summary>
public sealed class PublishedArticleJoinResult
{
    /// <summary>Gets or sets the article id value.</summary>
    public byte[] ArticleId { get; set; } = null!;
    /// <summary>Gets or sets the title value.</summary>
    public string Title { get; set; } = string.Empty;
    /// <summary>Gets or sets the username value.</summary>
    public string Username { get; set; } = string.Empty;
    /// <summary>Gets or sets the created at value.</summary>
    public DateTime? CreatedAt { get; set; }
}

/// <summary>Holds an article ID and its optional joined category name.</summary>
public sealed class ArticleCategoryJoinResult
{
    /// <summary>Gets or sets the article id value.</summary>
    public byte[] ArticleId { get; set; } = null!;
    /// <summary>Gets or sets the category name value.</summary>
    public string? CategoryName { get; set; }
}

/// <summary>Holds a role name and its optional linked user's ID.</summary>
public sealed class RoleUserJoinResult
{
    /// <summary>Gets or sets the role name value.</summary>
    public string RoleName { get; set; } = string.Empty;
    /// <summary>Gets or sets the user id value.</summary>
    public byte[]? UserId { get; set; }
}

/// <summary>Holds an article ID and its joined tag name.</summary>
public sealed class ArticleTagJoinResult
{
    /// <summary>Gets or sets the article id value.</summary>
    public byte[] ArticleId { get; set; } = null!;
    /// <summary>Gets or sets the tag name value.</summary>
    public string TagName { get; set; } = string.Empty;
}

/// <summary>Demonstrates inner, left, and right joins across knowledge-base tables.</summary>
public static class JoinDemo
{
    /// <summary>Runs the demonstration.</summary>
    /// <param name="db">An open connection to the knowledge_base database.</param>
    public static void Run(System.Data.IDbConnection db)
    {
        Console.WriteLine("\n=== ORMLITE JOIN DEMO ===");

        Console.WriteLine("\n--- INNER JOIN: published articles and author names ---");
        SqlExpression<Article> publishedArticlesQuery = db
            .From<Article>()
            .Join<Article, User>(
                (article, user) => article.AuthorId == user.UserId)
            .Join<Article, ArticleVersion>(
                (article, version) =>
                    article.CurrentPublishedVersionId == version.VersionId)
            .Select<Article, User, ArticleVersion>(
                (article, user, version) => new
                {
                    article.ArticleId,
                    version.Title,
                    user.Username,
                    article.CreatedAt
                });

        List<PublishedArticleJoinResult> publishedArticles =
            db.Select<PublishedArticleJoinResult>(publishedArticlesQuery);

        if (publishedArticles.Count == 0)
        {
            Console.WriteLine("No published articles found.");
        }
        else
        {
            foreach (PublishedArticleJoinResult item in publishedArticles)
            {
                Console.WriteLine(
                    $"{Convert.ToHexString(item.ArticleId)} | {item.Title} | " +
                    $"Author: {item.Username} | Created: {item.CreatedAt}");
            }
        }

        Console.WriteLine("\n--- LEFT JOIN: articles and their categories ---");
        SqlExpression<Article> articleCategoriesQuery = db
            .From<Article>()
            .LeftJoin<Article, ArticleCategory>(
                (article, articleCategory) =>
                    article.ArticleId == articleCategory.ArticleId)
            .LeftJoin<ArticleCategory, Category>(
                (articleCategory, category) =>
                    articleCategory.CategoryId == category.CategoryId)
            .Select<Article, Category>(
                (article, category) => new
                {
                    article.ArticleId,
                    CategoryName = category.Name
                });

        List<ArticleCategoryJoinResult> articleCategories =
            db.Select<ArticleCategoryJoinResult>(articleCategoriesQuery);

        if (articleCategories.Count == 0)
        {
            Console.WriteLine("No articles found.");
        }
        else
        {
            foreach (ArticleCategoryJoinResult item in articleCategories)
            {
                Console.WriteLine(
                    $"{Convert.ToHexString(item.ArticleId)} | " +
                    $"{item.CategoryName ?? "Uncategorized"}");
            }
        }

        Console.WriteLine("\n--- RIGHT JOIN: every role, including roles with no users ---");
        SqlExpression<UserRole> rolesAndUsersQuery = db
            .From<UserRole>()
            .RightJoin<UserRole, Role>(
                (userRole, role) => userRole.RoleId == role.RoleId)
            .Select<UserRole, Role>(
                (userRole, role) => new
                {
                    role.RoleName,
                    userRole.UserId
                });

        List<RoleUserJoinResult> rolesAndUsers =
            db.Select<RoleUserJoinResult>(rolesAndUsersQuery);

        if (rolesAndUsers.Count == 0)
        {
            Console.WriteLine("No roles found.");
        }
        else
        {
            foreach (RoleUserJoinResult item in rolesAndUsers)
            {
                string userId = item.UserId is null
                    ? "No user assigned"
                    : Convert.ToHexString(item.UserId);

                Console.WriteLine($"{item.RoleName} | {userId}");
            }
        }

        // Console.WriteLine("\n--- MULTIPLE JOIN: article IDs and their tag names ---");
        // SqlExpression<Article> articleTagsQuery = db
        //     .From<Article>()
        //     .Join<Article, ArticleTag>(
        //         (article, articleTag) => article.ArticleId == articleTag.ArticleId)
        //     .Join<ArticleTag, Tag>(
        //         (articleTag, tag) => articleTag.TagId == tag.TagId)
        //     .Select<Article, ArticleTag, Tag>(
        //         (article, articleTag, tag) => new
        //         {
        //             article.ArticleId,
        //             TagName = tag.Name
        //         });

        // List<ArticleTagJoinResult> articleTags =
        //     db.Select<ArticleTagJoinResult>(articleTagsQuery);

        // if (articleTags.Count == 0)
        // {
        //     Console.WriteLine("No article-tag links found.");
        // }
        // else
        // {
        //     foreach (ArticleTagJoinResult item in articleTags)
        //     {
        //         Console.WriteLine(
        //             $"{Convert.ToHexString(item.ArticleId)} | {item.TagName}");
        //     }
        // }
    }
}
