using ServiceStack.OrmLite;

namespace CSharpDemo.OrmLiteDemo;

public sealed class PublishedArticleJoinResult
{
    public byte[] ArticleId { get; set; } = null!;
    public string Title { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
}

public sealed class ArticleCategoryJoinResult
{
    public byte[] ArticleId { get; set; } = null!;
    public string? CategoryName { get; set; }
}

public sealed class RoleUserJoinResult
{
    public string RoleName { get; set; } = string.Empty;
    public byte[]? UserId { get; set; }
}

public sealed class ArticleTagJoinResult
{
    public byte[] ArticleId { get; set; } = null!;
    public string TagName { get; set; } = string.Empty;
}

public static class JoinDemo
{
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