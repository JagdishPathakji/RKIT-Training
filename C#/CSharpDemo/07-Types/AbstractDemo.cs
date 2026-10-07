using System;
namespace CSharpDemo.Types;

// Abstract class is a class that is designed to be a base class for other classes. Abstract class cannot be instantiated. Abstract class can have both concrete members and abstract members.
//
// Abstract methods can have declaration but no implementations. Derived class is forced to have implementation of such methods. If they does not provide implementation, they became abstract as well. They override using same signature using `override` keyword.
//
// Virtual methods are used in case where parent knows how to implement a method, but wants to allow child classes to change it if necessary. Child can override the virtual method.
//
//
// Abstract                                Virtual
// 1. Parent has no implementation         Parent has implementation
// for methods
// 2. Child implementation is              Child implementation !required
// required
// 3. Force child to implement             Allow child to customize
// 4. Parent class must be abstract        Can be normal or abstract
// for abstract method
//
//
// Non-virtual method --> The expression's compile-time type determines which method implementation is called.
// Virtual Method --> C# looks at the actual object.
// Abstract Method --> C# looks at the actual object.
// New keyword --> The expression's compile-time type determines which hidden method is called.
//

abstract class User {

    /// <summary>Gets the user's unique identifier.</summary>
    public Guid UserId { get; }
    /// <summary>Gets or sets the username value.</summary>
    public string Username { get; set; }
    /// <summary>Gets or sets the email value.</summary>
    public string Email { get; set; }
    /// <summary>Gets or sets the password hash value.</summary>
    public string PasswordHash { get; set; }
    /// <summary>Gets when the account was created.</summary>
    public DateTime CreatedAt { get; }

    /// <summary>Initializes a user with account details.</summary>
    /// <param name="username">The user's username.</param>
    /// <param name="email">The user's email address.</param>
    /// <param name="passwordHash">The stored password hash.</param>
    protected User(string username, string email, string passwordHash) {
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        UserId = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }

    /// <summary>Prints the user's ID, username, email, and creation time.</summary>
    public void DisplayBasicInfo() {
        Console.WriteLine("User ID: " + UserId);
        Console.WriteLine("Username: " + Username);
        Console.WriteLine("Email: " + Email);
        Console.WriteLine("Created At: " + CreatedAt);
    }

    /// <summary>Displays the user's role.</summary>
    public abstract void DisplayRole();
}


class Author : User {

    /// <summary>Creates an author account.</summary>
    /// <param name="username">The author's username.</param>
    /// <param name="email">The author's email address.</param>
    /// <param name="passwordHash">The author's stored password hash.</param>
    public Author(string username, string email, string passwordHash) : base(username, email, passwordHash) {

    }

    /// <summary>Displays that this user is an author.</summary>
    public override void DisplayRole() {
        Console.WriteLine("Role : Author");
    }

    /// <summary>Creates draft article.</summary>
    public void CreateDraftArticle() {
        Console.WriteLine(Username + " is creating an article draft.");
    }

    /// <summary>Demonstrates submitting an article for editor review.</summary>
    public void SubmitArticleForReview() {
        Console.WriteLine(Username + " submitted an article for review.");
    }
}

class Editor : User {

    /// <summary>Creates an editor account.</summary>
    /// <param name="username">The editor's username.</param>
    /// <param name="email">The editor's email address.</param>
    /// <param name="passwordHash">The editor's stored password hash.</param>
    public Editor(string username,string email, string passwordHash) : base(username, email, passwordHash) {
    }

    /// <summary>Displays that this user is an editor.</summary>
    public override void DisplayRole() {
        Console.WriteLine("Role: Editor");
    }

    /// <summary>Demonstrates reviewing an article.</summary>
    public void ReviewArticle() {
        Console.WriteLine(Username + " is reviewing an article.");
    }

    /// <summary>Demonstrates publishing an article.</summary>
    public void PublishArticle() {
        Console.WriteLine(Username + " published an article.");
    }
}

class Reviewer : User {

    /// <summary>Creates a reviewer account.</summary>
    /// <param name="username">The reviewer's username.</param>
    /// <param name="email">The reviewer's email address.</param>
    /// <param name="passwordHash">The reviewer's stored password hash.</param>
    public Reviewer(string username,string email,string passwordHash) : base(username, email, passwordHash) {
    }

    /// <summary>Displays that this user is a reviewer.</summary>
    public override void DisplayRole() {
        Console.WriteLine("Role: Reviewer");
    }

    /// <summary>Demonstrates reviewing an article.</summary>
    public void ReviewArticle() {
        Console.WriteLine(Username + " is reviewing an article.");
    }
}

/// <summary>Represents the AbstractDemo type.</summary>
public class AbstractDemo {

    /// <summary>Runs the demonstration.</summary>
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
