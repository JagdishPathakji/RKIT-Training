using System;
using System.Data;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.MySql;

namespace OrmLiteDemo;

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string City { get; set; }
}

public class Program
{
    public static void Main()
    {
        OrmLiteConnectionFactory dbFactory =
            new OrmLiteConnectionFactory(
                "Server=localhost;Database=ormlite_demo;User=root;Password=root;",
                MySqlDialect.Provider
            );

        using IDbConnection db = dbFactory.Open();

        // -----------------------------------------
        // Query 1
        // -----------------------------------------

        var query1 = db.From<Student>()
            .Where(x => x.Age >= 18)
            .OrderBy(x => x.Name)
            .Take(5);

        Console.WriteLine("QUERY 1");
        Console.WriteLine(query1.ToSelectStatement());


        // -----------------------------------------
        // Query 2
        // Change C# order:
        // Take -> Where -> OrderBy
        // -----------------------------------------

        var query2 = db.From<Student>()
            .Take(5)
            .Where(x => x.Age >= 18)
            .OrderBy(x => x.Name);

        Console.WriteLine("\nQUERY 2");
        Console.WriteLine(query2.ToSelectStatement());


        // -----------------------------------------
        // Query 3
        // OrderBy -> Take -> Where
        // -----------------------------------------

        var query3 = db.From<Student>()
            .OrderBy(x => x.Name)
            .Take(5)
            .Where(x => x.Age >= 18);

        Console.WriteLine("\nQUERY 3");
        Console.WriteLine(query3.ToSelectStatement());


        // -----------------------------------------
        // Query 4
        // Where -> Take -> OrderBy
        // -----------------------------------------

        var query4 = db.From<Student>()
            .Where(x => x.Age >= 18)
            .Take(5)
            .OrderBy(x => x.Name);

        Console.WriteLine("\nQUERY 4");
        Console.WriteLine(query4.ToSelectStatement());
    }
}