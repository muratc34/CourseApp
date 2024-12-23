namespace Domain.Core.Abstractions;

public interface IAuditableEntity
{
    long CreatedOnUtc { get; }
    long? ModifiedOnUtc { get; }
}
