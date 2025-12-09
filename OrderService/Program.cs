using System.Reflection;
using Contracts.Events;
using Microsoft.EntityFrameworkCore;
using OrderService;
using OrderService.Application;
using OrderService.Application.Handlers;
using OrderService.Application.Interfaces;
using OrderService.Application.Orders;
using OrderService.Application.Services;
using OrderService.Infrastructure.Persistence;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Routing.TypeBased;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IOrdersPublisher, OrdersPublisher>();
builder.Services.AddScoped<IOrderService, OrdersService>();

builder.Services.AddDbContext<OrdersDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("OrderDb")));
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<IOrderDbContext, OrdersDbContext>();

builder.Services.AddRebus(configure => configure
    .Routing(r => r.TypeBased().Map<OrderCreatedEvent>("saga_queue"))
    .Transport(t => t.UseRabbitMq(builder.Configuration["Rabbit:ConnectionString"]!, "order_queue"))
    .Options(o =>
    {
        o.LogPipeline();
        o.SetNumberOfWorkers(1);
        o.SetMaxParallelism(1);
    })
);
builder.Services.AddScoped<IOrderService, OrdersService>();
builder.Services.AutoRegisterHandlersFromAssemblyOf<OrderSucceededHandler>();
builder.Services.AutoRegisterHandlersFromAssemblyOf<CancelOrderHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseRouting();
app.UseHttpsRedirection();
app.Run();

