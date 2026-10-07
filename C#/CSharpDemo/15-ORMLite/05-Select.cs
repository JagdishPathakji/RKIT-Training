using ServiceStack.OrmLite;

namespace CSharpDemo.OrmLiteDemo;

public sealed class CategorySelectResult
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
}

public static class SelectDemo
{
    public static void ShowAllTables(System.Data.IDbConnection db)
    {
        Console.WriteLine("\n=== ALL KNOWLEDGE_BASE TABLES ===");
        List<User> users = db.Select<User>();
        Console.WriteLine($"\n--- user ({users.Count} rows) ---");
        foreach (User user in users)
        {
            Console.WriteLine(
                $"user_id={Convert.ToHexString(user.UserId)}, username={user.Username}, " +
                $"email={user.Email}, created_at={user.CreatedAt}");
        }

        List<Role> roles = db.Select<Role>();
        Console.WriteLine($"\n--- role ({roles.Count} rows) ---");
        foreach (Role role in roles)
            Console.WriteLine($"role_id={role.RoleId}, role_name={role.RoleName}");

        List<UserRole> userRoles = db.Select<UserRole>();
        Console.WriteLine($"\n--- user_role ({userRoles.Count} rows) ---");
        foreach (UserRole userRole in userRoles)
            Console.WriteLine($"user_id={Convert.ToHexString(userRole.UserId)}, role_id={userRole.RoleId}");

        List<ArticleStatus> statuses = db.Select<ArticleStatus>();
        Console.WriteLine($"\n--- status ({statuses.Count} rows) ---");
        foreach (ArticleStatus status in statuses)
            Console.WriteLine($"status_id={status.StatusId}, status_name={status.StatusName}");

        List<Category> categories = db.Select<Category>();
        Console.WriteLine($"\n--- category ({categories.Count} rows) ---");
        foreach (Category category in categories)
            Console.WriteLine($"category_id={category.CategoryId}, name={category.Name}");

        List<Tag> tags = db.Select<Tag>();
        Console.WriteLine($"\n--- tag ({tags.Count} rows) ---");
        foreach (Tag tag in tags)
            Console.WriteLine($"tag_id={tag.TagId}, name={tag.Name}");

        List<Article> articles = db.Select<Article>();
        Console.WriteLine($"\n--- article ({articles.Count} rows) ---");
        foreach (Article article in articles)
            Console.WriteLine(
                $"article_id={Convert.ToHexString(article.ArticleId)}, " +
                $"author_id={Convert.ToHexString(article.AuthorId)}, " +
                $"current_published_version_id={FormatId(article.CurrentPublishedVersionId)}, " +
                $"created_at={article.CreatedAt}");

        List<ArticleVersion> versions = db.Select<ArticleVersion>();
        Console.WriteLine($"\n--- article_version ({versions.Count} rows) ---");
        foreach (ArticleVersion version in versions)
            Console.WriteLine(
                $"version_id={Convert.ToHexString(version.VersionId)}, " +
                $"article_id={Convert.ToHexString(version.ArticleId)}, " +
                $"version_number={version.VersionNumber}, title={version.Title}, " +
                $"status_id={version.StatusId}, created_by={Convert.ToHexString(version.CreatedBy)}, " +
                $"created_at={version.CreatedAt}");

        List<ContentBlock> contentBlocks = db.Select<ContentBlock>();
        Console.WriteLine($"\n--- content_block ({contentBlocks.Count} rows) ---");
        foreach (ContentBlock block in contentBlocks)
            Console.WriteLine(
                $"block_id={Convert.ToHexString(block.BlockId)}, " +
                $"version_id={Convert.ToHexString(block.VersionId)}, " +
                $"block_type_id={block.BlockTypeId}, sequence_order={block.SequenceOrder}, " +
                $"content_data={block.ContentData}");

    }

    public static void Run(System.Data.IDbConnection db)
    {
        Console.WriteLine("\n=== ORMLITE SELECT DEMO ===");
        Console.WriteLine("This runs every Select example in order; enter values when prompted.\n");

        Console.WriteLine("\n--- Select all categories ---");
        ShowCategories(db.Select<Category>());

        Console.WriteLine("\n--- Select categories with a condition ---");
        SelectCategoriesByName(db);

        Console.WriteLine("\n--- Single with a condition ---");
        SingleCategoryByName(db);

        Console.WriteLine("\n--- SingleById ---");
        SingleCategoryById(db);

        Console.WriteLine("\n--- Select(...).First() ---");
        FirstMatchingCategory(db);

        Console.WriteLine("\n--- Exists with a condition ---");
        CheckCategoryExists(db);

        Console.WriteLine("\n--- Count with a condition ---");
        CountMatchingCategories(db);

        Console.WriteLine("\n--- Build From/Where query, then Select ---");
        BuildAndRunWhereQuery(db);

        Console.WriteLine("\n--- OrderBy and OrderByDescending ---");
        OrderCategories(db);

        Console.WriteLine("\n--- Skip and Take ---");
        PageCategories(db);

        Console.WriteLine("\n--- Select specific columns ---");
        SelectCategoryColumns(db);

        Console.WriteLine("\n--- Column<T> ---");
        SelectCategoryNames(db);

        Console.WriteLine("\n--- SelectDistinct ---");
        SelectDistinctArticleStatuses(db);

        Console.WriteLine("\n--- ColumnDistinct<T> ---");
        SelectDistinctArticleStatusesAsSet(db);

        Console.WriteLine("\nAll Select examples have run.");
    }

    private static string FormatId(byte[]? id) =>
        id is null ? "(null)" : Convert.ToHexString(id);

