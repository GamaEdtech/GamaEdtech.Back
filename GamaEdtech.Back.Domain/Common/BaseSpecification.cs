using Ardalis.Specification;
using GamaEdtech.Back.Domain.Entities;

namespace GamaEdtech.Back.Domain.Common
{
    public abstract class BaseSpecification<T> : Specification<T>
        where T : IEntity
    {
        protected abstract CriteriaSpecification<T> Criteria();
    }
}
