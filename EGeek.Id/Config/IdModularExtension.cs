using System.Reflection;
using EGeek.Id.Infra;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EGeek.Id.Config;

public static class IdModularExtension
{
    public static void Apply(
        IServiceCollection services, 
        ConfigurationManager configuration,
        List<Assembly> mediatoRAssembly)
    {
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<IdDbContext>()
            .AddDefaultTokenProviders();

        services.AddDbContext<IdDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("IdConnection"),
                config => config.MigrationsHistoryTable("__EFMigrationsHistory", "id"));
        });
        
        mediatoRAssembly.Add(typeof(IdModularExtension).Assembly);
    }
}