using SpecificationDemo.Entities;
using SpecificationDemoTest.Context;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SpecificationDemoTest.Steps.Posts
{
    [Binding]
    public class PostSteps
    {
        private readonly MockDbContext _mockDbContext;

        public PostSteps(MockDbContext mockDbContext)
        {
            _mockDbContext = mockDbContext;
        }

        [Given(@"the following posts exist:")]
        public async Task Given_the_following_posts_exist(Table table)
        {
            var posts = table.CreateSet<Post>();
            await _mockDbContext.BloggingContext.Posts.AddRangeAsync(posts);
            await _mockDbContext.BloggingContext.SaveChangesAsync();
        }
    }
}