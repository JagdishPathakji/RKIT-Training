using MySql.Data.MySqlClient;
namespace CSharpDemo.DatabaseWithCSharp;

/// <summary>Represents the UpdateTag type.</summary>
internal static class UpdateTag {

    /// <summary>Runs the demonstration.</summary>
    public static void Run() {

        int id = TagInput.ReadId();
        string name = TagInput.ReadName("New Tag Name: ");

        const string sql = """
            UPDATE TAG
            SET name = @name
            WHERE tag_id = @id;
        """;

        using MySqlConnection connection = DatabaseConnection.Create();
        connection.Open();

        using var command = new MySqlCommand(sql, connection);
        command.Parameters.Add("@name", MySqlDbType.VarChar, 100).Value = name;
        command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

        int rowsAffected = command.ExecuteNonQuery();

        Console.WriteLine(rowsAffected == 1 ? "Tag updated." : "No rows changed.");
    }
}
