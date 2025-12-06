using Contracts.Events;
using OrderService.Application.Interfaces;
using OrderService.Application.Orders;
using Rebus.Bus;
using Rebus.Config;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IOrdersPublisher, OrdersPublisher>();
builder.Services.AddRebus(configure => configure
    .Transport(t => t.UseRabbitMq(builder.Configuration["Rabbit:ConnectionString"]!, "orders_queue"))
    .Options(o =>
    {
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

