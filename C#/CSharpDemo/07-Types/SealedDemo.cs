using System;
namespace CSharpDemo.Types;
/*
A sealed class is a class that cannot be inherited.
You can create its object, but class cannot be inherited.
We need `sealed` to prevent further inheritance of a class.
Sealed class can have normal members.
Sealed class can implement interfaces.
Sealed class can inherit from another class.

Use sealed classes when:
    1. You may want to prevent subclasses from changing important behavior.
    2. Utility/domain classes where inheritance makes no sense.
    3. You want to protect class invariants / rules .
*/


// KnowledgeBaseRepository is intended to be the one concrete implementation for the database communication.
sealed class KnowledgeBaseRepository {

    public void SaveArticle() {

        Console.WriteLine("Saving Article to database...");
    }

    public void DeleteArticle() {

        Console.WriteLine("Deleteing article from database...");
    }
}

public class SealedDemo {

    public static void Run() {

        Console.WriteLine("=== SEALED CLASSES DEMO ===");

        KnowledgeBaseRepository repo = new KnowledgeBaseRepository();
        repo.SaveArticle();
        repo.DeleteArticle();

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey(true);

    }
}