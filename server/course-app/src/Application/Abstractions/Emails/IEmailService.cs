namespace Application.Abstractions.Emails;

public interface IEmailService
{
    Task SendEmailAsync(MailRequest mailRequest);
}