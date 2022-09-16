using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using OutboxPattern.Domain;
using OutboxPattern.Infrastructure.Outbox;
using System.Text.Json;

namespace OutboxPattern.Infrastructure.Interceptors
{
    public class ConertDomainEventsToOutboxMessagesInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData,
                                                         int result,
                                                         CancellationToken cancellationToken = default)
        {
            DbContext? dbContext = eventData.Context;

            if (dbContext is null)
                return base.SavedChangesAsync(eventData, result, cancellationToken);

            var events = dbContext.ChangeTracker.Entries<Entity>().Select(x => x.Entity).SelectMany(x =>
             {
                 var domainEvents = x.DomainEvents.ToList();
                 x.ClearDomainEvents();

                 return domainEvents;
             }
             ).ToList();

            var outboxMessages = events.ConvertAll(x => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccuredOnUtc = DateTime.UtcNow,
                Type = x.GetType().Name,
                Content = JsonSerializer.Serialize(x, x.GetType(), new JsonSerializerOptions
                {
                    WriteIndented = false
                })
        });

            dbContext.Set<OutboxMessage>().AddRange(outboxMessages);
            dbContext.SaveChanges();

            return base.SavedChangesAsync(eventData, result, cancellationToken);

        }
    }
}
