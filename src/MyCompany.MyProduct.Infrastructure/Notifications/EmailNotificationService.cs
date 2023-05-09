using MyCompany.MyProduct.Application.Abstractions.Emails;
using MyCompany.MyProduct.Application.Abstractions.Notifications;

namespace MyCompany.MyProduct.Infrastructure.Notifications;

internal sealed class EmailNotificationService : IEmailNotificationService
{
    private readonly IEmailService _emailService;

    public EmailNotificationService(IEmailService emailService)
    {
        _emailService = emailService;
    }
}