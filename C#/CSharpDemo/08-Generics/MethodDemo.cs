using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpDemo.Generics;

// Generic Methods
//

// The class doesn't need to e generic. Only method has a type parameter.
//
// syntax:
// returnType methodName<T>(T parameter) {
    // // code
// }
//
// methodName<T>(argument)
// or
// methodname(argument) // c# automatically infers the data type
//

/// <summary>Represents the UserRow type.</summary>
public class UserRow
{
    /// <summary>Gets or sets the username value.</summary>
    public string Username { get; set; } = "";
    /// <summary>Gets or sets the email value.</summary>
    public string Email { get; set; } = "";
}

/// <summary>Represents the RoleRow type.</summary>
public class RoleRow
{
    /// <summary>Gets or sets the role id value.</summary>
    public int RoleId { get; set; }
    /// <summary>Gets or sets the role name value.</summary>
    public string RoleName { get; set; } = "";
}

/// <summary>Represents the MethodDemoGen type.</summary>
public static class MethodDemoGen
{
    /// <summary>Runs the demonstration.</summary>
    public static void Run()
    {

        Console.WriteLine("=== GENERIC METHOD DEMO ===");

        var users = new List<UserRow>
        {
            new UserRow { Username = "demo_user_200", Email = "demo200@test.com" },
            new UserRow { Username = "alex", Email = "alex@test.com" }
        };

        var roles = new List<RoleRow>
        {
            new RoleRow { RoleId = 1, RoleName = "Admin" },
            new RoleRow { RoleId = 2, RoleName = "Member" }
        };

        // Use case 1: print USER rows.
        PrintRows(users, user => $"{user.Username} | {user.Email}");

        // The same generic method also prints ROLE rows.
        PrintRows(roles, role => $"{role.RoleId} | {role.RoleName}");

        // Use case 2: find a user or role using the same generic method.
        UserRow? foundUser =
            FindFirst(users, user => user.Username == "demo_user_200");

        RoleRow? foundRole =
            FindFirst(roles, role => role.RoleName == "Admin");

        Console.WriteLine(foundUser == null
            ? "User not found."
            : $"Found user: {foundUser.Username}");

        Console.WriteLine(foundRole == null
            ? "Role not found."
            : $"Found role: {foundRole.RoleName}");
    }

    private static void PrintRows<T>(
        IEnumerable<T> rows,
        Func<T, string> formatRow)
    {
        foreach (T row in rows)
        {
            Console.WriteLine(formatRow(row));
        }
    }

    private static T? FindFirst<T>(
        IEnumerable<T> rows,
        Func<T, bool> condition)
    {
        return rows.FirstOrDefault(condition);
    }
}
