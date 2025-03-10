using GamaEdtech.Back.FAQ.Domain.Entities;

namespace GamaEdtech.Back.FAQ.Domain.Common
{
    public interface IAggregateRoot : IEntity
    {
        void ClearDomainEvents();
        IReadOnlyList<IDomainEvent> DomainEvents { get; }
    }
}