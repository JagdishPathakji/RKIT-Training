using System;

public interface IEntity {
    Guid Id { get; }
}

public class Article : IEntity {
    public Guid Id { get; set; }
    public string Title { get; set; }

    public Article(Guid id, string title) {
        Id = id;
        Title = title;
    }
}

public class ArticleVersion : IEntity {
    public Guid Id { get; set; }
    public int VersionNumber { get; set; }

    public ArticleVersion(Guid id, int versionNumber) {
        Id = id;
        VersionNumber = versionNumber;
    }
}

public class ChangeTracker<T> where T : IEntity {
    private T entity;

    public ChangeTracker(T entity) {
        this.entity = entity;
    }

    public void Track() {
        Console.WriteLine("Tracking entity: " + entity.Id);
    }
}

class Program {
    static void Main(string[] args) {
        Article article = new Article(
            Guid.NewGuid(),
            "C# Interfaces"
        );

        ArticleVersion version = new ArticleVersion(
            Guid.NewGuid(),
            3
        );

        ChangeTracker<Article> articleTracker =
            new ChangeTracker<Article>(article);

        ChangeTracker<ArticleVersion> versionTracker =
            new ChangeTracker<ArticleVersion>(version);

        articleTracker.Track();
        versionTracker.Track();
    }
}