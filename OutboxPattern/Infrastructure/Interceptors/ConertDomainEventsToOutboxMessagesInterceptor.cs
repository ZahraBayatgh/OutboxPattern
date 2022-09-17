using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using OutboxPattern.Domain;
using OutboxPattern.Infrastructure.Outbox;
using System.Text.Json;

namespace OutboxPattern.Infrastructure.Interceptors
{
    public class ConertDomainEventsToOutboxMessagesInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            DbContext? dbContext = eventData.Context;

            if (dbContext is null)
                return base.SavingChangesAsync(eventData, result, cancellationToken);

            List<INotification> events = dbContext.ChangeTracker.Entries<Entity>().Select(x => x.Entity).SelectMany(x =>
            {
                var domainEvents = x.DomainEvents().ToList();
                x.ClearDomainEvents();

                return domainEvents;
            }
             ).ToList();

            List<OutboxMessage> outboxMessages = events.ConvertAll(x => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccuredOnUtc = DateTime.UtcNow,
                Type = x.GetType().FullName,
                //Content = JsonSerializer.Serialize(x, x.GetType(), new JsonSerializerOptions { WriteIndented = false })
                Content = JsonConvert.SerializeObject(x,new JsonSerializerSettings {TypeNameHandling= TypeNameHandling.All })
            });

            dbContext.Set<OutboxMessage>().AddRange(outboxMessages);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
