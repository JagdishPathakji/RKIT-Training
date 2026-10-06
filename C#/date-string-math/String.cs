using System;

class Program
{
    static void Main()
    {
        string text = "  Hello World  ";

        // 1. Length
        Console.WriteLine("1. Length");
        Console.WriteLine("Text   : \"" + text + "\"");
        Console.WriteLine("Length : " + text.Length);


        // 2. Access individual characters
        Console.WriteLine("\n2. Access Characters");
        Console.WriteLine("First character : " + text[0]);
        Console.WriteLine("Character at index 2 : " + text[2]);


        // 3. ToUpper()
        Console.WriteLine("\n3. ToUpper()");
        Console.WriteLine(text.ToUpper());


        // 4. ToLower()
        Console.WriteLine("\n4. ToLower()");
        Console.WriteLine(text.ToLower());


        // 5. Trim()
        Console.WriteLine("\n5. Trim()");
        string trimmedText = text.Trim();
        Console.WriteLine("Before Trim : \"" + text + "\"");
        Console.WriteLine("After Trim  : \"" + trimmedText + "\"");


        // 6. Contains()
        Console.WriteLine("\n6. Contains()");
        Console.WriteLine("Contains \"World\" : " + text.Contains("World"));
        Console.WriteLine("Contains \"CSharp\" : " + text.Contains("CSharp"));


        // 7. StartsWith()
        Console.WriteLine("\n7. StartsWith()");
        Console.WriteLine("Starts with \"  Hello\" : " + text.StartsWith("  Hello"));
        Console.WriteLine("Starts with \"World\"    : " + text.StartsWith("World"));


        // 8. EndsWith()
        Console.WriteLine("\n8. EndsWith()");
        Console.WriteLine("Ends with \"World  \" : " + text.EndsWith("World  "));
        Console.WriteLine("Ends with \"Hello\"    : " + text.EndsWith("Hello"));


        // 9. IndexOf()
        Console.WriteLine("\n9. IndexOf()");
        Console.WriteLine("Index of \"World\" : " + text.IndexOf("World"));
        Console.WriteLine("Index of \"Hello\" : " + text.IndexOf("Hello"));


        // 10. LastIndexOf()
        Console.WriteLine("\n10. LastIndexOf()");
        string repeatedText = "Hello World Hello";
        Console.WriteLine("Text : " + repeatedText);
        Console.WriteLine("Last index of \"Hello\" : " +
                          repeatedText.LastIndexOf("Hello"));


        // 11. Substring()
        Console.WriteLine("\n11. Substring()");
        string name = "Jagdish";

        Console.WriteLine("Original : " + name);
        Console.WriteLine("Substring(0, 3) : " + name.Substring(0, 3));
        Console.WriteLine("Substring(3)    : " + name.Substring(3));


        // 12. Replace()
        Console.WriteLine("\n12. Replace()");
        string sentence = "I like Java";
        Console.WriteLine("Before : " + sentence);

        string replacedSentence = sentence.Replace("Java", "C#");

        Console.WriteLine("After  : " + replacedSentence);


        // 13. Remove()
        Console.WriteLine("\n13. Remove()");
        string word = "Programming";

        Console.WriteLine("Original : " + word);
        Console.WriteLine("Remove(4) : " + word.Remove(4));
        Console.WriteLine("Remove(0, 4) : " + word.Remove(0, 4));


        // 14. Insert()
        Console.WriteLine("\n14. Insert()");
        string language = "C#";

        Console.WriteLine("Original : " + language);

        string inserted = language.Insert(2, " Programming");

        Console.WriteLine("After Insert : " + inserted);


        // 15. Split()
        Console.WriteLine("\n15. Split()");
        string fruits = "Apple,Banana,Mango";

        string[] fruitArray = fruits.Split(',');

        Console.WriteLine("Original : " + fruits);

        Console.WriteLine("After Split:");

        foreach (string fruit in fruitArray)
        {
            Console.WriteLine(fruit);
        }


        // 16. Join()
        Console.WriteLine("\n16. Join()");
        string[] languages = { "C#", "Java", "Python" };

        string joinedLanguages = string.Join(" | ", languages);

        Console.WriteLine("Joined : " + joinedLanguages);


        // 17. Concat()
        Console.WriteLine("\n17. Concat()");
        string firstName = "Jagdish";
        string lastName = "Patel";

        string fullName = string.Concat(firstName, " ", lastName);

        Console.WriteLine("Full Name : " + fullName);


        // 18. Equals()
        Console.WriteLine("\n18. Equals()");
        string a = "Hello";
        string b = "Hello";
        string c = "hello";

        Console.WriteLine("a.Equals(b) : " + a.Equals(b));
        Console.WriteLine("a.Equals(c) : " + a.Equals(c));


        // 19. Compare()
        Console.WriteLine("\n19. Compare()");
        Console.WriteLine("Compare(\"Apple\", \"Banana\") : " +
                          string.Compare("Apple", "Banana"));

        Console.WriteLine("Compare(\"Apple\", \"Apple\") : " +
                          string.Compare("Apple", "Apple"));

        Console.WriteLine("Compare(\"Banana\", \"Apple\") : " +
                          string.Compare("Banana", "Apple"));


        // 20. String interpolation
        Console.WriteLine("\n20. String Interpolation");

        int age = 21;
        string studentName = "Jagdish";

        Console.WriteLine($"Name: {studentName}, Age: {age}");


        // 21. String immutability
        Console.WriteLine("\n21. String Immutability");

        string original = "Hello";

        Console.WriteLine("Original : " + original);

        original = original + " World";

        Console.WriteLine("After modification : " + original);


        // 22. Null and empty string
        Console.WriteLine("\n22. Null and Empty String");

        string emptyString = "";
        string? nullString = null;

        Console.WriteLine("Empty string Length : " + emptyString.Length);
        Console.WriteLine("Null string : " + (nullString == null));
    }
}