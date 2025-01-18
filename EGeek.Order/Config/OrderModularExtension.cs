using System.Reflection;
using EGeek.Order.Infra.Database;
using EGeek.Order.PurchaseOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EGeek.Order.Config;

public static class OrderModularExtension
{
    public static void Apply(
        IServiceCollection services,
        ConfigurationManager configuration,
        List<Assembly> mediatoRAssembly)
    {
        services.AddDbContext<OrderDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("OrderConnection"), 
                config => config.MigrationsHistoryTable("__EFMigrationsHistory", "order"));
        });
        mediatoRAssembly.Add((Assembly)typeof(OrderModularExtension).Assembly);
        services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
    }
}