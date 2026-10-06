using System;

// we dont create User oject directly.
// no User creation without specific role assignment.

// Base class contains common data
// every subclass gets its data and methods
// force derived class to provide behavior that must vary


// 1. If a class contains an abstract member The class must be abstract.
// 2. If a class contains only virtual membersThe class does NOT need to be abstract.


// In normal inheritance where both class have show(), 
// Parent p = new Child();
// p.show(); // calls parent's show in normal case and calls child's show if parent's show is virtual

// Child c = new Child();
// c.show(); // calls itself's show() if it has or goes for parents if does not have it


abstract class User {

    public Guid UserId {get;}
    public string Username {get; set;}
    public string Email {get; set;}
    public string PasswordHash {get; set;}
    public DateTime CreatedAt {get;}


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

class Program {

    static void Main(string[] args) {

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
    }
}