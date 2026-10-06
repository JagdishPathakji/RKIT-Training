using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ORM.Models;

namespace ORM.Data;


public class AppDbContext : DbContext {
    
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    // You do not call OnConfiguring manually. It is a lifecycle method of DbContext.
    
    // EF Core invokes it internally when the context is created. That is why you never write context.OnConfiguring(...) in your code.
    
    // It is not a normal method you call yourself. It is called by the framework when it needs to build the database connection.

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if (!optionsBuilder.IsConfigured)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            string connectionString =
                configuration.GetConnectionString("KnowledgeBase");

            if (string.IsNullOrWhiteSpace(connectionString)) {
                throw new Exception("Connection string 'KnowledgeBase' not found.");
            }

            optionsBuilder.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString));
        }
    }
}