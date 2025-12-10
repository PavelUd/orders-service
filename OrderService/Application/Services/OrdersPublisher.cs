using Contracts.Events;
using OrderService.Application.Interfaces;
using OrderService.Application.Services.DTOs;
using Rebus.Bus;

namespace OrderService.Application.Services;

public class OrdersPublisher : IOrdersPublisher
{
    private readonly IBus _bus;
    private readonly ILogger<OrdersPublisher> _logger;

    public OrdersPublisher(IBus bus, ILogger<OrdersPublisher> logger)
    {
        _bus = bus;
        _logger = logger;
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
        
        _logger.LogInformation("Publishing order created: {@orderCreated}", orderCreated);
        await _bus.Send(orderCreated);
        
    }
}