using MediatR;
using OutboxPattern.Domain;

namespace OutboxPattern.Application.Commands
{
    public class CreateOrderConnand : IRequest
    {
        public Guid CustomerId { get; set; }
        public string Adderss { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}
