using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Interfaces;
using OrderService.Application.Orders.DTOs;

namespace OrderService.API;

[Tags("Заказы")]
[ApiController]
[Route("api/orders")]
public class OrdersController : Controller
{
    private IOrdersPublisher _ordersPublisher;

    public OrdersController(IOrdersPublisher ordersPublisher)
    {
        _ordersPublisher = ordersPublisher;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrderRequest request, CancellationToken cancellationToken)
    {
        await _ordersPublisher.PublishOrderCreatedAsync(request, cancellationToken);
        return Accepted($"/order/{request.OrderId}", new { request.OrderId });
    }
}
    