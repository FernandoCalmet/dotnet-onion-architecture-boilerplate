using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyCompany.MyProduct.Infrastructure.Authentication;
using MyCompany.MyProduct.Infrastructure.Identity;
using MyCompany.MyProduct.Infrastructure.Mapping;
using MyCompany.MyProduct.Infrastructure.Persistence;

namespace MyCompany.MyProduct.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        AddPersistence(services, configuration);
        AddAuthenticationServices(services);
        ConfigureIdentity(services);
        ConfigureMapping(services);
        return services;
    }

    private static void AddPersistence(IServiceCollection services, IConfiguration configuration) => services.AddPersistence(configuration);
    private static void AddAuthenticationServices(IServiceCollection services) => services.AddAuthenticationServices();
    private static void ConfigureIdentity(IServiceCollection services) => services.ConfigureIdentity();
    private static void ConfigureMapping(IServiceCollection services) => services.AddMapping();
}