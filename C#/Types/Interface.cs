/*
What is an Interface ?
An interface is a contract. It says:
"Any class that implements me must provide these members implementation."

For example, suppose your knowledge base application has different types of content:
1. text content
2. code content
3. image content
We might want every content type to have a method called Display().

we could define:
interface IContent {
    void Display();
}

class TextContent : IContent {

    public void Display() {
        // implementation
    }
}

class CodeContent : IContent {

    public void Display() {
        // implementation
    }
}
*/


/*
Why not use Abstract class everywhere ?
Interfaces become especially important because a class can implement multiple interfaces at a time. 
Whereas it can inherit only one class at a time.
*/

/*
What can an Interface contain ?
1. methods
2. properties
3. static members
4. static constructors
5. default implementations
6. constants
7. events
8. indexers
9. nested types

An interface cannot contain ordinary instance fields. We cannot use an interface as a place to store normal instance state. An interface traditionally describes what something can do, rather than storing its instance data.

We cannot create object of an Interface. Because it does have any concrete implementations. It is just a contract. However we can create  a reference of it.
*/

/*
1. An interface is declared with interface keyword
2. A normal interface method has no body
3. A normal interface method is abstract
4. A concrete implementing class must implement every required member
5. An abstract class doesn't have to implement everything
6. Normal interface implementation must be public
7. The implementing method doesn't use override
8. Interface methods can have parameters
9. Interface methods can return values
10. Interface methods can be overloaded
11. An interface can have multiple methods
12. Multiple classes can implement the same interface
13. A class can implement multiple interfaces
14. A class can inherit a class and implement interfaces simultaneously
15. An interface cannot normally be instantiated
16. Interface references provide polymorphism
17. An interface reference can access only interface members
18. Interface methods can have default implementations
19. Interfaces can have static methods (call it using interface)
20. Interface implementation can participate in inheritance
*/





// code
// using System;

// interface IContent
// {
//     void Display();
// }

// class TextContent : IContent
// {
//     public void Display()
//     {
//         Console.WriteLine("Displaying text content");
//     }

//     public void FormatText()
//     {
//         Console.WriteLine("Formatting text");
//     }
// }

// class CodeContent : IContent
// {
//     public void Display()
//     {
//         Console.WriteLine("Displaying code content");
//     }
// }

// class Program
// {
//     static void Main()
//     {
//         IContent content;

//         content = new TextContent();
//         content.Display();

//         content = new CodeContent();
//         content.Display();
//     }
// }





// access specifiers in interface
// properties in interface
// constants in interface
// multiple inheritance in interface
// explicit interface implementation






using System;

interface IContent
{
    void Display();

    void Save();

    string GetTitle();
}

interface IReviewable
{
    void Review();
}

class TextContent : IContent, IReviewable
{
    private string title;

    public TextContent(string title)
    {
        this.title = title;
    }

    public void Display()
    {
        Console.WriteLine("Displaying text content");
    }

    public void Save()
    {
        Console.WriteLine("Saving text content");
    }

    public string GetTitle()
    {
        return title;
    }

    public void Review()
    {
        Console.WriteLine("Reviewing text content");
    }

    public void FormatText()
    {
        Console.WriteLine("Formatting text");
    }
}

class CodeContent : IContent
{
    public void Display()
    {
        Console.WriteLine("Displaying code content");
    }

    public void Save()
    {
        Console.WriteLine("Saving code content");
    }

    public string GetTitle()
    {
        return "C# Code";
    }
}

class Program
{
    static void Main()
    {
        IContent content;

        content = new TextContent("C# Interfaces");

        content.Display();
        content.Save();

        Console.WriteLine(content.GetTitle());

        content = new CodeContent();

        content.Display();
        content.Save();

        Console.WriteLine(content.GetTitle());


        IReviewable reviewable = new TextContent("C# Interfaces");

        reviewable.Review();
    }
}