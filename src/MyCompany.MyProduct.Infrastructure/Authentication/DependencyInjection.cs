using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MyCompany.MyProduct.Application.Abstractions.Authentication;

namespace MyCompany.MyProduct.Infrastructure.Authentication;

internal static class DependencyInjection
{
    public static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
    {
        ConfigureJwtProvider(services);
        ConfigureJwtOptions(services);
        ConfigureJwtBearerOptions(services);
        AddAuthentication(services);
        AddHttpContextRelatedServices(services);

        return services;
    }

    private static void ConfigureJwtProvider(IServiceCollection services) =>
        services.AddScoped<IJwtProvider, JwtProvider>();

    private static void ConfigureJwtOptions(IServiceCollection services) =>
        services.ConfigureOptions<JwtOptionsSetup>();

    private static void ConfigureJwtBearerOptions(IServiceCollection services) =>
        services.ConfigureOptions<JwtBearerOptionsSetup>();

    private static void AddAuthentication(IServiceCollection services) =>
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer();

    private static void AddHttpContextRelatedServices(IServiceCollection services)
    {
        services.AddSingleton<IUserIdentifierProvider, UserIdentifierProvider>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    }
}