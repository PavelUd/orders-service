using Contracts.Events;
using OrderService.Application.Interfaces;
using OrderService.Application.Orders.DTOs;
using Rebus.Bus;

namespace OrderService.Application.Orders;

public class OrdersPublisher : IOrdersPublisher
{
    private readonly IBus _bus;

    public OrdersPublisher(IBus bus)
    {
        _bus = bus;
    }

    public async Task PublishOrderCreatedAsync(OrderRequest request, CancellationToken cancellationToken = default)
    {
        var orderCreated = new OrderCreatedEvent(
            request.OrderId,
            request.AccountId,
            request.Items
                .Select(i => new OrderItemDto(i.ProductId, i.Quantity))
                .ToList() 
        );
        await _bus.Publish(orderCreated);
        
    }
}