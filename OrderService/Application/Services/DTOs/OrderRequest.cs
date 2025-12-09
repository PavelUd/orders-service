using System.Text.Json.Serialization;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using OrderService.Domain.Entities;

namespace OrderService.Application.Orders.DTOs;

public class OrderRequest
{
    [JsonIgnore]
    public Guid OrderId { get; set; }
    public Guid AccountId { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<OrderRequest, Order>();
        }
    }
}