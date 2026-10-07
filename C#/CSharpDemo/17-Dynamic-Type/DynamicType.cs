using System;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.CSharp.RuntimeBinder;

namespace CSharpDemo.DynamicDemo;

/// <summary>Represents the Calculator type.</summary>
public class Calculator {
    /// <summary>Prints an integer value with its type label.</summary>
    /// <param name="value">The integer to print.</param>
    public void Print(int value) {
        Console.WriteLine($"Integer: {value}");
    }

    /// <summary>Prints a string value with its type label.</summary>
    /// <param name="value">The string to print.</param>
    public void Print(string value) {
        Console.WriteLine($"String: {value}");
    }
}

/// <summary>Represents the Student type.</summary>
public class Student {
    
    /// <summary>Gets or sets the name value.</summary>
    public string Name { get; set; }
    /// <summary>Gets or sets the age value.</summary>
    public int Age { get; set; }

    /// <summary>Prints an introduction using the student's name and age.</summary>
    public void Introduce() {
        Console.WriteLine($"Hi, I am {Name}, age {Age}.");
    }
}

/// <summary>Represents the DynamicExample type.</summary>
public class DynamicExample {

    /// <summary>Runs the demonstration.</summary>
    public static void Run() {

        Console.WriteLine("=== DYNAMIC TYPE DEMO ===");
            
        // 1. Basic dynamic usage
        dynamic value = "Jagdish";
        Console.WriteLine(value);
        Console.WriteLine(value.Length);

        value = 100;
        Console.WriteLine(value);
        Console.WriteLine(value + 50);


        // 2. Dynamic object
        dynamic student = new Student
            {
                Name = "Jagdish",
                Age = 21
            };

        Console.WriteLine(student.Name); // resolved at runtime
        Console.WriteLine(student.Age); // resolved at runtime
        student.Introduce(); // resolved at runtime


        // 3. Runtime overload resolution
        Calculator calculator = new Calculator();

        dynamic number = 10;
        dynamic text = "Hello";

        calculator.Print(number);
        calculator.Print(text);


        // 4. List<dynamic>
        List<dynamic> values = new List<dynamic>();

        values.Add(10);
        values.Add("Hello");
        values.Add(3.14);
        values.Add(true);

        foreach(dynamic item in values) {
            Console.WriteLine($"{item} -> {item.GetType().Name}");
        }


        // 5. ExpandoObject
        // ExpandoObject lets you dynamically add and remove members; a stored delegate can be invoked like a method.
//
        dynamic person = new ExpandoObject();

        person.Name = "Jagdish";
        person.Age = 21;
        person.City = "Anand";

        Console.WriteLine(person.Name);
        Console.WriteLine(person.Age);
        Console.WriteLine(person.City);


        // 6. Casting
        dynamic numberValue = 100;
        int intValue = (int)numberValue;
        Console.WriteLine(intValue);


        // 7. Dynamic conversion
        dynamic integer = 10;
        double doubleValue = integer;
        Console.WriteLine(doubleValue);


        // 8. Runtime error
        dynamic message = "Hello";
        try {
            Console.WriteLine(message.NotExistingProperty);
        } catch(RuntimeBinderException ex) {
            Console.WriteLine("RuntimeBinderException occurred.");
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey(true);
    }
}
