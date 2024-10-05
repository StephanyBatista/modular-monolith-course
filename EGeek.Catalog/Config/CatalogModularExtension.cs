using System.Reflection;
using EGeek.Catalog.Infra;
using EGeek.Catalog.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EGeek.Catalog.Config;

public static class CatalogModularExtension
{
    public static void Apply(
        IServiceCollection services,
        ConfigurationManager configuration,
        List<Assembly> mediatoRAssembly)
    {
        services.AddDbContext<CatalogDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("CatalogConnection"), 
                config => config.MigrationsHistoryTable("__EFMigrationsHistory", "catalog"));
        });
        mediatoRAssembly.Add(typeof(CatalogModularExtension).Assembly);
        services.AddScoped<RoleValidator>();
    }
}