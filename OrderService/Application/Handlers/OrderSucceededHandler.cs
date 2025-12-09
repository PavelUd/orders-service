using Contracts.Commands;
using OrderService.Application.Interfaces;
using OrderService.Domain.Enums;
using Rebus.Bus;
using Rebus.Handlers;

namespace OrderService.Application.Handlers;

public class OrderSucceededHandler  : IHandleMessages<OrderSucceededCommand>
{
    private readonly IOrderService _orderService;

    public OrderSucceededHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task Handle(OrderSucceededCommand message)
    {
        await _orderService.UpdateOrderStatus(message.OrderId, OrderStatus.Completed);
    }
}