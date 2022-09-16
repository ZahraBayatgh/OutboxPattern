using MediatR;
using OutboxPattern.Domain.Events;

namespace OutboxPattern.Application.DomainEventHandlers.OrderStartedEvent
{
    public class OrderStartedDomainEventHandelr : INotificationHandler<OrderStartedDomainEvent>
    {
        public Task Handle(OrderStartedDomainEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
