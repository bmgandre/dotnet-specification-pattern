using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SpecificationDemo.Data
{
    public class EfReadRepository<T> : IReadRepository<T> where T : class
    {
        private readonly DbContext _dbContext;

        public EfReadRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual T Find(params object[] keys)
        {
            var entity = _dbContext.Set<T>().Find(keys);
            return entity;
        }

        public virtual async Task<T> FindAsync(params object[] keys)
        {
            var entity = await _dbContext.Set<T>().FindAsync(keys);
            return entity;
        }

        public virtual async Task<T> FindAsync(CancellationToken cancellationToken, params object[] keys)
        {
            var entity = await _dbContext.Set<T>().FindAsync(cancellationToken, keys);
            return entity;
        }

        public virtual IEnumerable<T> Search(ISpecification<T> specification)
        {
            var query = specification.Prepare(_dbContext.Set<T>().AsQueryable());
            var result = query.ToList();
            return result;
        }

        public virtual async Task<IEnumerable<T>> SearchAsync(ISpecification<T> specification)
        {
            var result = await specification.Prepare(_dbContext.Set<T>().AsQueryable()).ToListAsync();
            return result;
        }

        public virtual async Task<IEnumerable<T>> SearchAsync(ISpecification<T> specification, CancellationToken cancellationToken)
        {
            var result = await specification.Prepare(_dbContext.Set<T>().AsQueryable()).ToListAsync(cancellationToken);
            return result;
        }

        public virtual IEnumerable<T> Search(ISpecification<T> specification, int pageNumber, int pageSize)
        {
            var entities = specification.Prepare(_dbContext.Set<T>().AsQueryable()).Skip(pageNumber).Take(pageSize).ToList();
            return entities;
        }

        public virtual async Task<IEnumerable<T>> SearchAsync(ISpecification<T> specification, int pageNumber, int pageSize)
        {
            var query = specification.Prepare(_dbContext.Set<T>().AsQueryable()).Skip(pageNumber).Take(pageSize);
            var entities = await query.ToListAsync();
            return entities;
        }

        public virtual async Task<IEnumerable<T>> SearchAsync(ISpecification<T> specification, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var query = specification.Prepare(_dbContext.Set<T>().AsQueryable()).Skip(pageNumber).Take(pageSize);
            var entities = await query.ToListAsync(cancellationToken);
            return entities;
        }

        public virtual IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            var query = _dbContext.Set<T>().Where(predicate);
            return query;
        }

        public virtual IQueryable<T> Where<TProperty>(Expression<Func<T, bool>> predicate, params Expression<Func<T, TProperty>>[] paths)
        {
            var queryable = paths
                .Aggregate(_dbContext.Set<T>().AsQueryable(), (current, include) => current.Include(include));

            return queryable.Where(predicate);
        }

        public virtual IQueryable<T> Where(ISpecification<T> specification)
        {
            return Where(specification.Predicate);
        }

        public virtual IQueryable<T> Where<TProperty>(ISpecification<T> specification, params Expression<Func<T, TProperty>>[] paths)
        {
            return Where(specification.Predicate, paths);
        }

        public virtual long Count(Expression<Func<T, bool>> predicate)
        {
            var result = _dbContext.Set<T>().LongCount(predicate);
            return result;
        }

        public virtual long Count(ISpecification<T> specification)
        {
            return Count(specification.Predicate);
        }

        public virtual async Task<long> CountAsync(Expression<Func<T, bool>> predicate)
        {
            var result = await _dbContext.Set<T>().LongCountAsync(predicate);
            return result;
        }

        public virtual async Task<long> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Set<T>().LongCountAsync(predicate, cancellationToken);
            return result;
        }

        public virtual async Task<long> CountAsync(ISpecification<T> specification)
        {
            return await CountAsync(specification.Predicate);
        }

        public virtual async Task<long> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken)
        {
            return await CountAsync(specification.Predicate, cancellationToken);
        }

        public virtual bool Exists(Expression<Func<T, bool>> predicate)
        {
            var count = Count(predicate);
            return count > 0;
        }

        public virtual bool Exists(ISpecification<T> specification)
        {
            return Exists(specification.Predicate);
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            var count = await CountAsync(predicate);
            return count > 0;
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            var count = await CountAsync(predicate, cancellationToken);
            return count > 0;
        }

        public async Task<bool> ExistsAsync(ISpecification<T> specification)
        {
            return await ExistsAsync(specification.Predicate);
        }

        public async Task<bool> ExistsAsync(ISpecification<T> specification, CancellationToken cancellationToken)
        {
            return await ExistsAsync(specification.Predicate, cancellationToken);
        }

        public virtual T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            var entity = _dbContext.Set<T>().FirstOrDefault(predicate);
            return entity;
        }

        public virtual async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public virtual T FirstOrDefault(ISpecification<T> specification)
        {
            return FirstOrDefault(specification.Predicate);
        }

        public virtual async Task<T> FirstOrDefaultAsync(ISpecification<T> specification)
        {
            return await FirstOrDefaultAsync(specification.Predicate);
        }

        public virtual async Task<T> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken)
        {
            return await FirstOrDefaultAsync(specification.Predicate, cancellationToken);
        }

        public virtual IQueryable<T> AsQueryable()
        {
            return _dbContext.Set<T>().AsQueryable();
        }
    }
}