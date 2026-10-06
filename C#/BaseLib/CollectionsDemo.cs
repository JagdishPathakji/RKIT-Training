using System;
using System.Collections.Generic;

namespace BaseLib;

public static class CollectionsDemo
{
    public static void Run()
    {
        // Use case 1: store and sort a list of names
        List<string> students = new() { "Asha", "Rahul", "Neha" };
        students.Add("Sameer");
        students.Sort();

        Console.WriteLine("Students: " + string.Join(", ", students));

        // Use case 2: use dictionary for key-value lookup
        Dictionary<string, int> marks = new()
        {
            ["C#"] = 95,
            ["SQL"] = 90,
            ["HTML"] = 88
        };

        Console.WriteLine("C# score: " + marks["C#"]);

        // Use case 3: store only unique values
        HashSet<string> emails = new()
        {
            "asha@gmail.com",
            "rahul@gmail.com",
            "asha@gmail.com"
        };

        emails.Add("neha@gmail.com");
        Console.WriteLine("Unique emails: " + string.Join(", ", emails));
    }
}