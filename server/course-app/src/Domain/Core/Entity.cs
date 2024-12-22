namespace Domain.Core;

public class Entity : IEntity
{
    public Guid Id { get; set; }

    protected Entity()
    {
        Id = Guid.NewGuid();
    }
}