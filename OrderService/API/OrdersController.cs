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
    private readonly IOrderService _orderService;

    public OrdersController(IOrdersPublisher ordersPublisher, IOrderService orderService)
    {
        _ordersPublisher = ordersPublisher;
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrderRequest request, CancellationToken cancellationToken)
    {
       var idResult = await _orderService.CreateOrder(request);
       if (!idResult.Succeeded)
       {
           return BadRequest(idResult.Errors);
       }
       request.OrderId = idResult.Data;
       await _ordersPublisher.PublishOrderCreatedAsync(request, cancellationToken);
       return Accepted($"/order/{idResult.Data}", new { idResult.Data });
    }
}
    