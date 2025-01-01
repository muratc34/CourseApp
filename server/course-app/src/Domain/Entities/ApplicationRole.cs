namespace Domain.Entities;

public class ApplicationRole : IdentityRole<Guid>, IAuditableEntity
{
    public ApplicationRole(string name) : base(name)
    {
        Ensure.NotNull(name, "The name is required.", nameof(name));
    }
    public ApplicationRole()
    {
    }

    public long CreatedOnUtc { get; }
    public long? ModifiedOnUtc { get; }

    public static ApplicationRole Create(string roleName)
    {
        return new ApplicationRole(roleName);
    }
}
