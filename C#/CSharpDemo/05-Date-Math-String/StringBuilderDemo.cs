using System;
using System.Text;
namespace CSharpDemo.DateMathString;

// StringBuilder is a class used to create and modify strings efficiently when the string changes multiple times.
//
// Namespace used: System.Text;
//
// 1. Understanding the problem
// - C# string is immutable.
// - That means once a string object is created, its existing content cannot be changed.
// - Whenever we do text += word, the original string is not modified. This becomes inefficient when we perform many modifications. In C#, when a string value is changed, the old string stays intact in memory, and the variable is redirected to a brand-new string object.
// - When you assign a new value to a string variable, that variable stops pointing to the old memory address and points to the new one. If no other variables or objects hold a reference to that old string, it becomes "unreachable".
// - The old string remains allocated on the managed heap, occupying space just as it did before, but it is now considered garbage.
// - The memory is released by the .NET Garbage Collector (GC). You cannot predict the exact moment this will happen.
//
//
//
// 2. What is StringBuilder ?
// - StringBuilder is a mutable sequence of characters.
// - StringBuilder sb = new StringBuilder();
// -  Unlike string, its existing character buffer can be modified.
// String
// ────────────────────────
// "Hello"
   // ↓ creates new string
// "Hello World"
   // ↓ creates new string
// "Hello World!"
//
//
// StringBuilder
// ────────────────────────
// [ Hello ]
    // ↓ modify same builder
// [ Hello World ]
    // ↓ modify same builder
// [ Hello World! ]
//
//
//
// 3. Understanding Buffer in StringBuilder
// - A buffer is basically a reserved area of memory used to temporarily store data while you work with it.
// - For StringBuilder, that data is characters.
// - sb.Capacity is the current capacity for that buffer.
// - sb.Length is the number of characters currently stored in the builder.
// - sb.MaxCapacity is the maximum capacity for the buffer, we don't handle this manually.
// - StringBuilder doesn't eliminate memory allocation completely. It reduces the number of allocations/copies that would otherwise happen from repeated string concatenation. (This change in memory happens only when StringBuilder needs to grow its storage because required set of characters can't fit into it.)
//

/// <summary>Represents the StringBuilderDemo type.</summary>
public class StringBuilderDemo
{
    /// <summary>Runs the demonstration.</summary>
    public void Run()
    {
        ConsoleHelper.Clear();
        Console.WriteLine("=== STRING BUILDER DEMO ===");


        // The default initial capacity is 16 characters.
        StringBuilder sb = new StringBuilder();

        // Append
        sb.Append("Hello");
        sb.Append(" ");
        sb.Append("Jagdish");
        Console.WriteLine(sb);

        // AppendLine
        sb.AppendLine();
        sb.AppendLine("Welcome to C#");
        Console.WriteLine(sb);

        // Insert
        sb.Insert(6, "Dear ");
        Console.WriteLine(sb);

        // Replace
        sb.Replace("C#", ".NET");
        Console.WriteLine(sb);

        // Character modification
        sb[0] = 'h';
        Console.WriteLine(sb);

        // Length
        Console.WriteLine($"Length: {sb.Length}");

        // Capacity
        Console.WriteLine($"Capacity: {sb.Capacity}");

        // Convert to string
        string result = sb.ToString();
        Console.WriteLine("\nFinal string:");
        Console.WriteLine(result);

        // Clear
        sb.Clear();
        Console.WriteLine($"\nAfter Clear: '{sb}'");

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey(true);
    }
}
