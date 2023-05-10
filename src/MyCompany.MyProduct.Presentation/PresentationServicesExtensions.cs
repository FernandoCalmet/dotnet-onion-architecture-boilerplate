using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MyCompany.MyProduct.Presentation.Middlewares;

namespace MyCompany.MyProduct.Presentation;

public static class PresentationServicesExtensions
{
    public static void AddPresentationServices(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddApplicationPart(PresentationAssembly.Assembly);

        services.AddTransient<GlobalExceptionHandlingMiddleware>();
    }

    public static void UsePresentationServices(this WebApplication app)
    {
        app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
        app.MapControllers();
    }
}