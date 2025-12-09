using OrderService.Application.Common;
using OrderService.Application.Orders.DTOs;
using OrderService.Domain.Enums;

namespace OrderService.Application.Interfaces;

public interface IOrderService
{
    public Task<Result<None>> UpdateOrderStatus(Guid orderId, OrderStatus orderStatus, string comment = "");

    public  Task<Result<Guid>> CreateOrder(OrderRequest request);

}