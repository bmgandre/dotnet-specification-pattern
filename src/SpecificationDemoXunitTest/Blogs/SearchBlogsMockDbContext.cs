using Microsoft.EntityFrameworkCore;
using SpecificationDemo.Data;
using SpecificationDemo.Entities;
using System;
using System.Collections.Generic;

namespace SpecificationDemoXunitTest.Blogs
{
    public class SearchBlogsMockDbContext
    {
        public SearchBlogsMockDbContext()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>()
                .UseInMemoryDatabase(databaseName: "Blog")
                .Options;

            BloggingContext = new BloggingContext(options);

            MockData();
        }

        public BloggingContext BloggingContext { get; }

        public void Dispose()
        {
            BloggingContext.Database.EnsureDeleted();
            BloggingContext.Dispose();
            GC.SuppressFinalize(this);
        }

        private void MockData()
        {
            BloggingContext.AddRange(GetBlogs());
            BloggingContext.SaveChanges();
        }

        private IEnumerable<Blog> GetBlogs()
        {
            return new List<Blog>()
            {
                new Blog
                {
                    BlogId = 1,
                        Url = "http://pcworld.blog.com",
                        Created = new DateTime(2017, 1, 1)
                },
                new Blog
                {
                    BlogId = 2,
                        Url = "http://csharpnews.blog.com",
                        Created = new DateTime(2015, 1, 1),
                },
                new Blog
                {
                    BlogId = 3,
                        Url = "http://dotnetcore.blog.com",
                        Created = new DateTime(2017, 6, 23)
                }
            };
        }
    }
}