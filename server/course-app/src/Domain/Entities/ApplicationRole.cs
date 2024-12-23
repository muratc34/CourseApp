namespace Domain.Entities;

public class ApplicationRole : IdentityRole<Guid>, IAuditableEntity, ISoftDeletableEntity
{
    public ApplicationRole(string name) : base(name)
    {
        Ensure.NotNull(name, "The name is required.", nameof(name));
    }

    public long CreatedOnUtc { get; }
    public long? ModifiedOnUtc { get; }
    public long? DeletedOnUtc { get; }
    public bool Deleted { get; }

    public static ApplicationRole Create(string roleName)
    {
        return new ApplicationRole(roleName);
    }
}
