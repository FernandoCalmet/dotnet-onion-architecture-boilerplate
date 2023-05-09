using MyCompany.MyProduct.Application;
using MyCompany.MyProduct.Infrastructure;
using MyCompany.MyProduct.Presentation;

namespace MyCompany.MyProduct.WebApi;

public static class WebApplicationExtensions
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        AddApplicationServices(builder);
        AddPresentationServices(builder);
    }

    public static void ConfigureMiddleware(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseAuthentication();
        app.MapControllers();
        app.UseInfrastructureServices();
    }

    private static void AddApplicationServices(WebApplicationBuilder builder) =>
        builder.Services
            .AddInfrastructureServices(builder.Configuration)
            .AddApplicationServices();

    private static void AddPresentationServices(WebApplicationBuilder builder) =>
        builder.Services.AddPresentationServices();
}