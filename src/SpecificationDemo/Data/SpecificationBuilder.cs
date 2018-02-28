using System;
using System.Linq;
using System.Linq.Expressions;

namespace SpecificationDemo.Data
{
    public abstract class SpecificationBuilder<T> : ISpecification<T>
    {
        public abstract Expression<Func<T, bool>> Predicate { get; }

        protected SpecificationBuilder()
        {
        }

        public static SpecificationBuilder<T> Create()
        {
            return new NullSpecification<T>();
        }

        public bool IsSatisfiedBy(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (Predicate == null)
            {
                throw new InvalidSpecificationException("Predicate cannot be null");
            }

            var predicate = Predicate.Compile();
            return predicate(entity);
        }

        public T SatisfyingItemFrom(IQueryable<T> query)
        {
            return Prepare(query).SingleOrDefault();
        }

        public IQueryable<T> SatisfyingItemsFrom(IQueryable<T> query)
        {
            return Prepare(query);
        }

        public IQueryable<T> Prepare(IQueryable<T> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (Predicate == null)
            {
                throw new InvalidSpecificationException("Predicate cannot be null");
            }

            var q = query.Where(Predicate);
            return q;
        }

        public ISpecification<T> Init(Expression<Func<T, bool>> expression)
        {
            return new ExpressionSpecification<T>(expression);
        }

        public ISpecification<T> InitEmpty()
        {
            return new NullSpecification<T>();
        }

        public ISpecification<T> And(ISpecification<T> other)
        {
            return new AndSpecification<T>(this, other);
        }

        public ISpecification<T> And(Expression<Func<T, bool>> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return new AndSpecification<T>(this, new ExpressionSpecification<T>(other));
        }

        public ISpecification<T> Or(ISpecification<T> other)
        {
            return new OrSpecification<T>(this, other);
        }

        public ISpecification<T> Or(Expression<Func<T, bool>> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return new OrSpecification<T>(this, new ExpressionSpecification<T>(other));
        }

        public ISpecification<T> Not()
        {
            return new NotSpecification<T>(this);
        }

        protected class SwapVisitor : ExpressionVisitor
        {
            private readonly Expression from, to;

            public SwapVisitor(Expression from, Expression to)
            {
                this.from = from;
                this.to = to;
            }

            public override Expression Visit(Expression node)
            {
                return node == from ? to : base.Visit(node);
            }
        }
    }

    public class NullSpecification<T> : SpecificationBuilder<T>
    {
        public override Expression<Func<T, bool>> Predicate { get; }

        public NullSpecification()
        {
        }
    }

    public class ExpressionSpecification<T> : SpecificationBuilder<T>
    {
        private readonly Expression<Func<T, bool>> _predicate;

        public override Expression<Func<T, bool>> Predicate { get => _predicate; }

        public ExpressionSpecification(Expression<Func<T, bool>> predicate)
        {
            _predicate = predicate;
        }
    }

    public class AndSpecification<T> : SpecificationBuilder<T>
    {
        private readonly ISpecification<T> _left;
        private readonly ISpecification<T> _right;

        public override Expression<Func<T, bool>> Predicate
        {
            get
            {
                return _left.Predicate != null ? And(_left.Predicate, _right.Predicate) : _right.Predicate;
            }
        }

        public AndSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _left = left;
            _right = right ?? throw new ArgumentNullException(nameof(right));
        }

        private static Expression<Func<T, bool>> And(Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            if (left == null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right == null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            var visitor = new SwapVisitor(left.Parameters[0], right.Parameters[0]);
            var binaryExpression = Expression.AndAlso(visitor.Visit(left.Body), right.Body);
            var lambda = Expression.Lambda<Func<T, bool>>(binaryExpression, right.Parameters);
            return lambda;
        }
    }

    public class OrSpecification<T> : SpecificationBuilder<T>
    {
        private readonly ISpecification<T> _left;
        private readonly ISpecification<T> _right;

        public override Expression<Func<T, bool>> Predicate
        {
            get
            {
                return _left.Predicate != null ? Or(_left.Predicate, _right.Predicate) : _right.Predicate;
            }
        }

        public OrSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _left = left;
            _right = right ?? throw new ArgumentNullException(nameof(right));
        }

        private static Expression<Func<T, bool>> Or(Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            if (left == null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right == null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            var visitor = new SwapVisitor(left.Parameters[0], right.Parameters[0]);
            var binaryExpression = Expression.OrElse(visitor.Visit(left.Body), right.Body);
            var lambda = Expression.Lambda<Func<T, bool>>(binaryExpression, right.Parameters);
            return lambda;
        }
    }

    public class NotSpecification<T> : SpecificationBuilder<T>
    {
        private readonly ISpecification<T> _left;

        public override Expression<Func<T, bool>> Predicate
        {
            get
            {
                return Not(_left.Predicate);
            }
        }

        public NotSpecification(ISpecification<T> left)
        {
            _left = left ?? throw new ArgumentNullException(nameof(left));
        }

        private static Expression<Func<T, bool>> Not(Expression<Func<T, bool>> left)
        {
            if (left == null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            var notExpression = Expression.Not(left.Body);
            var lambda = Expression.Lambda<Func<T, bool>>(notExpression, left.Parameters.Single());
            return lambda;
        }
    }
}