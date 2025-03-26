using System.Linq.Expressions;

namespace GamaEdtech.Back.Domain.Common
{
    public sealed class IdentityCriteriaSpecification<T> : CriteriaSpecification<T>
    {
        public override Expression<Func<T, bool>> ToExpression()
        {
            return x => true;
        }
    }

    public abstract class CriteriaSpecification<T>
    {
        public static readonly CriteriaSpecification<T> All = new IdentityCriteriaSpecification<T>();

        public bool IsSatisfiedBy(T entity)
        {
            Func<T, bool> predicate = ToExpression().Compile();
            return predicate(entity);
        }

        public abstract Expression<Func<T, bool>> ToExpression();

        public CriteriaSpecification<T> And(CriteriaSpecification<T> specification)
        {
            return this == All
                ? specification
                : specification == All ?
                this : new AndCriteriaSpecification<T>(this, specification);
        }

        public CriteriaSpecification<T> Or(CriteriaSpecification<T> specification)
        {
            return this == All || specification == All ? All
                : new OrCriteriaSpecification<T>(this, specification);
        }

        public CriteriaSpecification<T> Not()
        {
            return new NotCriteriaSpecification<T>(this);
        }
    }

    internal sealed class AndCriteriaSpecification<T>(CriteriaSpecification<T> left, CriteriaSpecification<T> right)
        : CriteriaSpecification<T>
    {
        private readonly CriteriaSpecification<T> _left = left;
        private readonly CriteriaSpecification<T> _right = right;

        public override Expression<Func<T, bool>> ToExpression()
        {
            Expression<Func<T, bool>> leftExpression = _left.ToExpression();
            Expression<Func<T, bool>> rightExpression = _right.ToExpression();

            var invokedExpression = Expression.Invoke(rightExpression, leftExpression.Parameters);

            return (Expression<Func<T, bool>>)Expression.Lambda(Expression.AndAlso(leftExpression.Body, invokedExpression), leftExpression.Parameters);
        }
    }

    internal sealed class OrCriteriaSpecification<T>(CriteriaSpecification<T> left, CriteriaSpecification<T> right)
        : CriteriaSpecification<T>
    {
        private readonly CriteriaSpecification<T> _left = left;
        private readonly CriteriaSpecification<T> _right = right;

        public override Expression<Func<T, bool>> ToExpression()
        {
            Expression<Func<T, bool>> leftExpression = _left.ToExpression();
            Expression<Func<T, bool>> rightExpression = _right.ToExpression();

            var invokedExpression = Expression.Invoke(rightExpression, leftExpression.Parameters);

            return (Expression<Func<T, bool>>)Expression.Lambda(Expression.OrElse(leftExpression.Body, invokedExpression), leftExpression.Parameters);
        }
    }

    internal sealed class NotCriteriaSpecification<T>(CriteriaSpecification<T> specification)
        : CriteriaSpecification<T>
    {
        private readonly CriteriaSpecification<T> _specification = specification;

        public override Expression<Func<T, bool>> ToExpression()
        {
            Expression<Func<T, bool>> expression = _specification.ToExpression();
            UnaryExpression notExpression = Expression.Not(expression.Body);

            return Expression.Lambda<Func<T, bool>>(notExpression, expression.Parameters.Single());
        }
    }
}