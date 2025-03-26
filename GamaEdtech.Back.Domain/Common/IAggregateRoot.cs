using GamaEdtech.Back.Domain.Entities;

namespace GamaEdtech.Back.Domain.Common
{
    public interface IAggregateRoot : IEntity
    {
        void ClearDomainEvents();
        IReadOnlyList<IDomainEvent> DomainEvents { get; }
    }
}