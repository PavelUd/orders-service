using AutoMapper;
using OrderService.Application.Common;
using OrderService.Application.Interfaces;
using OrderService.Application.Orders.DTOs;
using OrderService.Domain.Entities;
using OrderService.Domain.Enums;

namespace OrderService.Application.Orders;

public class OrdersService : IOrderService
{
    private readonly IOrderDbContext _orderDbContext;
    private readonly IMapper _mapper;

    public OrdersService(IOrderDbContext orderDbContext, IMapper mapper)
    {
        _orderDbContext = orderDbContext;
        _mapper = mapper;
    }
    
    public async Task<Result<None>> UpdateOrderStatus(Guid orderId, OrderStatus orderStatus)
    {
        try
        {
            var order = _orderDbContext.Orders.Find(orderId);
            if (order == null)
            {
                return await Result<None>.FailureAsync("Order not found");
            }

            order.Status = orderStatus;
            await _orderDbContext.SaveChangesAsync();
            return await Result<None>.SuccessAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return await Result<None>.FailureAsync(ex.Message);
        }
    }

    public async Task<Result<Guid>> CreateOrder(OrderRequest request)
    {
        try
        {
            var order = _mapper.Map<Order>(request);
            _orderDbContext.Orders.Add(order);
            await _orderDbContext.SaveChangesAsync();
            
            return await Result<Guid>.SuccessAsync(order.Id);
        }
        catch (Exception ex)
        {
            return await Result<Guid>.FailureAsync(ex.Message);
        }
    }
}