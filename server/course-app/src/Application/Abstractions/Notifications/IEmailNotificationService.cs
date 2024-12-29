using Application.Abstractions.Notifications.Contracts;

namespace Application.Abstractions.Notifications;

public interface IEmailNotificationService
{
    Task SendEmailVerification(EmailVerification verificationEmail);
}
