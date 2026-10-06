// Sealed class : This class is the final class in this inheritance hierarchy. Nobody should inherit from it. A sealed class can have a parent, but cannot have children. A class be both abstract and sealed.

using System;

// we can also have sealed method's
// we can use this concept only in case of method overriding
// after it is sealed, further subclasses cannot override that method.
// we can seal both virtual and abstract methods 

// KnowledgeBaseRepository is intended to be the one concrete implementation of the repository's database behavior.
sealed class KnowledgeBaseRepository
{
    public void SaveArticle()
    {
        Console.WriteLine("Saving article to database...");
    }

    public void DeleteArticle()
    {
        Console.WriteLine("Deleting article from database...");
    }
}

class Program
{
    static void Main()
    {
        KnowledgeBaseRepository repository =
            new KnowledgeBaseRepository();

        repository.SaveArticle();
        repository.DeleteArticle();
    }
}
