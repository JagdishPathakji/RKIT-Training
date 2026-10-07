using MySql.Data.MySqlClient;
namespace CSharpDemo.DatabaseWithCSharp;

/// <summary>Represents the DatabaseConnection type.</summary>
internal static class DatabaseConnection {

    /// <summary>Creates a MySQL connection for the knowledge_base database.</summary>
    /// <returns>A configured MySQL connection that has not yet been opened.</returns>
    public static MySqlConnection Create() {

        const string variableName = "KNOWLEDGE_BASE_CONNECTION_STRING";
        string? connectionString = Environment.GetEnvironmentVariable(variableName);

        if(string.IsNullOrWhiteSpace(connectionString)) {
            throw new InvalidOperationException($"Set the {variableName} environment variable.");
        }

        var builder = new MySqlConnectionStringBuilder(connectionString);
        
        if(!string.Equals(builder.Database,"knowledge_base",StringComparison.OrdinalIgnoreCase)) {
            throw new InvalidOperationException("The connection string must target knowledge_base database");
        }
    
        return new MySqlConnection(builder.ConnectionString);
    }
}
