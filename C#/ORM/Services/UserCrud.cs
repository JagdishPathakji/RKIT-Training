using System;
using System.Linq;
using ORM.Data;
using ORM.Models;

namespace ORM.Services;

public class UserCrud
{
    public static void InsertUser()
    {
        Console.WriteLine("\n=== INSERT USER ===");

        using var context = new AppDbContext();

        User user = new User
        {
            UserId = Guid.NewGuid().ToByteArray(),
            Username = "demo_user_200",
            Email = "demo200@test.com",
            PasswordHash = "hash_200",
            CreatedAt = DateTime.Now
        };

        context.Users.Add(user);
        context.SaveChanges();

        Console.WriteLine("Inserted user: " + user.Username);
    }

    public static void GetAllUsers()
    {
        Console.WriteLine("\n=== GET ALL USERS ===");

        using var context = new AppDbContext();

        var users = context.Users
            .OrderBy(u => u.Username)
            .ToList();

        if (users.Count == 0)
        {
            Console.WriteLine("No users found.");
            return;
        }

        foreach (var user in users)
        {
            Console.WriteLine(user.Username + " | " + user.Email);
        }
    }

    public static void UpdateUserEmail(string username, string newEmail)
    {
        Console.WriteLine("\n=== UPDATE USER EMAIL ===");

        using var context = new AppDbContext();

        var user = context.Users
            .FirstOrDefault(u => u.Username == username);

        if (user == null)
        {
            Console.WriteLine("User not found.");
            return;
        }

        user.Email = newEmail;
        context.SaveChanges();

        Console.WriteLine("Updated email for: " + user.Username + " -> " + user.Email);
    }

    public static void DeleteUser(string username)
    {
        Console.WriteLine("\n=== DELETE USER ===");

        using var context = new AppDbContext();

        var user = context.Users
            .FirstOrDefault(u => u.Username == username);

        if (user == null)
        {
            Console.WriteLine("User not found.");
            return;
        }

        context.Users.Remove(user);
        context.SaveChanges();

        Console.WriteLine("Deleted user: " + username);
    }
}