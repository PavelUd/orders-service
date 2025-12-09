using Contracts.Commands;
using Contracts.Events;
using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Sagas;

namespace SagaCoordinator;

public class OrderSaga : Saga<OrderSagaData>,
    IAmInitiatedBy<OrderCreatedEvent>,
    IHandleMessages<ItemsReservedEvent>,
    IHandleMessages<ItemsReservationFailedEvent>
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

    public async Task Handle(ItemsReservedEvent message)
    {
        Data.TotalPrice = message.TotalPrice;
        
        await _bus.Send(new OrderSucceededCommand(message.OrderId));
    }

    public async Task Handle(ItemsReservationFailedEvent message)
    {
        await _bus.Send(new CancelOrderCommand(message.OrderId, message.Reason));
    }
    
    

    protected override void CorrelateMessages(ICorrelationConfig<OrderSagaData> config)
    {
        config.Correlate<OrderCreatedEvent>(msg => msg.OrderId, data => data.OrderId);
        config.Correlate<ItemsReservedEvent>(msg => msg.OrderId, data => data.OrderId);
        config.Correlate<ItemsReservationFailedEvent>(msg => msg.OrderId, data => data.OrderId);
    }
} 