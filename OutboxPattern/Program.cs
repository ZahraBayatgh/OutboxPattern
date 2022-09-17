using Microsoft.EntityFrameworkCore;
using OutboxPattern.Infrastructure;
using MediatR;
using OutboxPattern.Infrastructure.Interceptors;
using OutboxPattern.Infrastructure.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ConertDomainEventsToOutboxMessagesInterceptor>();
builder.Services.AddDbContext<OrderingDbContext>((sp, optionsBuilder) =>
{ 
    var interceptor=sp.GetService<ConertDomainEventsToOutboxMessagesInterceptor>();
    optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")).AddInterceptors(interceptor);
}
) ;
builder.Services.AddMediatR(typeof(Program));
builder.Services.AddHostedService<ProccessOutboxMessageBackgroundService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
