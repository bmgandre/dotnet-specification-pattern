using Microsoft.EntityFrameworkCore;
using SpecificationDemo.Data;
using System;

namespace SpecificationDemoTest.Context
{
    public class MockDbContext : IDisposable
    {
        public MockDbContext()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>()
                .UseInMemoryDatabase(databaseName: "Blog")
                .Options;

            BloggingContext = new BloggingContext(options);
        }

        public BloggingContext BloggingContext { get; }

        public void Dispose()
        {
            BloggingContext.Database.EnsureDeleted();
            BloggingContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}