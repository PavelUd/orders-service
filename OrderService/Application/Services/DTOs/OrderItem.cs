namespace OrderService.Application.Services.DTOs;

public class OrderItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}