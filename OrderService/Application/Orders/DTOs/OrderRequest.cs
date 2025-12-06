namespace OrderService.Application.Orders.DTOs;

public class OrderRequest
{
    public Guid OrderId { get; set; }
    public Guid AccountId { get; set; }
    public List<OrderItem> Items { get; set; } = new();
}