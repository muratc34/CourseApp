namespace Domain.Core.Abstractions;

public interface ISoftDeletableEntity
{
    long? DeletedOnUtc { get; }
    bool Deleted { get; }
}