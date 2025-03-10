using Ardalis.Specification;
using GamaEdtech.Back.FAQ.Domain.Entities;

namespace GamaEdtech.Back.FAQ.Domain.Common
{
    public abstract class BaseSpecification<T> : Specification<T>
        where T : IEntity
    {
        protected abstract CriteriaSpecification<T> Criteria();
    }
}
