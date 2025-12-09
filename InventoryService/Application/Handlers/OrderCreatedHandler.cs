using Contracts.Commands;
using Contracts.Events;
using InventoryService.Application.Interfaces;
using Rebus.Bus;
using Rebus.Handlers;

namespace InventoryService.Application.Handlers;

public class OrderCreatedHandler : IHandleMessages<ReserveItemsCommand>
{
    private readonly IInventoryService _inventoryService;
    private readonly IBus _bus;

    public OrderCreatedHandler(IInventoryService inventoryService, IBus bus)
    {
        _inventoryService = inventoryService;
        _bus = bus;
    }

    public async Task Handle(ReserveItemsCommand message)
    {
        var result = await _inventoryService.ReserveItemsAsync(message.Items);
        if (result.Succeeded)
        {
            var evt = new ItemsReservedEvent(message.OrderId, result.Data);
            await _bus.Send(evt);
        }
        else
        {
            var evt = new ItemsReservationFailedEvent(message.OrderId, string.Join(",", result.Errors));
            await _bus.Send(evt);
        }
    }
}