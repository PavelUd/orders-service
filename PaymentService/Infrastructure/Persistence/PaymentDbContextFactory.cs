using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PaymentService.Infrastructure.Persistence;

public class PaymentDbContextFactory : IDesignTimeDbContextFactory<PaymentDbContext>
{
    public PaymentDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false)
            .Build();
        var databaseConnectionString = configuration.GetConnectionString("PaymentDb");
        if (string.IsNullOrWhiteSpace(databaseConnectionString))
            throw new ArgumentException("Database connection string is not initialized");
        var optionsBuilder = new DbContextOptionsBuilder<PaymentDbContext>();
        optionsBuilder.UseNpgsql(databaseConnectionString);

        return new PaymentDbContext(optionsBuilder.Options);
    }

}
