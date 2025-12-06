using OrderService.Application.Orders.DTOs;

namespace OrderService.Application.Interfaces;

public interface IOrdersPublisher
{
    public Task PublishOrderCreatedAsync(OrderRequest request, CancellationToken cancellationToken);
}