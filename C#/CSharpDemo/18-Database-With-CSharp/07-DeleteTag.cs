using MySql.Data.MySqlClient;
namespace CSharpDemo.DatabaseWithCSharp;

internal static class DeleteTag {

    public static void Run() {

        int id = TagInput.ReadId();
        const string sql = "DELETE FROM TAG WHERE tag_id = @id;";

        using MySqlConnection connection = DatabaseConnection.Create();
        connection.Open();

        using var command = new MySqlCommand(sql, connection);
        command.Parameters.Add("@id",MySqlDbType.Int32).Value = id;

        int rowsAffected = command.ExecuteNonQuery();
        
        Console.WriteLine(
            rowsAffected == 1 ? "Tag deleted." : "Tag not found.");
    }
}