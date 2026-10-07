using System;
namespace CSharpDemo.DateMathString;

/// <summary>Represents the MathDemo type.</summary>
public class MathDemo
{
    /// <summary>Runs the demonstration.</summary>
    public void Run()
    {
        ConsoleHelper.Clear();

        Console.WriteLine("=== MATH AND NUMBERS DEMO ===");

        double rating = 4.736;

        Console.WriteLine($"Rating: {rating}");
        Console.WriteLine($"Round: {Math.Round(rating, 2)}");
        Console.WriteLine($"Ceiling: {Math.Ceiling(rating)}");
        Console.WriteLine($"Floor: {Math.Floor(rating)}");
        Console.WriteLine($"Absolute: {Math.Abs(-rating)}");

        Console.WriteLine($"\nMaximum: {Math.Max(10, 25)}");
        Console.WriteLine($"Minimum: {Math.Min(10, 25)}");
        Console.WriteLine($"Power: {Math.Pow(2, 3)}");
        Console.WriteLine($"Square Root: {Math.Sqrt(25)}");

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey(true);
    }
}
