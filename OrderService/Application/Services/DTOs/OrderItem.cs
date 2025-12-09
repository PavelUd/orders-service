namespace OrderService.Application.Orders.DTOs;

public class OrderItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}