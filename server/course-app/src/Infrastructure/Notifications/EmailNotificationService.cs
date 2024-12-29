using Application.Abstractions.Emails;
using Application.Abstractions.Notifications;
using Application.Abstractions.Notifications.Contracts;

namespace Infrastructure.Notifications;

internal class EmailNotificationService : IEmailNotificationService
{
    private readonly IEmailService _emailService;

    public EmailNotificationService(IEmailService emailService) => _emailService = emailService;

    public async Task SendWelcomeEmail(WelcomeEmail welcomeEmail)
    {
        var mailRequest = new MailRequest(
               welcomeEmail.EmailTo,
               "Welcome to CodeVich! 🎉",
               $"Welcome to CodeVich {welcomeEmail.Name}," +
               Environment.NewLine +
               Environment.NewLine +
               $"You have registered with the email {welcomeEmail.EmailTo}.");

        await _emailService.SendEmailAsync(mailRequest);
    }

    public async Task SendEmailVerification(EmailVerification emailVerification)
    {
        var mailRequest = new MailRequest(
               emailVerification.EmailTo,
               "Verify your email address",
               $@"Welcome to CodeVich, {emailVerification.Name}!

                Thank you for signing up on CodeVich. To complete your registration, please verify your email address by clicking the link below:

                Your verification token: {emailVerification.Token}

                If you did not sign up for an account on CodeVich, please ignore this email.

                For any questions, feel free to contact our support team at [SupportEmail].

                Welcome aboard!

                Best regards,
                The CodeVich Team
                ");

        await _emailService.SendEmailAsync(mailRequest);
    }
}
