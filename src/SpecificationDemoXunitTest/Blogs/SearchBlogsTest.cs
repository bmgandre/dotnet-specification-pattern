using NFluent;
using SpecificationDemo.Data;
using SpecificationDemo.Entities;
using SpecificationDemo.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SpecificationDemoXunitTest.Blogs
{
    public class SearchBlogsTest : IClassFixture<SearchBlogsMockDbContext>
    {
        private readonly SearchBlogsMockDbContext _mockDbContext;

        public SearchBlogsTest(SearchBlogsMockDbContext mockDbContext)
        {
            _mockDbContext = mockDbContext;
        }

        [Theory]
        [Xunit.InlineDataAttribute("2014-12-25", "3")]
        [Xunit.InlineDataAttribute("2015-02-01", "2")]
        [Xunit.InlineDataAttribute("2017-03-05", "1")]
        public async Task Get_blogs_not_expired_and_created_after_date(string dateParameter, string countParameter)
        {
            // Arrange
            var afterDate = DateTime.Parse(dateParameter);
            var count = long.Parse(countParameter);

            // Act
            var blogRepository = new EfReadRepository<Blog>(_mockDbContext.BloggingContext);
            var service = new BlogService(blogRepository);
            var searchResult = await service.GetNotExpiredBlogsAfterAsync(afterDate, CancellationToken.None);

            // Assert
            Check.That(searchResult).IsEqualTo(count);
        }
    }
}