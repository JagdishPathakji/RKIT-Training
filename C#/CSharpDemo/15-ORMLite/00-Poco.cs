using ServiceStack.DataAnnotations;
namespace CSharpDemo.OrmLiteDemo;

/// <summary>Maps a knowledge-base user account to the user table.</summary>
[Alias("user")]
public class User
{
    /// <summary>Gets or sets the user id value.</summary>
    [PrimaryKey]
    [Alias("user_id")]
    [CustomField("BINARY(16)")]
    public byte[] UserId { get; set; } = null!;

    /// <summary>Gets or sets the username value.</summary>
    [Alias("username")]
    public string Username { get; set; } = string.Empty;

    /// <summary>Gets or sets the email value.</summary>
    [Alias("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the password hash value.</summary>
    [Alias("password_hash")]
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>Gets or sets the created at value.</summary>
    [Alias("created_at")]
    public DateTime? CreatedAt { get; set; }
}

/// <summary>Maps a user authorization role to the role table.</summary>
[Alias("role")]
public class Role
{
    /// <summary>Gets or sets the role id value.</summary>
    [PrimaryKey]
    [Alias("role_id")]
    public int RoleId { get; set; }

    /// <summary>Gets or sets the role name value.</summary>
    [Alias("role_name")]
    public string RoleName { get; set; } = string.Empty;
}

/// <summary>Maps a user's association with an authorization role.</summary>
[Alias("user_role")]
[CompositeKey(nameof(UserId), nameof(RoleId))]
public class UserRole
{
    /// <summary>Gets or sets the user id value.</summary>
    [Alias("user_id")]
    [CustomField("BINARY(16)")]
    public byte[] UserId { get; set; } = null!;

    /// <summary>Gets or sets the role id value.</summary>
    [Alias("role_id")]
    public int RoleId { get; set; }
}

/// <summary>Maps an article-version status lookup value.</summary>
[Alias("status")]
public class ArticleStatus
{
    /// <summary>Gets or sets the status id value.</summary>
    [PrimaryKey]
    [Alias("status_id")]
    public int StatusId { get; set; }

    /// <summary>Gets or sets the status name value.</summary>
    [Alias("status_name")]
    public string StatusName { get; set; } = string.Empty;
}

/// <summary>Maps a content-block type lookup value.</summary>
[Alias("block_type")]
public class BlockType
{
    /// <summary>Gets or sets the block type id value.</summary>
    [PrimaryKey]
    [Alias("block_type_id")]
    public int BlockTypeId { get; set; }

    /// <summary>Gets or sets the type name value.</summary>
    [Alias("type_name")]
    public string TypeName { get; set; } = string.Empty;
}

/// <summary>Maps an article category to the category table.</summary>
[Alias("category")]
public class Category
{
    /// <summary>Gets or sets the category id value.</summary>
    [PrimaryKey]
    [AutoIncrement]
    [Alias("category_id")]
    public int CategoryId { get; set; }

    /// <summary>Gets or sets the name value.</summary>
    [Alias("name")]
    public string Name { get; set; } = string.Empty;
}

/// <summary>Maps an article tag to the tag table.</summary>
[Alias("tag")]
public class Tag
{
    /// <summary>Gets or sets the tag id value.</summary>
    [PrimaryKey]
    [AutoIncrement]
    [Alias("tag_id")]
    public int TagId { get; set; }

    /// <summary>Gets or sets the name value.</summary>
    [Alias("name")]
    public string Name { get; set; } = string.Empty;
}

/// <summary>Maps an article's identity, author, and current published version.</summary>
[Alias("article")]
public class Article
{
    /// <summary>Gets or sets the article id value.</summary>
    [PrimaryKey]
    [Alias("article_id")]
    [CustomField("BINARY(16)")]
    public byte[] ArticleId { get; set; } = null!;

    /// <summary>Gets or sets the author id value.</summary>
    [Alias("author_id")]
    [CustomField("BINARY(16)")]
    public byte[] AuthorId { get; set; } = null!;

    /// <summary>Gets or sets the current published version id value.</summary>
    [Alias("current_published_version_id")]
    [CustomField("BINARY(16)")]
    public byte[]? CurrentPublishedVersionId { get; set; }

    /// <summary>Gets or sets the created at value.</summary>
    [Alias("created_at")]
    public DateTime? CreatedAt { get; set; }
}

/// <summary>Maps a versioned article title, status, author, and timestamp.</summary>
[Alias("article_version")]
public class ArticleVersion
{
    /// <summary>Gets or sets the version id value.</summary>
    [PrimaryKey]
    [Alias("version_id")]
    [CustomField("BINARY(16)")]
    public byte[] VersionId { get; set; } = null!;

    /// <summary>Gets or sets the article id value.</summary>
    [Alias("article_id")]
    [CustomField("BINARY(16)")]
    public byte[] ArticleId { get; set; } = null!;

    /// <summary>Gets or sets the version number value.</summary>
    [Alias("version_number")]
    public int VersionNumber { get; set; }

    /// <summary>Gets or sets the title value.</summary>
    [Alias("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>Gets or sets the status id value.</summary>
    [Alias("status_id")]
    public int StatusId { get; set; }

    /// <summary>Gets or sets the created by value.</summary>
    [Alias("created_by")]
    [CustomField("BINARY(16)")]
    public byte[] CreatedBy { get; set; } = null!;

    /// <summary>Gets or sets the created at value.</summary>
    [Alias("created_at")]
    public DateTime? CreatedAt { get; set; }
}

/// <summary>Maps an ordered content block belonging to an article version.</summary>
[Alias("content_block")]
public class ContentBlock
{
    /// <summary>Gets or sets the block id value.</summary>
    [PrimaryKey]
    [Alias("block_id")]
    [CustomField("BINARY(16)")]
    public byte[] BlockId { get; set; } = null!;

    /// <summary>Gets or sets the version id value.</summary>
    [Alias("version_id")]
    [CustomField("BINARY(16)")]
    public byte[] VersionId { get; set; } = null!;

    /// <summary>Gets or sets the block type id value.</summary>
    [Alias("block_type_id")]
    public int BlockTypeId { get; set; }

    /// <summary>Gets or sets the content data value.</summary>
    [Alias("content_data")]
    public string? ContentData { get; set; }

    /// <summary>Gets or sets the sequence order value.</summary>
    [Alias("sequence_order")]
    public int SequenceOrder { get; set; }
}

/// <summary>Maps the many-to-many link between an article and a category.</summary>
[Alias("article_category")]
[CompositeKey(nameof(ArticleId), nameof(CategoryId))]
public class ArticleCategory
{
    /// <summary>Gets or sets the article id value.</summary>
    [Alias("article_id")]
    [CustomField("BINARY(16)")]
    public byte[] ArticleId { get; set; } = null!;

    /// <summary>Gets or sets the category id value.</summary>
    [Alias("category_id")]
    public int CategoryId { get; set; }
}

/// <summary>Maps the many-to-many link between an article and a tag.</summary>
[Alias("article_tag")]
[CompositeKey(nameof(ArticleId), nameof(TagId))]
public class ArticleTag
{
    /// <summary>Gets or sets the article id value.</summary>
    [Alias("article_id")]
    [CustomField("BINARY(16)")]
    public byte[] ArticleId { get; set; } = null!;

    /// <summary>Gets or sets the tag id value.</summary>
    [Alias("tag_id")]
    public int TagId { get; set; }
}

/// <summary>Maps an editor's decision and feedback for an article version.</summary>
[Alias("editorial_review")]
public class EditorialReview
{
    /// <summary>Gets or sets the review id value.</summary>
    [PrimaryKey]
    [Alias("review_id")]
    [CustomField("BINARY(16)")]
    public byte[] ReviewId { get; set; } = null!;

    /// <summary>Gets or sets the version id value.</summary>
    [Alias("version_id")]
    [CustomField("BINARY(16)")]
    public byte[] VersionId { get; set; } = null!;

    /// <summary>Gets or sets the editor id value.</summary>
    [Alias("editor_id")]
    [CustomField("BINARY(16)")]
    public byte[] EditorId { get; set; } = null!;

    /// <summary>Gets or sets the decision value.</summary>
    [Alias("decision")]
    public string Decision { get; set; } = string.Empty;

    /// <summary>Gets or sets the feedback value.</summary>
    [Alias("feedback")]
    public string Feedback { get; set; } = string.Empty;

    /// <summary>Gets or sets the reviewed at value.</summary>
    [Alias("reviewed_at")]
    public DateTime? ReviewedAt { get; set; }
}

/// <summary>Maps a user's rating for an article.</summary>
[Alias("user_rating")]
[CompositeKey(nameof(ArticleId), nameof(UserId))]
public class UserRating
{
    /// <summary>Gets or sets the article id value.</summary>
    [Alias("article_id")]
    [CustomField("BINARY(16)")]
    public byte[] ArticleId { get; set; } = null!;

    /// <summary>Gets or sets the user id value.</summary>
    [Alias("user_id")]
    [CustomField("BINARY(16)")]
    public byte[] UserId { get; set; } = null!;

    /// <summary>Gets or sets the rating score.</summary>
    [Alias("rating_value")]
    public decimal RatingValue { get; set; }

    /// <summary>Gets or sets the updated at value.</summary>
    [Alias("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>Maps a user's comment on an article.</summary>
[Alias("user_comment")]
public class UserComment
{
    /// <summary>Gets or sets the comment id value.</summary>
    [PrimaryKey]
    [Alias("comment_id")]
    [CustomField("BINARY(16)")]
    public byte[] CommentId { get; set; } = null!;

    /// <summary>Gets or sets the article id value.</summary>
    [Alias("article_id")]
    [CustomField("BINARY(16)")]
    public byte[] ArticleId { get; set; } = null!;

    /// <summary>Gets or sets the user id value.</summary>
    [Alias("user_id")]
    [CustomField("BINARY(16)")]
    public byte[] UserId { get; set; } = null!;

    /// <summary>Gets or sets the comment text value.</summary>
    [Alias("comment_text")]
    public string CommentText { get; set; } = string.Empty;

    /// <summary>Gets or sets the created at value.</summary>
    [Alias("created_at")]
    public DateTime? CreatedAt { get; set; }
}
