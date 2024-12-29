using Application.Abstractions.Notifications;
using Application.Abstractions.Notifications.Contracts;
using Domain.Events;
using MassTransit;

namespace Infrastructure.Messaging.Consumers;

public class UserCreatedEventConsumer : IConsumer<UserRegisteredEvent>
{
    private readonly IEmailNotificationService _emailNotificationService;

    public UserCreatedEventConsumer(IEmailNotificationService emailNotificationService)
    {
        _emailNotificationService = emailNotificationService;
    }

    public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
    {
        await _emailNotificationService.SendWelcomeEmail(new WelcomeEmail(context.Message.Email, context.Message.FullName));
        await _emailNotificationService.SendEmailVerification(new EmailVerification(context.Message.Email, context.Message.FullName, context.Message.Token.ToString()));


    }
}
