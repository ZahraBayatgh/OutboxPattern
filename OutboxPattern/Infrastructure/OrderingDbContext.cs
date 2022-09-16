using Microsoft.EntityFrameworkCore;
using OutboxPattern.Domain;
using OutboxPattern.Infrastructure.Outbox;

namespace OutboxPattern.Infrastructure
{
    public class OrderingDbContext:DbContext
    {
        public OrderingDbContext(DbContextOptions<OrderingDbContext> options):base(options)
        {

        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OutboxMessage>  OutboxMessages { get; set; }
    }
}
