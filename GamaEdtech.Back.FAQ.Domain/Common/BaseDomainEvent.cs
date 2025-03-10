using MediatR;

namespace GamaEdtech.Back.FAQ.Domain.Common
{
    public interface IDomainEvent : INotification
    {
        public Guid EventId { get; }
        public string Route { get; }
    }
}