using System.Data;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.MySql;

namespace CSharpDemo.OrmLiteDemo;

/// <summary>Creates and opens connections to the knowledge_base MySQL database.</summary>
public static class KnowledgeBaseConnection
{
    private const string ConnectionStringEnvironmentVariable =
        "KNOWLEDGE_BASE_CONNECTION_STRING";

    /// <summary>Creates a factory configured for the knowledge_base MySQL database.</summary>
    /// <returns>A factory that can open database connections.</returns>
    public static OrmLiteConnectionFactory CreateFactory()
    {
        string? connectionString =
            Environment.GetEnvironmentVariable(ConnectionStringEnvironmentVariable);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                $"Set the {ConnectionStringEnvironmentVariable} environment variable " +
                "to a MySQL connection string for the knowledge_base database.");
        }

        return new OrmLiteConnectionFactory(
            connectionString,
            MySqlDialect.Provider);
    }

    /// <summary>Opens a connection and verifies it targets knowledge_base.</summary>
    /// <returns>An open connection to the knowledge_base database.</returns>
    public static IDbConnection Open()
    {
        IDbConnection db = CreateFactory().Open();

        try
        {
            string? databaseName = db.Scalar<string>("SELECT DATABASE()");

            if (!string.Equals(databaseName, "knowledge_base", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    $"Connected to '{databaseName ?? "(no database)"}'; expected 'knowledge_base'.");
            }

            return db;
        }
        catch
        {
            db.Dispose();
            throw;
        }
    }
}
