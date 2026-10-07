using ServiceStack.DataAnnotations;
namespace CSharpDemo.OrmLiteDemo;

[Alias("user")]
public class User
{
    [PrimaryKey]
    [Alias("user_id")]
    [CustomField("BINARY(16)")]
    public byte[] UserId { get; set; } = null!;

    [Alias("username")]
    public string Username { get; set; } = string.Empty;

    [Alias("email")]
    public string Email { get; set; } = string.Empty;

    [Alias("password_hash")]
    public string PasswordHash { get; set; } = string.Empty;

    [Alias("created_at")]
    public DateTime? CreatedAt { get; set; }
}

[Alias("role")]
public class Role
{
    [PrimaryKey]
    [Alias("role_id")]
    public int RoleId { get; set; }

    [Alias("role_name")]
    public string RoleName { get; set; } = string.Empty;
}

[Alias("user_role")]
[CompositeKey(nameof(UserId), nameof(RoleId))]
public class UserRole
{
    [Alias("user_id")]
    [CustomField("BINARY(16)")]
    public byte[] UserId { get; set; } = null!;

    [Alias("role_id")]
    public int RoleId { get; set; }
}

[Alias("status")]
public class ArticleStatus
{
    [PrimaryKey]
    [Alias("status_id")]
    public int StatusId { get; set; }

    [Alias("status_name")]
    public string StatusName { get; set; } = string.Empty;
}

[Alias("block_type")]
public class BlockType
{
    [PrimaryKey]
    [Alias("block_type_id")]
    public int BlockTypeId { get; set; }

    [Alias("type_name")]
    public string TypeName { get; set; } = string.Empty;
}

[Alias("category")]
public class Category
{
    [PrimaryKey]
    [AutoIncrement]
    [Alias("category_id")]
    public int CategoryId { get; set; }

    [Alias("name")]
    public string Name { get; set; } = string.Empty;
}

[Alias("tag")]
public class Tag
{
    [PrimaryKey]
    [AutoIncrement]
    [Alias("tag_id")]
    public int TagId { get; set; }

    [Alias("name")]
    public string Name { get; set; } = string.Empty;
}

[Alias("article")]
public class Article
{
    [PrimaryKey]
    [Alias("article_id")]
    [CustomField("BINARY(16)")]
    public byte[] ArticleId { get; set; } = null!;

    [Alias("author_id")]
    [CustomField("BINARY(16)")]
    public byte[] AuthorId { get; set; } = null!;

    [Alias("current_published_version_id")]
    [CustomField("BINARY(16)")]
    public byte[]? CurrentPublishedVersionId { get; set; }

    [Alias("created_at")]
    public DateTime? CreatedAt { get; set; }
}

[Alias("article_version")]
public class ArticleVersion
{
    [PrimaryKey]
    [Alias("version_id")]
    [CustomField("BINARY(16)")]
    public byte[] VersionId { get; set; } = null!;

    [Alias("article_id")]
    [CustomField("BINARY(16)")]
    public byte[] ArticleId { get; set; } = null!;

    [Alias("version_number")]
    public int VersionNumber { get; set; }

    [Alias("title")]
    public string Title { get; set; } = string.Empty;

    [Alias("status_id")]
    public int StatusId { get; set; }

    [Alias("created_by")]
    [CustomField("BINARY(16)")]
    public byte[] CreatedBy { get; set; } = null!;

    [Alias("created_at")]
    public DateTime? CreatedAt { get; set; }
}

[Alias("content_block")]
public class ContentBlock
{
    [PrimaryKey]
    [Alias("block_id")]
    [CustomField("BINARY(16)")]
    public byte[] BlockId { get; set; } = null!;

    [Alias("version_id")]
    [CustomField("BINARY(16)")]
    public byte[] VersionId { get; set; } = null!;

    [Alias("block_type_id")]
    public int BlockTypeId { get; set; }

    [Alias("content_data")]
    public string? ContentData { get; set; }

    [Alias("sequence_order")]
    public int SequenceOrder { get; set; }
}

[Alias("article_category")]
[CompositeKey(nameof(ArticleId), nameof(CategoryId))]
public class ArticleCategory
{
    [Alias("article_id")]
    [CustomField("BINARY(16)")]
    public byte[] ArticleId { get; set; } = null!;

    [Alias("category_id")]
    public int CategoryId { get; set; }
}

[Alias("article_tag")]
[CompositeKey(nameof(ArticleId), nameof(TagId))]
public class ArticleTag
{
    [Alias("article_id")]
    [CustomField("BINARY(16)")]
    public byte[] ArticleId { get; set; } = null!;

    [Alias("tag_id")]
    public int TagId { get; set; }
}

[Alias("editorial_review")]
public class EditorialReview
{
    [PrimaryKey]
    [Alias("review_id")]
    [CustomField("BINARY(16)")]
    public byte[] ReviewId { get; set; } = null!;

    [Alias("version_id")]
    [CustomField("BINARY(16)")]
    public byte[] VersionId { get; set; } = null!;

    [Alias("editor_id")]
    [CustomField("BINARY(16)")]
    public byte[] EditorId { get; set; } = null!;

    [Alias("decision")]
    public string Decision { get; set; } = string.Empty;

    [Alias("feedback")]
    public string Feedback { get; set; } = string.Empty;

    [Alias("reviewed_at")]
    public DateTime? ReviewedAt { get; set; }
}

[Alias("user_rating")]
[CompositeKey(nameof(ArticleId), nameof(UserId))]
public class UserRating
{
    [Alias("article_id")]
    [CustomField("BINARY(16)")]
    public byte[] ArticleId { get; set; } = null!;

    [Alias("user_id")]
    [CustomField("BINARY(16)")]
    public byte[] UserId { get; set; } = null!;

    [Alias("rating_value")]
    public decimal RatingValue { get; set; }

    [Alias("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}

[Alias("user_comment")]
public class UserComment
{
    [PrimaryKey]
    [Alias("comment_id")]
    [CustomField("BINARY(16)")]
    public byte[] CommentId { get; set; } = null!;

    [Alias("article_id")]
    [CustomField("BINARY(16)")]
    public byte[] ArticleId { get; set; } = null!;

    [Alias("user_id")]
    [CustomField("BINARY(16)")]
    public byte[] UserId { get; set; } = null!;

    [Alias("comment_text")]
    public string CommentText { get; set; } = string.Empty;

    [Alias("created_at")]
    public DateTime? CreatedAt { get; set; }
}