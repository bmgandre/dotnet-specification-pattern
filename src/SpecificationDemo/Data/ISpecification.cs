using System;
using System.Linq;
using System.Linq.Expressions;

namespace SpecificationDemo.Data
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Predicate { get; }

        bool IsSatisfiedBy(T entity);

        IQueryable<T> Prepare(IQueryable<T> query);

        T SatisfyingItemFrom(IQueryable<T> query);

        IQueryable<T> SatisfyingItemsFrom(IQueryable<T> query);

        ISpecification<T> Init(Expression<Func<T, bool>> expression);

        ISpecification<T> InitEmpty();

        ISpecification<T> And(ISpecification<T> specification);

        ISpecification<T> And(Expression<Func<T, bool>> right);

        ISpecification<T> Or(ISpecification<T> specification);

        ISpecification<T> Or(Expression<Func<T, bool>> right);

        ISpecification<T> Not();
    }
}