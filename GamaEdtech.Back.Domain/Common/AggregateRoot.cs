using GamaEdtech.Back.Domain.Entities;
using System.Text.Json.Serialization;

namespace GamaEdtech.Back.Domain.Common
{
    public abstract class AggregateRoot<T> : BaseEntity<T>, IAggregateRoot, IEntity
    {
        protected AggregateRoot() : base()
        {
            _domainEvents = [];
        }

        [JsonIgnore]
        private readonly List<IDomainEvent> _domainEvents;

        [JsonIgnore]
        public IReadOnlyList<IDomainEvent> DomainEvents
        {
            get { return _domainEvents; }
        }

        protected void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            if (domainEvent == null)
                return;
            _domainEvents?.Add(domainEvent);
        }

        protected void RemoveDomainEvent(IDomainEvent domainEvent)
        {
            if (domainEvent == null)
                return;
            _domainEvents?.Remove(domainEvent);
        }

        public void ClearDomainEvents() => _domainEvents?.Clear();
    }

    public abstract class AggregateRoot : AggregateRoot<Guid>
    {

    }
}