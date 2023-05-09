using Microsoft.Extensions.DependencyInjection;
using MyCompany.MyProduct.Application.Abstractions.Emails;

namespace MyCompany.MyProduct.Infrastructure.Emails;

internal static class EmailServicesExtensions
{
    internal static IServiceCollection AddEmailServices(this IServiceCollection services) =>
        services.AddTransient<IEmailService, EmailService>();
}