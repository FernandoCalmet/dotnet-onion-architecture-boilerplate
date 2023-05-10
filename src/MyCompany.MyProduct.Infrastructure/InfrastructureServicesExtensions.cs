using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyCompany.MyProduct.Infrastructure.Authentication;
using MyCompany.MyProduct.Infrastructure.Common;
using MyCompany.MyProduct.Infrastructure.Emails;
using MyCompany.MyProduct.Infrastructure.Identity;
using MyCompany.MyProduct.Infrastructure.Logging;
using MyCompany.MyProduct.Infrastructure.Mapping;
using MyCompany.MyProduct.Infrastructure.Notifications;
using MyCompany.MyProduct.Infrastructure.OpenApi;
using MyCompany.MyProduct.Infrastructure.Persistence;

namespace MyCompany.MyProduct.Infrastructure;

public static class InfrastructureServicesExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddPersistence(configuration)
            .AddOpenApiDocumentation(configuration)
            .AddAuthenticationServices()
            .AddEmailServices()
            .ConfigureIdentity()
            .AddMapping()
            .AddCommonServices()
            .AddNotifications();

    public static IApplicationBuilder UseInfrastructureServices(this IApplicationBuilder app) =>
        app.UseOpenApiDocumentation();

    public static void UseLoggingServices(this WebApplicationBuilder builder) =>
        builder.RegisterSerilog();
}