using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyCompany.MyProduct.Application.Abstractions.Data;
using MyCompany.MyProduct.Infrastructure.Persistence.Identity;

namespace MyCompany.MyProduct.Infrastructure.Persistence;

internal static class PersistenceServicesExtensions
{
    internal static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureApplicationDbContext(services, configuration);
        ConfigureIdentityDbContext(services, configuration);
        ConfigureServices(services);

        return services;
    }

    private static void ConfigureApplicationDbContext(IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

    private static void ConfigureIdentityDbContext(IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContext<IdentityDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                builder => builder.MigrationsAssembly(typeof(IdentityDbContext).Assembly.FullName)));

    private static void ConfigureServices(IServiceCollection services) =>
        services
            .AddScoped(provider => provider.GetRequiredService<ApplicationDbContext>())
            .AddScoped(provider => provider.GetRequiredService<IdentityDbContext>())
            .AddScoped<IUnitOfWork, UnitOfWork>();
}