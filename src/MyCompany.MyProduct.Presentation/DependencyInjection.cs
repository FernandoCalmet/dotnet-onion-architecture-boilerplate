using Microsoft.Extensions.DependencyInjection;

namespace MyCompany.MyProduct.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        ConfigureServices(services);
        return services;
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        AddControllers(services);
    }

    private static void AddControllers(IServiceCollection services)
    {
        services
            .AddControllers()
            .AddApplicationPart(PresentationAssembly.Assembly);
    }
}