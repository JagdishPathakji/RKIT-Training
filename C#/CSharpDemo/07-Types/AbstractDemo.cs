using System;
namespace CSharpDemo.Types;

/*
Abstract class is a class that is designed to be a base class for other classes. Abstract class cannot be instantiated. Abstract class can have both concrete members and abstract members. 

Abstract methods can have declaration but no implementations. Derived class is forced to have implementation of such methods. If they does not provide implementation, they became abstract as well. They override using same signature using `override` keyword.

Virtual methods are used in case where parent knows how to implement a method, but wants to allow child classes to change it if necessary. Child can override the virtual method. 


Abstract                                Virtual
1. Parent has no implementation         Parent has implementation
for methods
2. Child implementation is              Child implementation !required
required
3. Force child to implement             Allow child to customize
4. Parent class must be abstract        Can be normal or abstract
for abstract method


Normal Method ---> Reference Type decides which will be called.
Virtual Method --> C# looks at the actual object.
Abstract Method --> C# looks at the actual object.
New keyword --> Reference Type decides which will be called.
*/

abstract class User {

    public Guid UserId { get; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public DateTime CreatedAt { get; }

    protected User(string username, string email, string passwordHash) {
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        UserId = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }

    public void DisplayBasicInfo() {
        Console.WriteLine("User ID: " + UserId);
        Console.WriteLine("Username: " + Username);
        Console.WriteLine("Email: " + Email);
        Console.WriteLine("Created At: " + CreatedAt);
    }

    public abstract void DisplayRole();
}


class Author : User {

    public Author(string username, string email, string passwordHash) : base(username, email, passwordHash) {

    }

    public override void DisplayRole() {
        Console.WriteLine("Role : Author");
    }

    public void CreateDraftArticle() {
        Console.WriteLine(Username + " is creating an article draft.");
    }

    public void SubmitArticleForReview() {
        Console.WriteLine(Username + " submitted an article for review.");
    }
}

class Editor : User {

    public Editor(string username,string email, string passwordHash) : base(username, email, passwordHash) {
    }

    public override void DisplayRole() {
        Console.WriteLine("Role: Editor");
    }

    public void ReviewArticle() {
        Console.WriteLine(Username + " is reviewing an article.");
    }

    public void PublishArticle() {
        Console.WriteLine(Username + " published an article.");
    }
}

class Reviewer : User {

    public Reviewer(string username,string email,string passwordHash) : base(username, email, passwordHash) {
    }

    public override void DisplayRole() {
        Console.WriteLine("Role: Reviewer");
    }

    public void ReviewArticle() {
        Console.WriteLine(Username + " is reviewing an article.");
    }
}

public class AbstractDemo {

    public static void Run() {

        Console.WriteLine("=== ABSTRACT CLASSES DEMO ===");

        Author author = new Author("jagdish","jagdish@example.com","hashed_password");
        author.DisplayRole();
        author.DisplayBasicInfo();
        author.CreateDraftArticle();
        author.SubmitArticleForReview();

        Console.WriteLine("----------------");
        Console.WriteLine("----------------");

        Editor editor = new Editor("mihir","mihir@example.com",
        "hashed_password");
        editor.DisplayRole();
        editor.DisplayBasicInfo();
        editor.ReviewArticle();
        editor.PublishArticle();

        Console.WriteLine("----------------");
        Console.WriteLine("----------------");

        Reviewer reviewer = new Reviewer("rudra","rudra@example.com","hashed_passowrd");
        reviewer.DisplayRole();
        reviewer.DisplayBasicInfo();
        reviewer.ReviewArticle();

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey(true);

    }
}