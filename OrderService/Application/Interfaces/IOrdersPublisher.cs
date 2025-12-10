using OrderService.Application.Services.DTOs;

namespace OrderService.Application.Interfaces;

public interface IOrdersPublisher
{
    public Task PublishOrderCreatedAsync(OrderRequest request, CancellationToken cancellationToken);
}