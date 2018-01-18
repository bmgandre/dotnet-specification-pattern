using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SpecificationDemo.Data;
using System.IO;

namespace SpecificationDemoConsole
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BloggingContext>
    {
        public BloggingContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<BloggingContext>();
            var connectionString = configuration.GetConnectionString(configuration["Provider"]);

            if (configuration["Provider"] == "SqlServer")
            {
                builder.UseSqlServer(connectionString);
            }
            else
            {
                builder.UseSqlite(connectionString);
            }

            return new BloggingContext(builder.Options);
        }
    }
}