using OutboxPattern.Domain.Events;
using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;

namespace OutboxPattern.Domain
{
    public class Order : Entity
    {
        public Order(Guid customerId, string adderss, OrderStatus orderStatus)
        {
            CustomerId = customerId;
            Adderss = adderss;
            OrderStatus = orderStatus;

            var orderStartedDomainEvent = new OrderStartedDomainEvent(customerId, adderss, orderStatus);

            AddDomainEvent(orderStartedDomainEvent);
        }

        public Guid CustomerId { get; set; }
        public string Adderss { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}
