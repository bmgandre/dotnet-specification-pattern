using Microsoft.EntityFrameworkCore;
using SpecificationDemo.Entities;

namespace SpecificationDemo.Data
{
    public class BloggingContext : DbContext
    {
        private string _connectionString;

        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<Post> Posts { get; set; }

        public BloggingContext()
        {
        }

        public BloggingContext(DbContextOptions<BloggingContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Required to use in LinqPad
        /// </summary>
        /// <param name="connectionString"></param>
        public BloggingContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrEmpty(_connectionString))
            {
                // optionsBuilder.UseSqlite(_connectionString);
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }
    }
}