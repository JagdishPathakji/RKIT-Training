using System.Data;
using ServiceStack.OrmLite;

namespace CSharpDemo.OrmLiteDemo;

/// <summary>Demonstrates full-row Update and field-limited UpdateOnly on a selected user.</summary>
public static class UpdateDemo
{
    // Demonstrates Update and UpdateOnly on an existing user row.
    /// <summary>Runs the demonstration.</summary>
    /// <param name="db">An open connection to the knowledge_base database.</param>
    public static void Run(IDbConnection db)
    {
        List<User> users = db.Select<User>();
        if (users.Count == 0)
        {
            Console.WriteLine("There are no users to update.");
            return;
        }

        Console.WriteLine("Users:");
        for (int i = 0; i < users.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {users[i].Username} ({users[i].Email})");
        }

        Console.Write("Choose a user number: ");
        if (!int.TryParse(Console.ReadLine(), out int selection) ||
            selection < 1 || selection > users.Count)
        {
            Console.WriteLine("Invalid user number.");
            return;
        }

        User user = users[selection - 1];
        Console.Write($"New username for {user.Username}: ");
        string? username = Console.ReadLine();
        Console.Write($"New email for {user.Email}: ");
        string? email = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email))
        {
            Console.WriteLine("Username and email cannot be empty.");
            return;
        }

        user.Username = username.Trim();
        user.Email = email.Trim();
        Console.WriteLine($"Update: {db.Update(user)} row updated.");
        DisplayUser(user);

        Console.Write("New email for UpdateOnly: ");
        string? updatedEmail = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(updatedEmail))
        {
            Console.WriteLine("Email cannot be empty; UpdateOnly was skipped.");
            return;
        }

        user.Email = updatedEmail.Trim();
        Console.WriteLine(
            $"UpdateOnly: {db.UpdateOnly(() => new User { Email = user.Email }, x => x.UserId == user.UserId)} row updated.");
        DisplayUser(user);
    }

    // Prints the selected user's non-sensitive fields.
    private static void DisplayUser(User user)
    {
        Console.WriteLine($"User: {user.Username} | {user.Email}");
    }
}
