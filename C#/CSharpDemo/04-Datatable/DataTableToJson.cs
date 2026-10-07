using System;
using System.Data;
using System.Text.Json;
using System.Collections.Generic;

namespace CSharpDemo.DataTableDemo;

/// <summary>Represents the DataTableToJson type.</summary>
public class DataTableToJson {

    /// <summary>Runs the demonstration.</summary>
    public static void Run() {

        ConsoleHelper.Clear();
        Console.WriteLine("=== DATATABLE TO JSON DEMO ===");

        DataTable table = new DataTable("User");

        table.Columns.Add("Name", typeof(string));
        table.Columns.Add("Username", typeof(string));
        table.Columns.Add("Email", typeof(string));

        table.Rows.Add("Jagdish","jagEditor","pathakjijagdish1@gmail.com");
        table.Rows.Add("Mihir","MihirReviewer","pathakjimihir1@gmail.com");
        table.Rows.Add("Yug","YugAuthor","dholakiyaYug@gmail.com");
        table.Rows.Add("Rudra","RudraEditor","Gohilrudra1@gmail.com");

        // convert Datatable -> List of dictionary
        // object? means value can be any C# type, and it can also be null.
        var result = new List<Dictionary<string, object?>>();
        
        foreach(DataRow row in table.Rows) {

            var item = new Dictionary<string,object?>();

            foreach(DataColumn column in table.Columns) {
                item[column.ColumnName] = (row[column] == DBNull.Value) ? null : row[column]; 
            }

            result.Add(item);
        }

        // convert List to Json
        string json = JsonSerializer.Serialize(
            result,
            new JsonSerializerOptions {
                WriteIndented = true
            }
        );

        Console.WriteLine(json);
        
        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey(true);
    }
}
