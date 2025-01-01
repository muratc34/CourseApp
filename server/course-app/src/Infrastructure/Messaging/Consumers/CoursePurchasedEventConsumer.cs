
namespace Infrastructure.Messaging.Consumers;

public class CoursePurchasedEventConsumer : IConsumer<CoursePurchasedEvent>
{
    private readonly IEmailNotificationService _emailNotificationService;

    public CoursePurchasedEventConsumer(IEmailNotificationService emailNotificationService)
    {
        _emailNotificationService = emailNotificationService;
    }

    public async Task Consume(ConsumeContext<CoursePurchasedEvent> context)
    {
        await _emailNotificationService.SendCoursePurchase(new CoursePurchase(context.Message.CourseName, context.Message.StudentName, context.Message.InstructorName, context.Message.InstructorEmail));
    }
}
