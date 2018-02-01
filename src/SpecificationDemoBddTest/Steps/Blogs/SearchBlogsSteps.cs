using NFluent;
using SpecificationDemo.Data;
using SpecificationDemo.Entities;
using SpecificationDemo.Services;
using SpecificationDemoTest.Context;
using System;
using System.Threading;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SpecificationDemoTest.Steps.Blogs
{
    [Binding]
    public class SearchBlogsSteps
    {
        private readonly MockDbContext _mockDbContext;

        private DateTime _searchDateTime;
        private long _searchResult;

        public SearchBlogsSteps(MockDbContext mockDbContext)
        {
            _mockDbContext = mockDbContext;
        }

        [Given(@"I looking for blogs created after (.+)")]
        public void Given_I_looking_for_blogs_created_after(string value)
        {
            _searchDateTime = DateTime.Parse(value);
        }

        [When(@"I search for blogs")]
        public async Task When_I_search_for_blogs()
        {
            var blogRepository = new EfReadRepository<Blog>(_mockDbContext.BloggingContext);
            var specificationFactory = new SpecificationFactory();
            var service = new BlogService(specificationFactory, blogRepository);
            _searchResult = await service.GetNotExpiredBlogsAfterAsync(_searchDateTime, CancellationToken.None);
        }

        [Then(@"the count should be (.+)")]
        public void Then_the_count_should_be(long value)
        {
            Check.That(_searchResult).IsEqualTo(value);
        }
    }
}