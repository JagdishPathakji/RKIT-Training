using System;
using MySql.Data.MySqlClient;


// others
using CSharpDemo;
using CSharpDemo.NamespaceAndLibraries;
using CSharpDemo.Enums;
using CSharpDemo.DataTableDemo;
using CSharpDemo.DateMathString;
using CSharpDemo.FileDirectory;
using CSharpDemo.ScopeAndAccessibility;
using CSharpDemo.DatabaseWithCSharp;
using CSharpDemo.DynamicDemo;
using CSharpDemo.LambdaExpression;
using CSharpDemo.ExtensionMethod;
using CSharpDemo.Types;
using CSharpDemo.LINQList;
using CSharpDemo.LINQTable;

class Program {

    public static void Main() {

        while(true) {

            ConsoleHelper.Clear();

            Console.WriteLine("======================");
            Console.WriteLine("0-Exit");
            Console.WriteLine("01-Scope-And-Accessibility");
            Console.WriteLine("02-Namespace-And-Libraries");    
            Console.WriteLine("03-Enumerations");
            Console.WriteLine("04-DataTable");
            Console.WriteLine("05-Date-Math-String");
            Console.WriteLine("06-File-Operations");
            Console.WriteLine("07-Types (Abstract, Sealed, Interfaces)");
            Console.WriteLine("12-Lambda-Expressions");
            Console.WriteLine("13-Extension-Methods");
            Console.WriteLine("14-LINQ");
            Console.WriteLine("17-Dynamic-Type");
            Console.WriteLine("18-CRUD-Database-With-C#");
            Console.WriteLine("19-String-Interpolation");
            Console.WriteLine("======================");

            Console.Write("Select Module: ");
            string? choice = Console.ReadLine();

            switch(choice) {

                case "1": 
                        Call1();
                        break;
                case "2":
                        Call2();
                        break;
                case "3":
                        Call3();
                        break;
                case "4":
                        Call4();
                        break;
                case "5":
                        Call5();
                        break;
                case "6":
                        Call6();
                        break;
                case "7":
                        Call7();
                        break;
                case "12":
                        Call12();
                        break;
                case "13":
                        Call13();
                        break;
                case "14":
                        Call14();
                        break;
                case "17":
                        Call17();
                        break;
                case "18":
                        Call18();
                        break;
                case "19":
                        Call19();
                        break;
                case "0":   
                        return;
                default:
                    Console.WriteLine("Invalid choice.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey(true);
                    break;
            }
        }
    }

    static void Call1() {
        AccessSpecifiersDemo demo = new AccessSpecifiersDemo();
        demo.Run();
    }

    static void Call2() {
        NamespaceDemo demo = new NamespaceDemo();
        demo.Run();
    }

    static void Call3() {
        EnumsDemo demo = new EnumsDemo();
        demo.Run();
    }

    static void Call4() {
        DataTableDemo demo = new DataTableDemo();
        demo.Run();
    }

    static void Call5() {
        DateTimeDemo demo1 = new DateTimeDemo();
        demo1.Run();
        MathDemo demo2 = new MathDemo();
        demo2.Run();
        StringDemo demo3 = new StringDemo();
        demo3.Run();
        StringBuilderDemo demo4 = new StringBuilderDemo();
        demo4.Run();
    }

    static void Call6() {
        FileDemo demo1 = new FileDemo();
        demo1.Run();
        DirectoryDemo demo2 = new DirectoryDemo();
        demo2.Run();
    }

    static void Call7() {

        ConsoleHelper.Clear();
        AbstractDemo.Run();
        ConsoleHelper.Clear();
        SealedDemo.Run();
        ConsoleHelper.Clear();
        InterfaceDemo.Run();
    }

    static void Call12() {

        ConsoleHelper.Clear();
        LambdaExpressionDemo.Run();
    }

    static void Call13() {

        ConsoleHelper.Clear();
        ExtensionMethodDemo.Run();
    }

    static void Call14() {
        
        ConsoleHelper.Clear();
        ListDemoLinq.Run();
        ConsoleHelper.Clear();
        DataTableDemoLinq.Run();
    }

    static void Call17() {

        ConsoleHelper.Clear();
        DynamicExample.Run();
    }

    static void Call18() {

        while (true) {
            ConsoleHelper.Clear();
            Console.WriteLine("=== KNOWLEDGE BASE TAG CRUD DEMO ===");
            Console.WriteLine("\n1. Create tag");
            Console.WriteLine("2. Read tags");
            Console.WriteLine("3. Update tag");
            Console.WriteLine("4. Delete tag");
            Console.WriteLine("5. Return to main menu");
            Console.Write("Choose: ");

            string? choice = Console.ReadLine();

            if(choice == "5") {
                return;
            }

            try {
                switch (choice) {
                    case "1":
                        CreateTag.Run();
                        break;
                    case "2":
                        ReadTags.Run();
                        break;
                    case "3":
                        UpdateTag.Run();
                        break;
                    case "4":
                        DeleteTag.Run();
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
            // MySQL error 1062 means a duplicate-key violation.
            catch(MySqlException ex) when (ex.Number == 1062) {
                Console.WriteLine("A tag with that name already exists.");
                Console.Error.WriteLine(ex);
            }
            catch(MySqlException ex) {
                Console.WriteLine(
                    "Database operation failed.");
                Console.Error.WriteLine(ex);
            }
            catch(Exception ex) {
                Console.WriteLine(
                    "Operation failed.");
                Console.Error.WriteLine(ex);
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
        }
    }

    static void Call19() {

        ConsoleHelper.Clear();
        Console.WriteLine("=== INTERPOLATION DEMO ===");

        string name = "Jagdish";
        int age = 21;
        double marks = 91.5678;
        DateTime date = new DateTime(2026, 10, 6);

        // 1. Basic interpolation
        Console.WriteLine($"Name: {name}");

        // 2. Multiple values
        Console.WriteLine($"Name: {name}, Age: {age}");

        // 3. Expression
        Console.WriteLine($"Age after 5 years: {age + 5}");

        // 4. Method call
        Console.WriteLine($"Uppercase: {name.ToUpper()}");

        // 5. Conditional expression
        Console.WriteLine(
            $"Status: {(age >= 18 ? "Adult" : "Minor")}"
        );

        // 6. Number formatting
        Console.WriteLine($"Marks: {marks:F2}");

        // 7. Date formatting
        Console.WriteLine($"Date: {date:dd-MM-yyyy}");

        // 8. Literal braces
        Console.WriteLine($"{{ Name = {name} }}");

        // 9. Verbatim + interpolation
        string path = $@"C:\Users\{name}\Documents";
        Console.WriteLine(path);

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey(true);
    }
}