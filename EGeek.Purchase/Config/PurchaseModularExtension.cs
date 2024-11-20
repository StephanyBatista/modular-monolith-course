using System.Reflection;
using EGeek.Purchase.Checkout;
using EGeek.Purchase.Infra.Database;
using EGeek.Purchase.Infra.Payment;
using EGeek.Purchase.Infra.ShippingCost;
using EGeek.Purchase.ShoppingCarts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EGeek.Purchase.Config;

public static class PurchaseModularExtension
{
    public static void Apply(
        IServiceCollection services,
        ConfigurationManager configuration,
        List<Assembly> mediatoRAssembly)
    {
        services.AddDbContext<PurchaseDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("PurchaseConnection"), 
                config => config.MigrationsHistoryTable("__EFMigrationsHistory", "purchase"));
        });
        mediatoRAssembly.Add(typeof(PurchaseModularExtension).Assembly);
        services.AddScoped<IShippingCost, ShippingCostApi>();
        services.AddScoped<IPayment, PaymentApi>();
        services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
    }
}