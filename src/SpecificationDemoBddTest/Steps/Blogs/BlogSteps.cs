using SpecificationDemo.Entities;
using SpecificationDemoTest.Context;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SpecificationDemoTest.Steps.Blogs
{
    [Binding]
    public class BlogSteps
    {
        private readonly MockDbContext _mockDbContext;

        public BlogSteps(MockDbContext mockDbContext)
        {
            _mockDbContext = mockDbContext;
        }

        [Given(@"the following blogs exist:")]
        public async Task Given_the_following_blogs_exist(Table table)
        {
            var blogs = table.CreateSet<Blog>();
            await _mockDbContext.BloggingContext.Blogs.AddRangeAsync(blogs);
            await _mockDbContext.BloggingContext.SaveChangesAsync();
        }
    }
}