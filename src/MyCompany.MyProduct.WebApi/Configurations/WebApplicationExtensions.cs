using MyCompany.MyProduct.Application;
using MyCompany.MyProduct.Infrastructure;
using MyCompany.MyProduct.Persistence;
using MyCompany.MyProduct.Presentation;

namespace MyCompany.MyProduct.WebApi.Configurations;

internal static class WebApplicationExtensions
{
    internal static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.AddConfigurations();
        builder.AddLoggingServices();
        builder.Services.AddPresentationServices();
        builder.Services.AddApplicationServices();
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddPersistenceServices(builder.Configuration);
    }

    internal static void ConfigureMiddleware(this WebApplication app)
    {
        app.UseOpenApiServices();
        app.UsePresentationServices();
        app.UseInfrastructureServices();
    }

    private static void AddConfigurations(this WebApplicationBuilder builder)
    {
        const string configurationsDirectory = "Configurations";
        var env = builder.Environment;

        builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"{configurationsDirectory}/security.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"{configurationsDirectory}/security.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"{configurationsDirectory}/logger.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"{configurationsDirectory}/logger.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"{configurationsDirectory}/database.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"{configurationsDirectory}/database.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"{configurationsDirectory}/openapi.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"{configurationsDirectory}/openapi.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"{configurationsDirectory}/messaging.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"{configurationsDirectory}/messaging.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    }
}