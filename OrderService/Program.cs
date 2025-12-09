using Contracts.Events;
using OrderService;
using OrderService.Application;
using OrderService.Application.Interfaces;
using OrderService.Application.Orders;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Routing.TypeBased;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IOrdersPublisher, OrdersPublisher>();
builder.Services.AddRebus(configure => configure
    .Routing(r => r.TypeBased().Map<OrderCreatedEvent>("order_queue"))
    .Transport(t => t.UseRabbitMq(builder.Configuration["Rabbit:ConnectionString"]!, "orderh_queue"))
    .Options(o =>
    {
        o.LogPipeline();
        o.SetNumberOfWorkers(1);
        o.SetMaxParallelism(1);
    })
);


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

