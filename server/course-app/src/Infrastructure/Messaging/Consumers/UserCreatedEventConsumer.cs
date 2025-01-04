namespace Infrastructure.Messaging.Consumers;

public class UserCreatedEventConsumer : IConsumer<UserRegisteredEvent>
{
    private readonly IEmailNotificationService _emailNotificationService;
    private readonly ICacheService _cacheService;

    public UserCreatedEventConsumer(IEmailNotificationService emailNotificationService, ICacheService cacheService)
    {
        _emailNotificationService = emailNotificationService;
        _cacheService = cacheService;
    }

    public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
    {
        await _emailNotificationService.SendEmailVerification(new EmailVerification(context.Message.Email, context.Message.FullName, context.Message.Token.ToString()));
        await _cacheService.SetAsync(CachingKeys.EmailVerificationKey(context.Message.UserId), context.Message.Token.ToString(), TimeSpan.FromMinutes(5));
    }
}
