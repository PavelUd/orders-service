using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OrderService.Infrastructure.Persistence;

public class OrdersContextFactory : IDesignTimeDbContextFactory<OrdersDbContext>
{
    public OrdersDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false)
            .Build();
        var databaseConnectionString = configuration.GetConnectionString("OrderDb");
        if (string.IsNullOrWhiteSpace(databaseConnectionString))
            throw new ArgumentException("Database connection string is not initialized");
        var optionsBuilder = new DbContextOptionsBuilder<OrdersDbContext>();
        optionsBuilder.UseNpgsql(databaseConnectionString);

        return new OrdersDbContext(optionsBuilder.Options);
    }
    
}