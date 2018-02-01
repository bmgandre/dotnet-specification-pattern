using SpecificationDemo.Data;
using SpecificationDemo.Entities;
using SpecificationDemo.Specifications;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SpecificationDemo.Services
{
    public class BlogService
    {
        private readonly IReadRepository<Blog> _blogRepository;
        private readonly ISpecificationFactory _specificationFactory;

        public BlogService(ISpecificationFactory  specificationFactory,
            IReadRepository<Blog> repository)
        {
            _blogRepository = repository;
            _specificationFactory = specificationFactory;
        }

        public async Task<long> GetNotExpiredBlogsAfterAsync(DateTime dateTime, CancellationToken cancellationToken)
        {
            var specification = _specificationFactory.Create<Blog>()
                .NotExpired()
                .CreatedAfter(dateTime);

            var result = await _blogRepository
                .CountAsync(specification, cancellationToken);

            return result;
        }
    }
}