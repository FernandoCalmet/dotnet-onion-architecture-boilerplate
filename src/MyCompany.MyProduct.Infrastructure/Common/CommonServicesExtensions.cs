using Microsoft.Extensions.DependencyInjection;
using MyCompany.MyProduct.Application.Abstractions.Common;

namespace MyCompany.MyProduct.Infrastructure.Common;

internal static class CommonServicesExtensions
{
    internal static IServiceCollection AddCommonServices(this IServiceCollection services) =>
        services.AddTransient<IDateTime, MachineDateTime>();
}