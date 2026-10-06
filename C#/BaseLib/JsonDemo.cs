using System;
using System.Collections.Generic;
using System.Text.Json;

namespace BaseLib;

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public static class JsonDemo
{
    public static void Run()
    {
        // Use case 1: serialize one object to JSON
        Student student = new() { Id = 1, Name = "Asha", IsActive = true };

        string jsonString = JsonSerializer.Serialize(
            student,
            new JsonSerializerOptions { WriteIndented = true });

        Console.WriteLine("Serialized student:");
        Console.WriteLine(jsonString);

        // Use case 2: deserialize JSON into an object
        string rawJson = "{\"Id\":2,\"Name\":\"Rahul\",\"IsActive\":false}";
        Student? studentFromJson = JsonSerializer.Deserialize<Student>(rawJson);

        Console.WriteLine(
            "Deserialized student: " + studentFromJson?.Name +
            ", Active = " + studentFromJson?.IsActive);

        // Use case 3: serialize a list of objects
        List<Student> students = new()
        {
            new Student { Id = 1, Name = "Asha", IsActive = true },
            new Student { Id = 2, Name = "Rahul", IsActive = false }
        };

        string listJson = JsonSerializer.Serialize(
            students,
            new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

        Console.WriteLine("Student list JSON:");
        Console.WriteLine(listJson);
    }
}