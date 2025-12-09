using System.Reflection;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using OrderService.Application.Interfaces;
using OrderService.Application.Orders.DTOs;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Persistence;

public class OrdersDbContext : DbContext, IOrderDbContext
{
    public DbSet<Order> Orders { get; set; }
    
    public  OrdersDbContext(DbContextOptions<OrdersDbContext> options)
        : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .Property(o => o.Status)
            .HasConversion<string>();
        
        modelBuilder.Entity<Order>()
            .Property(o => o.Items)
            .HasColumnType("jsonb")
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<List<OrderItem>>(v, (JsonSerializerOptions)null));
        
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    
    public void BeginTransaction()
    {
        Database.BeginTransaction();
    }

    public void CommitTransaction()
    {
        Database.CommitTransaction();
    }

    public void RollbackTransaction()
    {
        Database.RollbackTransaction();
    }
}