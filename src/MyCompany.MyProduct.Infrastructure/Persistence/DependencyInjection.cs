using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyCompany.MyProduct.Infrastructure.Persistence;

internal static class DependencyInjection
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContext(services, configuration);
        AddScopedDbContext(services);
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

    private static void AddScopedDbContext(IServiceCollection services) =>
        services.AddScoped(provider => provider.GetRequiredService<ApplicationDbContext>());
}