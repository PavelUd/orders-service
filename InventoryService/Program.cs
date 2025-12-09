using System.Reflection;
using Contracts.Events;
using FluentValidation;
using InventoryService.Application.Handlers;
using InventoryService.Application.Interfaces;
using InventoryService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Rebus.Config;
using Rebus.Routing.TypeBased;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("InventoryDb")));
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<IInventoryDbContext, InventoryDbContext>();

builder.Services.AddScoped<IInventoryService, InventoryService.Application.InventoryService>();

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddRebus(configure => configure
    .Transport(t => t.UseRabbitMq(builder.Configuration["Rabbit:ConnectionString"], "inventory_queue"))
    .Options(o =>
    {
        o.LogPipeline();
        o.SetNumberOfWorkers(1);
        o.SetMaxParallelism(1);
    })
    .Routing(r => r.TypeBased()
        .Map<ItemsReservedEvent>("saga_queue")
        .Map<ItemsReservationFailedEvent>("saga_queue")));
builder.Services.AutoRegisterHandlersFromAssemblyOf<OrderCreatedHandler>();
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

