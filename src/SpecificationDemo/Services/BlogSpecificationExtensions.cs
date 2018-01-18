using SpecificationDemo.Data;
using SpecificationDemo.Entities;
using System;

namespace SpecificationDemo.Specifications
{
    public static class BlogSpecificationExtensions
    {
        public static ISpecification<Blog> Activated(this ISpecification<Blog> specification)
        {
            return specification.And(x => x.Activated.HasValue);
        }

        public static ISpecification<Blog> CreatedAfter(this ISpecification<Blog> specification,
            DateTime dateTime)
        {
            return specification.And(x => x.Created >= dateTime);
        }

        public static ISpecification<Blog> NotExpired(this ISpecification<Blog> specification)
        {
            return specification.NotBanned().NotRemoved();
        }

        public static ISpecification<Blog> NotRemoved(this ISpecification<Blog> specification)
        {
            return specification.And(x => !x.Removed.HasValue);
        }

        public static ISpecification<Blog> NotBanned(this ISpecification<Blog> specification)
        {
            return specification.And(x => !x.Banned.HasValue);
        }
    }
}