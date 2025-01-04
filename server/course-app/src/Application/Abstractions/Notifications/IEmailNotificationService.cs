namespace Application.Abstractions.Notifications;

public interface IEmailNotificationService
{
    Task SendEmailVerification(EmailVerification verificationEmail);
    Task SendCoursePurchase(CoursePurchase coursePurchase); 
}
