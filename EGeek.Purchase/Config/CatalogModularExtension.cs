using System.Reflection;
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
    }
}