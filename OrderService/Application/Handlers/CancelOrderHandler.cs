using Contracts.Commands;
using OrderService.Application.Interfaces;
using OrderService.Domain.Enums;
using Rebus.Bus;
using Rebus.Handlers;

namespace OrderService.Application.Handlers;

public class CancelOrderHandler : IHandleMessages<CancelOrderCommand>
{
    private readonly IOrderService _orderService;
    private readonly IBus _bus;

    public CancelOrderHandler(IOrderService orderService, IBus bus)
    {
        _orderService = orderService;
        _bus = bus;
    }

    public async Task Handle(CancelOrderCommand message)
    {
        await _orderService.UpdateOrderStatus(message.OrderId, OrderStatus.Failed, message.Reason);
    }
}