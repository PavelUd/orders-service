using Contracts.Commands;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using SagaCoordinator;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRebus(
    configure => configure
        .Transport(t => 
            t.UseRabbitMq(builder.Configuration["Rabbit:ConnectionString"], "order_queue"))
        .Routing(r => r.TypeBased().Map<ReserveItemsCommand>("inventory_queue"))
        .Sagas(s => s.StoreInPostgres(
            builder.Configuration.GetConnectionString("RebusSql"),
            dataTableName: "RebusSagaData",
            indexTableName: "RebusSagaIndex",
            automaticallyCreateTables: true))
        .Options(o =>
        {
            
            o.LogPipeline();
            o.SetNumberOfWorkers(1);
            o.SetMaxParallelism(1);
            
        })
       ,
    isDefaultBus: true
);

builder.Services.AutoRegisterHandlersFromAssemblyOf<OrderSaga>();
var app = builder.Build();
app.MapGet("/", () => "Saga Coordinator is running");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Run();
