namespace Application.Abstractions.Messaging;

public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent @event) where TEvent : BaseEvent;
}
