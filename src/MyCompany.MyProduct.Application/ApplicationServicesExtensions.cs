using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MyCompany.MyProduct.Application.Behaviors;
using System.Reflection;

namespace MyCompany.MyProduct.Application;

public static class ApplicationServicesExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        ConfigureMediatR(services);
        ConfigurePipelineBehaviors(services);
    }

    private static void ConfigureMediatR(IServiceCollection services) =>
        services.AddMediatR(config =>
            config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

    private static void ConfigurePipelineBehaviors(IServiceCollection services)
    {
        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationPipelineBehavior<,>));

        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(LoggingPipelineBehavior<,>));

        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(TransactionBehavior<,>));
    }
}