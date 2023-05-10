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

namespace MyCompany.MyProduct.Infrastructure;

public static class InfrastructureServicesExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddOpenApiDocumentationServices(configuration)
            .AddAuthenticationServices()
            .AddEmailServices()
            .AddIdentityServices()
            .AddMappingServices()
            .AddCommonServices()
            .AddNotificationServices();

    public static void AddLoggingServices(this WebApplicationBuilder builder) =>
        builder.RegisterSerilog();

    public static void UseInfrastructureServices(this WebApplication app) =>
        app
            .UseAuthorization()
            .UseAuthentication()
            .UseHttpsRedirection();

    public static void UseOpenApiServices(this IApplicationBuilder builder) =>
        builder.UseOpenApiDocumentation();
}