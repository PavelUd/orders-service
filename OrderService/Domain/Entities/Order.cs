using System.ComponentModel.DataAnnotations.Schema;
using OrderService.Application.Services.DTOs;
using OrderService.Domain.Common;
using OrderService.Domain.Enums;

namespace OrderService.Domain.Entities;

[Table("orders")]
public class Order : BaseEntity
{
   [Column("account_id")]
   public Guid AccountId { get; set; }
   
   [Column("status")]
   public OrderStatus Status { get; set; }
   
   [Column("items")]
   public List<OrderItem> Items { get; set; }

   [Column("comment")] public string Comment { get; set; } = "";

}