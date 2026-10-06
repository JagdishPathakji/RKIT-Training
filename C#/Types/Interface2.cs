/*
Access specifiers in inheritance :-
"Everything in an interface is public"

Can we make a normal interface method private ? -> NO
Because this is a member with no implementation. It represents a contract that implementing classes must fulfill. A private member would not make sense as a contract for implementing classes.

Can we make a normal interface method protected ? -> NO
protected is an accessibility concept tied to inheritance, while implementing an interface is not class inheritance.

Can we make a normal interface method internal ? -> NO
This method is part of the interface contract, but only code in the same assembly should be able to use that interface member. An internal member would make that contract assembly-specific so it is not allowed.

Modern C# allows private interface members, but they must have an implementation. An implementing class cannot directly access them.
*/

/*
if a method in interface has implementation means it is virtual by default and it is optional to override them as well.
*/


// using System;

// public interface IContent
// {
//     // Public contract
//     void Display();

//     // Default implementation
//     void ShowInfo()
//     {
//         Validate();
//         Console.WriteLine("Showing content information");
//     }

//     // Private interface helper
//     private void Validate()
//     {
//         Console.WriteLine("Validating content");
//     }
// }

// class TextContent : IContent
// {
//     // Normal interface implementation
//     public void Display()
//     {
//         Console.WriteLine("Displaying text content");
//     }
// }

// class Program
// {
//     static void Main()
//     {
//         IContent content = new TextContent();

//         content.Display();

//         content.ShowInfo();

//         // content.Validate();  // ❌ Cannot access private interface member
//     }
// }




/*
Properties in Interface :
An interface can also declare properties.  The interface property is primarily a requirement / contract.
Example:-
interface IStudent {
    string Name {get; set;}
}

This does not mean the interface contains a `Name` variable. Interface can never contain instance variables. It means that "Any class that implements IStudent must provide a Name property with get and set."

so:
class Student : IStudent {

    public string Name {get; set;} // implement the property
}



Why we put a property in an interface ?
Think of interface as an contract. Above code says that "Every Student must have a way to read / write Name."

Complete Example:-

interface IStudent {
    string Name {get;}
}
class Student : IStudent {
    public string Name {get; set;}
} 
In this case, an reference of IStudent can only access get where Student object can access both get and set.
*/


interface IUser {

    int UserId {get;}
    string Name {get; set;}
    int RoleId {get;}
}

class Author : IUser {

    public int UserId { get; }
    public string Name { get; set; }
    public int RoleId { get; }

    public Author(int UserId, string name) {
        // for only get available, set inside constructor is still allowed
        RoleId = 2;
        UserId = UserId;
        Name = name;
    }
}

class Editor : IUser {

    public int UserId { get; }
    public string Name { get; set; }
    public int RoleId { get; }

    public Editor(int userId, string name) {
        // for only get available, set inside constructor is still allowed
        UserId = userId; 
        Name = name;
        RoleId = 3;       // Editor
    }
}

class Program {
    static void Main(string[] args) {

        IUser author = new Author(101, "Jagdish");
        IUser editor = new Editor(201, "Rahul");

        Console.WriteLine("Author");
        Console.WriteLine("ID: " + author.UserId);
        Console.WriteLine("Name: " + author.Name);
        Console.WriteLine("Role ID: " + author.RoleId);

        Console.WriteLine();

        Console.WriteLine("Editor");
        Console.WriteLine("ID: " + editor.UserId);
        Console.WriteLine("Name: " + editor.Name);
        Console.WriteLine("Role ID: " + editor.RoleId);

        Console.WriteLine();

        author.Name = "Jagdish Patel";

        Console.WriteLine("Updated Author Name: " + author.Name);
    }
}