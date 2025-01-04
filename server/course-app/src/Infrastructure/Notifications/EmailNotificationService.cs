namespace Infrastructure.Notifications;

internal class EmailNotificationService : IEmailNotificationService
{
    private readonly IEmailService _emailService;

    public EmailNotificationService(IEmailService emailService) => _emailService = emailService;

    public async Task SendCoursePurchase(CoursePurchase coursePurchase)
    {
        var mailRequest = new MailRequest(coursePurchase.InstructorEmail, $"New Course Purchase by {coursePurchase.StudentName}",
$@"Dear {coursePurchase.InstructorName},

I hope this message finds you well. I am writing to inform you that {coursePurchase.StudentName} has recently purchased your course, {coursePurchase.CourseName}.

We are excited to see how {coursePurchase.StudentName} will benefit from your expertise and guidance. If there is any additional information or specific instructions you would like to share, please let us know.

Thank you for your dedication and commitment to teaching.

Best regards,
The Inveon Course App Team
");
        await _emailService.SendEmailAsync(mailRequest);
    }

    public async Task SendEmailVerification(EmailVerification emailVerification)
    {
        var mailRequest = new MailRequest(emailVerification.EmailTo, "Verify your email address",
$@"Welcome to Inveon Course App, {emailVerification.Name}!

Thank you for signing up on Inveon Course App. To complete your registration, please verify your e-mail address using the code below:

Your verification code: {emailVerification.Token}

If you did not sign up for an account on Inveon Course App, please ignore this email.
For any questions, feel free to contact our support team at courseapp34@gmail.com.
Welcome aboard!

Best regards,
The Inveon Course App Team
");

        await _emailService.SendEmailAsync(mailRequest);
    }
}