    private static void SelectCategoriesByName(System.Data.IDbConnection db)
    {
        Console.Write("Enter text to search in category names: ");
        string searchText = ReadRequiredInput();

        List<Category> categories =
            db.Select<Category>(category => category.Name.Contains(searchText));

        ShowCategories(categories);
    }

    private static void SingleCategoryByName(System.Data.IDbConnection db)
    {
        Console.Write("Enter the exact category name: ");
        string name = ReadRequiredInput();

        Category? category = db.Single<Category>(item => item.Name == name);
        ShowCategory(category);
    }

    private static void SingleCategoryById(System.Data.IDbConnection db)
    {
        Console.Write("Enter category_id: ");
        if (!int.TryParse(Console.ReadLine(), out int categoryId))
        {
            Console.WriteLine("Enter a valid numeric category_id.");
            return;
        }

        Category? category = db.SingleById<Category>(categoryId);
        ShowCategory(category);
    }

    private static void FirstMatchingCategory(System.Data.IDbConnection db)
    {
        Console.Write("Enter text to search in category names: ");
        string searchText = ReadRequiredInput();

        List<Category> matches =
            db.Select<Category>(item => item.Name.Contains(searchText));

        if (matches.Count == 0)
        {
            Console.WriteLine("No category found.");
            return;
        }

        Category firstCategory = matches.First();
        ShowCategory(firstCategory);
    }

    private static void CheckCategoryExists(System.Data.IDbConnection db)
    {
        Console.Write("Enter the exact category name: ");
        string name = ReadRequiredInput();

        bool exists = db.Exists<Category>(category => category.Name == name);
        Console.WriteLine(exists ? "A matching category exists." : "No matching category exists.");
    }

    private static void CountMatchingCategories(System.Data.IDbConnection db)
    {
        Console.Write("Enter text to search in category names: ");
        string searchText = ReadRequiredInput();

        long count = db.Count<Category>(category => category.Name.Contains(searchText));
        Console.WriteLine($"Matching categories: {count}");
    }

    private static void BuildAndRunWhereQuery(System.Data.IDbConnection db)
    {
        Console.Write("Enter text to search in category names: ");
        string searchText = ReadRequiredInput();

        SqlExpression<Category> query = db
            .From<Category>()
            .Where(category => category.Name.Contains(searchText));

        Console.WriteLine("The query is now built; Select executes it:");
        ShowCategories(db.Select(query));
    }

    private static void OrderCategories(System.Data.IDbConnection db)
    {
        Console.WriteLine("OrderBy(Name):");
        ShowCategories(db.Select(db.From<Category>().OrderBy(category => category.Name)));

        Console.WriteLine("\nOrderByDescending(Name):");
        ShowCategories(db.Select(
            db.From<Category>().OrderByDescending(category => category.Name)));
    }

    private static void PageCategories(System.Data.IDbConnection db)
    {
        Console.Write("How many categories to skip? ");
        if (!int.TryParse(Console.ReadLine(), out int skip) || skip < 0)
        {
            Console.WriteLine("Enter a non-negative number.");
            return;
        }

        Console.Write("How many categories to take? ");
        if (!int.TryParse(Console.ReadLine(), out int take) || take < 1)
        {
            Console.WriteLine("Enter a number greater than zero.");
            return;
        }

        SqlExpression<Category> query = db
            .From<Category>()
            .OrderBy(category => category.CategoryId)
            .Skip(skip)
            .Take(take);

        ShowCategories(db.Select(query));
    }

    private static void SelectCategoryColumns(System.Data.IDbConnection db)
    {
        SqlExpression<Category> query = db
            .From<Category>()
            .Select(category => new
            {
                category.CategoryId,
                category.Name
            });

        List<CategorySelectResult> results = db.Select<CategorySelectResult>(query);

        foreach (CategorySelectResult result in results)
        {
            Console.WriteLine($"{result.CategoryId}. {result.Name}");
        }
    }

    private static void SelectCategoryNames(System.Data.IDbConnection db)
    {
        SqlExpression<Category> query = db
            .From<Category>()
            .Select(category => category.Name);

        List<string> names = db.Column<string>(query);
        foreach (string name in names)
        {
            Console.WriteLine(name);
        }
    }

    private static void SelectDistinctArticleStatuses(System.Data.IDbConnection db)
    {
        SqlExpression<ArticleVersion> query = db
            .From<ArticleVersion>()
            .SelectDistinct(version => version.StatusId);

        List<int> statusIds = db.Column<int>(query);
        Console.WriteLine($"Distinct status IDs: {string.Join(", ", statusIds)}");
    }

    private static void SelectDistinctArticleStatusesAsSet(System.Data.IDbConnection db)
    {
        SqlExpression<ArticleVersion> query = db
            .From<ArticleVersion>()
            .Select(version => version.StatusId);

        HashSet<int> statusIds = db.ColumnDistinct<int>(query);
        Console.WriteLine($"Distinct status IDs: {string.Join(", ", statusIds.Order())}");
    }

    private static void ShowCategories(IEnumerable<Category> categories)
    {
        List<Category> results = categories.ToList();
        if (results.Count == 0)
        {
            Console.WriteLine("No categories found.");
            return;
        }

        foreach (Category category in results)
        {
            Console.WriteLine($"{category.CategoryId}. {category.Name}");
        }
    }

    private static void ShowCategory(Category? category)
    {
        Console.WriteLine(category is null
            ? "No category found."
            : $"{category.CategoryId}. {category.Name}");
    }

    private static string ReadRequiredInput()
    {
        string? value = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException("Search input cannot be empty.");
        }

        return value.Trim();
    }
}