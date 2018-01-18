using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SpecificationDemo.Data
{
    public interface IReadRepository<T>
    {
        T Find(params object[] keys);

        Task<T> FindAsync(params object[] keys);

        Task<T> FindAsync(CancellationToken cancellationToken, params object[] keys);

        IEnumerable<T> Search(ISpecification<T> specification);

        Task<IEnumerable<T>> SearchAsync(ISpecification<T> specification);

        Task<IEnumerable<T>> SearchAsync(ISpecification<T> specification, CancellationToken cancellationToken);

        IEnumerable<T> Search(ISpecification<T> specification, int pageNumber, int pageSize);

        Task<IEnumerable<T>> SearchAsync(ISpecification<T> specification, int pageNumber, int pageSize);

        Task<IEnumerable<T>> SearchAsync(ISpecification<T> specification, int pageNumber, int pageSize, CancellationToken cancellationToken);

        IQueryable<T> Where(Expression<Func<T, bool>> predicate);

        IQueryable<T> Where(ISpecification<T> specification);

        IQueryable<T> Where<TProperty>(Expression<Func<T, bool>> predicate, params Expression<Func<T, TProperty>>[] paths);

        long Count(Expression<Func<T, bool>> predicate);

        long Count(ISpecification<T> specification);

        Task<long> CountAsync(Expression<Func<T, bool>> predicate);

        Task<long> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);

        Task<long> CountAsync(ISpecification<T> specification);

        Task<long> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken);

        bool Exists(Expression<Func<T, bool>> predicate);

        bool Exists(ISpecification<T> specification);

        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);

        Task<bool> ExistsAsync(ISpecification<T> specification);

        Task<bool> ExistsAsync(ISpecification<T> specification, CancellationToken cancellationToken);

        T FirstOrDefault(Expression<Func<T, bool>> predicate);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);

        T FirstOrDefault(ISpecification<T> specification);

        Task<T> FirstOrDefaultAsync(ISpecification<T> specification);

        Task<T> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken);

        IQueryable<T> AsQueryable();
    }
}