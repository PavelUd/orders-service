using AutoMapper;
using OrderService.Domain.Entities;
using OrderService.Domain.Enums;

namespace OrderService.Application.Services.DTOs;

public class OrderDto
{
    public Guid Id { get; set; }
    
    public Guid AccountId { get; set; }
    
    public OrderStatus Status { get; set; }
    
    public List<OrderItem> Items { get; set; }

    public string Comment { get; set; } = "";
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Order, OrderDto>();
        }
    }
}