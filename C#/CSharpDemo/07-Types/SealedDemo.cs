using System;
namespace CSharpDemo.Types;
// A sealed class is a class that cannot be inherited.
// You can create its object, but class cannot be inherited.
// We need `sealed` to prevent further inheritance of a class.
// Sealed class can have normal members.
// Sealed class can implement interfaces.
// Sealed class can inherit from another class.
//
// Use sealed classes when:
    // 1. You may want to prevent subclasses from changing important behavior.
    // 2. Utility/domain classes where inheritance makes no sense.
    // 3. You want to protect class invariants / rules .
//


// This non-inheritable repository placeholder prints messages; it does not access a database.
sealed class KnowledgeBaseRepository {

    /// <summary>Prints a message simulating an article save.</summary>
    public void SaveArticle() {

        Console.WriteLine("Saving Article to database...");
    }

    /// <summary>Prints a message simulating an article deletion.</summary>
    public void DeleteArticle() {

        Console.WriteLine("Deleteing article from database...");
    }
}

/// <summary>Represents the SealedDemo type.</summary>
public class SealedDemo {

    /// <summary>Runs the demonstration.</summary>
    public static void Run() {

        Console.WriteLine("=== SEALED CLASSES DEMO ===");

        KnowledgeBaseRepository repo = new KnowledgeBaseRepository();
        repo.SaveArticle();
        repo.DeleteArticle();

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey(true);

    }
}
