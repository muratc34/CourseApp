namespace Infrastructure.Messaging;

internal class RabbitMQEventPublisher : IEventPublisher
{
    private readonly IBus _bus;

    public RabbitMQEventPublisher(IBus bus)
    {
        _bus = bus;
    }

    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : BaseEvent
    {
        await _bus.Publish(@event);
    }
}
