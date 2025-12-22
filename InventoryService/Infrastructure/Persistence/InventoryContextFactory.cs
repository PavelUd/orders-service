using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace InventoryService.Infrastructure.Persistence;

public class InventoryContextFactory : IDesignTimeDbContextFactory< InventoryDbContext>
{
    public InventoryDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false)
            .Build();
        var databaseConnectionString = configuration.GetConnectionString("InventoryDb");
        if (string.IsNullOrWhiteSpace(databaseConnectionString))
            throw new ArgumentException("Database connection string is not initialized");
        var optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
        optionsBuilder.UseNpgsql(databaseConnectionString);
        
        return new InventoryDbContext(optionsBuilder.Options);
    }
    
}