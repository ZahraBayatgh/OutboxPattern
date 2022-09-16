using MediatR;
using OutboxPattern.Domain;
using OutboxPattern.Infrastructure;

namespace OutboxPattern.Application.Commands
{
    public class CreateOrderConnandHandler : IRequestHandler<CreateOrderConnand>
    {
        private readonly OrderingDbContext _orderingDbContext;

        public CreateOrderConnandHandler(OrderingDbContext orderingDbContext)
        {
            _orderingDbContext = orderingDbContext;
        }
        public async Task<Unit> Handle(CreateOrderConnand request, CancellationToken cancellationToken)
        {
            Order order = new(request.CustomerId, request.Adderss, request.OrderStatus);

            _orderingDbContext.Orders.Add(order);

           await _orderingDbContext.SaveChangesAsync();

            return default;
        }
    }
}
