using MyCompany.MyProduct.Application;
using MyCompany.MyProduct.Infrastructure;
using MyCompany.MyProduct.Presentation;

namespace MyCompany.MyProduct.WebApi;

public static class WebApplicationExtensions
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddApplicationServices();
        builder.Services.AddPresentationServices();
        builder.AddLoggingServices();
    }

    public static void ConfigureMiddleware(this WebApplication app)
    {
        app.UsePresentationServices();
        app.UseInfrastructureServices();
        app.UseOpenApiServices();
    }
}