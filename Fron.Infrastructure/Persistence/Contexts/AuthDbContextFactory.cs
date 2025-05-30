using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Fron.Infrastructure.Persistence.Contexts;
public class AuthDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
{
    public AuthDbContext CreateDbContext(string[] args)
    {
        var parent = Directory.GetParent(Directory.GetCurrentDirectory());

        // Set up configuration to read from appsettings.json of startup project
        var basePath = Path.Combine(parent!.FullName, "Fron.ApiProjectExtensions");
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("AuthDbConnection");

        var optionsBuilder = new DbContextOptionsBuilder<AuthDbContext>();
        optionsBuilder.UseSqlServer(connectionString); // or any other provider

        return new AuthDbContext(optionsBuilder.Options);
    }
}