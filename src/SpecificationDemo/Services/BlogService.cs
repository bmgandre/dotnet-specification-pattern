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
        private readonly EfReadRepository<Blog> _blogRepository;

        public BlogService(EfReadRepository<Blog> repository)
        {
            _blogRepository = repository;
        }

        public async Task<long> GetNotExpiredBlogsAfterAsync(DateTime dateTime, CancellationToken cancellationToken)
        {
            var specification = SpecificationBuilder<Blog>.Create()
                .NotExpired()
                .CreatedAfter(dateTime);

            var result = await _blogRepository
                .CountAsync(specification, cancellationToken);

            return result;
        }
    }
}