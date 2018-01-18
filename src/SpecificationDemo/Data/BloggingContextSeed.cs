using SpecificationDemo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpecificationDemo.Data
{
    public class BloggingContextSeed
    {
        public static async Task SeedAsync(BloggingContext bloggingContext)
        {
            if (!bloggingContext.Blogs.Any())
            {
                bloggingContext.Blogs.AddRange(GetPreconfiguredBlogs());
                await bloggingContext.SaveChangesAsync();
            }
        }

        private static IEnumerable<Blog> GetPreconfiguredBlogs()
        {
            return new List<Blog>
            {
                new Blog
                {
                    Url = "http://rockmusic.blog.com",
                    Created = new DateTime(2017, 1, 1),
                    Activated = new DateTime(2017, 1, 1),
                    Posts = new List<Post>
                    {
                        new Post
                        {
                            Title = "[Review] Foo Fighters - Concrete and Gold",
                            Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                            Created = new DateTime(2017, 1, 1),
                            Activated = new DateTime(2017, 1, 1),
                        },
                        new Post
                        {
                            Title = "[Review] Queens of the Stone Age - Villians",
                            Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                            Created = new DateTime(2017, 2, 1),
                            Activated = new DateTime(2017, 2, 1),
                        },
                        new Post
                        {
                            Title = "[Review] Royal Blood - How Did We Get So Dark?",
                            Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                            Created = new DateTime(2017, 2, 1),
                            Activated = new DateTime(2017, 2, 1),
                        },
                        new Post
                        {
                            Title = "[Review] Liam Gallagher - As You Were",
                            Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                            Created = new DateTime(2017, 2, 15),
                            Activated = new DateTime(2017, 2, 15),
                        },
                        new Post
                        {
                            Title = "[Review] Prophets of Rage - Prophets of Rage",
                            Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                            Created = new DateTime(2017, 3, 20),
                            Activated = new DateTime(2017, 3, 30),
                        },
                        new Post
                        {
                            Title = "[Review] Robert Plant - Carry Fire",
                            Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                            Created = new DateTime(2017, 3, 20),
                            Activated = new DateTime(2017, 3, 30),
                        },
                    }
                },
                new Blog
                {
                    Url = "http://popmusic.blog.com",
                    Created = new DateTime(2015, 1, 1),
                    Activated = new DateTime(2015, 2, 1)
                },
                new Blog
                {
                    Url = "http://csharpnews.blog.com",
                    Created = new DateTime(2010, 1, 1),
                    Activated = new DateTime(2010, 6, 1)
                },
                new Blog
                {
                    Url = "http://prohibit-content.blog.com",
                    Created = new DateTime(2015, 1, 1),
                    Activated = new DateTime(2015, 3, 2)
                }
            };
        }
    }
}