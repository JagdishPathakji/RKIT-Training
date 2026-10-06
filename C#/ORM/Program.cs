using ORM.Services;

class Program {
    public static void Main(string[] args) {

        UserCrud.InsertUser();
        UserCrud.GetAllUsers();
        UserCrud.UpdateUserEmail("demo_user_200", "updated200@test.com");
        UserCrud.GetAllUsers();
        UserCrud.DeleteUser("demo_user_200");

        UserRoleCrud.GetUsersWithRolesStartingFromUserRole();
        UserRoleCrud.GetUsersWithRolesStartingFromUser();
        UserRoleCrud.GetUsersWithRolesStartingFromRole();
    
    }
}