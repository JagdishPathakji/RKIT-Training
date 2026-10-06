using System;
using Microsoft.EntityFrameworkCore;
using ORM.Data;

namespace ORM.Services;

public class UserRoleCrud
{
    public static void GetUsersWithRolesStartingFromUserRole()
    {
        Console.WriteLine("\n=== START FROM USER_ROLE ===");

        using var context = new AppDbContext();

        var userRoles = context.UserRoles
            .Include(ur => ur.User)
            .Include(ur => ur.Role)
            .ToList();

        foreach (var userRole in userRoles)
        {
            Console.WriteLine(userRole.User.Username + " | " + userRole.User.Email + " | " + userRole.Role.RoleName);
        }
    }

    public static void GetUsersWithRolesStartingFromUser()
    {
        Console.WriteLine("\n=== START FROM USER ===");

        using var context = new AppDbContext();

        var users = context.Users
            .Include(user => user.UserRoles)
            .ThenInclude(userRole => userRole.Role)
            .ToList();

        foreach (var user in users)
        {
            foreach (var userRole in user.UserRoles)
            {
                Console.WriteLine(user.Username + " | " + user.Email + " | " + userRole.Role.RoleName);
            }
        }
    }

    public static void GetUsersWithRolesStartingFromRole()
    {
        Console.WriteLine("\n=== START FROM ROLE ===");

        using var context = new AppDbContext();

        var roles = context.Roles
            .Include(role => role.UserRoles)
            .ThenInclude(userRole => userRole.User)
            .ToList();

        foreach (var role in roles)
        {
            foreach (var userRole in role.UserRoles)
            {
                Console.WriteLine(userRole.User.Username + " | " + userRole.User.Email + " | " + role.RoleName);
            }
        }
    }
}