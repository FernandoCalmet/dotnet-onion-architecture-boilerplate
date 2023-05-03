using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MyCompany.MyProduct.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        ConfigureMediatr(services);
        return services;
    }

    private static void ConfigureMediatr(IServiceCollection services)
    {
        services.AddMediatR(config =>
            config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
    }
}