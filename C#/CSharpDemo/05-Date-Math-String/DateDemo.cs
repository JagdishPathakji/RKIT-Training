using System;
namespace CSharpDemo.DateMathString;

public class DateTimeDemo
{
    public void Run()
    {
        ConsoleHelper.Clear();

        Console.WriteLine("=== DATE AND TIME DEMO ===");

        DateTime publishedAt = DateTime.Now;

        Console.WriteLine($"Published: {publishedAt}");
        Console.WriteLine($"Date: {publishedAt.Date}");
        Console.WriteLine($"Year: {publishedAt.Year}");
        Console.WriteLine($"Month: {publishedAt.Month}");
        Console.WriteLine($"Day: {publishedAt.Day}");
        Console.WriteLine($"Day of Week: {publishedAt.DayOfWeek}");

        Console.WriteLine($"\nAfter 7 days: {publishedAt.AddDays(7)}");
        Console.WriteLine($"After 1 month: {publishedAt.AddMonths(1)}");
        Console.WriteLine($"Before 2 days: {publishedAt.AddDays(-2)}");

        DateTime reviewDate = publishedAt.AddDays(7);
        TimeSpan difference = reviewDate - publishedAt;

        Console.WriteLine($"\nReview Date: {reviewDate}");
        Console.WriteLine($"Days until review: {difference.Days}");

        DateTimeOffset utcNow = DateTimeOffset.UtcNow;
        Console.WriteLine($"UTC: {utcNow}");

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey(true);
    }
}