namespace Domain.Core.Abstractions;

public abstract class BaseEvent
{
    protected BaseEvent()
    {
        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    public long CreatedAt { get; private set; }
    
}
