using Contracts.Commands;
using Contracts.Events;
using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Sagas;

namespace SagaCoordinator;

public class OrderSaga : Saga<OrderSagaData>,
    IAmInitiatedBy<OrderCreatedEvent>,
    IHandleMessages<ItemsReservedEvent>
{
    private readonly IBus _bus;

    public OrderSaga(IBus bus)
    {
        _bus = bus;
    }
    
    public async Task Handle(OrderCreatedEvent message)
    {
        Data.OrderId = message.OrderId;

        await _bus.Send(new ReserveItemsCommand(message.OrderId, message.Items));
    }

    public Task Handle(ItemsReservedEvent message)
    {
        Data.TotalPrice = message.TotalPrice;
        MarkAsComplete();

        return Task.CompletedTask;
    }

    protected override void CorrelateMessages(ICorrelationConfig<OrderSagaData> config)
    {
        config.Correlate<OrderCreatedEvent>(msg => msg.OrderId, data => data.OrderId);
        config.Correlate<ItemsReservedEvent>(msg => msg.OrderId, data => data.OrderId);
    }
} 