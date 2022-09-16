using MediatR;

namespace OutboxPattern.Domain.Events
{
    public class OrderStartedDomainEvent : INotification
    {
        public OrderStartedDomainEvent(Guid customerId, string adderss, OrderStatus orderStatus)
        {
            CustomerId = customerId;
            Adderss = adderss;
            OrderStatus = orderStatus;
        }

        public Guid CustomerId { get; set; }
        public string Adderss { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}
