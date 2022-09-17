using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OutboxPattern.Domain;
using OutboxPattern.Domain.Events;
using System.Reflection;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace OutboxPattern.Infrastructure.BackgroundServices
{
    public class ProccessOutboxMessageBackgroundService : BackgroundService
    {
        public IServiceProvider Services { get; }

        public ProccessOutboxMessageBackgroundService(IServiceProvider services)
        {
            Services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = Services.CreateScope())
                {
                    var orderingDbContext = scope.ServiceProvider.GetRequiredService<OrderingDbContext>();

                    var messages = await orderingDbContext.OutboxMessages.Where(x => x.ProcessedOnUtc == null).Take(20).ToListAsync();
                    var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

                    foreach (var message in messages)
                    {
                        try
                        {
                            //Assembly assembly = typeof(OrderStartedDomainEvent).Assembly;
                            //Type type = assembly.GetType(message.Type);

                            //var domainEvent = JsonSerializer.Deserialize(message.Content, type, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                            INotification? domainEvent = JsonConvert.DeserializeObject<INotification>(message.Content, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                            if (domainEvent is null)
                                continue;

                            await publisher.Publish(domainEvent, stoppingToken);

                            message.ProcessedOnUtc = DateTime.UtcNow;
                            await orderingDbContext.SaveChangesAsync();
                        }
                        catch
                        {
                            message.ProcessedOnUtc = null;
                            await orderingDbContext.SaveChangesAsync();
                        }
                    }

                    await Task.Delay(10000, stoppingToken);
                }
            }
        }
    }
}
