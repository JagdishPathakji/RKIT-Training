using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpDemo.Types;

/*
Interface is a contract. It says:
"Any class that implements me must provide these members implementation".

For example, suppose our knowledgebase application has different types of content:
1. text content
2. code content
3. image content

We might want every content type to have a method called Display().
We could define:
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
Why not used Abstract class instead ?
Interfaces become important because a class can implement multiple interfaces at a time. Whereas it can inherit only one class at a time.
*/

/*
What interface can contain ?
1. methods
2. properties
3. static members
4. static constructors
5. default implementations
6. constants
*/

/*
An interface cannot contain ordinary instance fields. We cannot use an interface as a place to store normal instance state. An interface traditionally describes what something can do, rather than storing its instance data.

We cannot create object of an Interface. Because it does have any concrete implementations. It is just a contract. However we can create  a reference of it.
*/

/*
1. An interface is declared with interface keyword.
2. A normal interface method has no body.
3. A normal interface method is abstract.
4. A concrete implementing class must implement every required member.
5. An abstract class doesn't have to implement everything.
6. Normal interface implementation must be public.
7. Implementing method doesn't use override keyword.
8. Interface methods can have parameters, return values and can be overloaded.
9. Multiple classes can implement same interface.
10. A class can implement multiple interface and a class simultaneously.
11. An interface cannot normally be instantiated.
12. An interface reference can only access interface members.
13. Interface methods can have default implementation. (optional to implement by implementing class)
14. Interface can have static method.
*/

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
*/

/*
Constants in Interfaces:=
A constant is a value that cannot be changed after it is declared.
1. constants are automatically public
2. the class does not need to implement it
3. you cannot make it private
4. constants belongs to an interface rather than a particular class
that's why access using InterfaceName.ConstantName
5. Inside implementing class as well the constant is accessed as:-
InterfaceName.ConstantName
6. constants cannot be changed after declared
7. You don't create an object to access a constant (static-like)
*/

public interface IContentBlock {

    int SequenceOrder { get; }
    void Display();
}

public sealed class TextContentBlock : IContentBlock {
    
    public int SequenceOrder { get; }
    public string ContentData { get; }

    public TextContentBlock(int sequenceOrder, string contentData) {
        SequenceOrder = sequenceOrder;
        ContentData = contentData;
    }

    public void Display() {
        Console.WriteLine(ContentData);
    }
}

public sealed class CodeContentBlock : IContentBlock
{
    public int SequenceOrder { get; }
    public string ContentData { get; }

    public CodeContentBlock(int sequenceOrder, string contentData)
    {
        SequenceOrder = sequenceOrder;
        ContentData = contentData;
    }

    public void Display()
    {
        Console.WriteLine("[Code]");
        Console.WriteLine(ContentData);
        Console.WriteLine("[/Code]");
    }
}

public static class InterfaceDemo
{
    public static void Run()
    {
        Console.WriteLine("=== INTERFACES DEMO ===");

        List<IContentBlock> articleBlocks =
        [
            new TextContentBlock(2, "This is a C# variable declaration:"),
            new CodeContentBlock(1, "int articleCount = 3;"),
            new TextContentBlock(3, "The variable stores the number of articles.")
        ];

        foreach(IContentBlock block in articleBlocks.OrderBy(block => block.SequenceOrder))
        {
            block.Display();
        }

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey(true);
    }
}