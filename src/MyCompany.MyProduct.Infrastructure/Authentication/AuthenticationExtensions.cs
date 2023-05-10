using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MyCompany.MyProduct.Application.Abstractions.Authentication;

namespace MyCompany.MyProduct.Infrastructure.Authentication;

internal static class AuthenticationExtensions
{
    internal static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
    {
        services.ConfigureJwtProvider();
        services.ConfigureJwtOptions();
        services.ConfigureJwtBearerOptions();
        services.ConfigureJwtAuthentication();
        services.ConfigureHttpContextRelatedServices();

        return services;
    }

    private static void ConfigureJwtProvider(this IServiceCollection services) =>
        services.AddScoped<IJwtProvider, JwtProvider>();

    private static void ConfigureJwtOptions(this IServiceCollection services) =>
        services.ConfigureOptions<JwtOptionsSetup>();

    private static void ConfigureJwtBearerOptions(this IServiceCollection services) =>
        services.ConfigureOptions<JwtBearerOptionsSetup>();

    private static void ConfigureJwtAuthentication(this IServiceCollection services) =>
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer();

    private static void ConfigureHttpContextRelatedServices(this IServiceCollection services) =>
        services
            .AddSingleton<IUserIdentifierProvider, UserIdentifierProvider>()
            .AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
}