using MySql.Data.MySqlClient;
namespace CSharpDemo.DatabaseWithCSharp;

internal static class DatabaseConnection {

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