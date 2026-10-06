using System;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.CSharp.RuntimeBinder;

namespace CSharpDemo.DynamicDemo;

public class Calculator {
    public void Print(int value) {
        Console.WriteLine($"Integer: {value}");
    }

    public void Print(string value) {
        Console.WriteLine($"String: {value}");
    }
}

public class Student {
    
    public string Name { get; set; }
    public int Age { get; set; }

    public void Introduce() {
        Console.WriteLine($"Hi, I am {Name}, age {Age}.");
    }
}

public class DynamicExample {

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
        /*
        ExpandoObject is a special .NET class that lets you dynamically add/remove properties and methods at runtime.
        */
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