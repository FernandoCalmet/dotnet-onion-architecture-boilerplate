﻿using Microsoft.Extensions.DependencyInjection;
using MyCompany.MyProduct.Application.Abstractions.Notifications;

namespace MyCompany.MyProduct.Infrastructure.Notifications;

internal static class NotificationExtensions
{
    internal static IServiceCollection AddNotifications(this IServiceCollection services) =>
        services.AddScoped<IEmailNotificationService, EmailNotificationService>();
}