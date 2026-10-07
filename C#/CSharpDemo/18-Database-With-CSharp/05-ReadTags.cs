using MySql.Data.MySqlClient;
namespace CSharpDemo.DatabaseWithCSharp;

/// <summary>Represents the ReadTags type.</summary>
internal static class ReadTags {

    /// <summary>Runs the demonstration.</summary>
    public static void Run() {

        const string sql = """
            SELECT tag_id, name
            FROM TAG
            ORDER BY tag_id
            LIMIT 100;
        """;

        using MySqlConnection connection = DatabaseConnection.Create();
        connection.Open();

        using var command = new MySqlCommand(sql, connection);
        using MySqlDataReader reader = command.ExecuteReader();

        bool foundAny = false;

        while(reader.Read()) {

            foundAny = true;

            var tag = new Tag {
                // Find the numeric column index (position) of a column using its name.
                Id = reader.GetInt32(reader.GetOrdinal("tag_id")),
                Name = reader.GetString(reader.GetOrdinal("name"))
            };

            Console.WriteLine(tag);
        }

        if(!foundAny) {
            Console.WriteLine("No Tags Found.");
        }
    }
}
