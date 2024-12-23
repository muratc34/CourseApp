namespace Domain.Entities;

public class Category : Entity, IAuditableEntity, ISoftDeletableEntity
{
    public Category(string name) : base()
    {
        Ensure.NotEmpty(name, "The name is required", nameof(name));

        Name = name;
    }
    public Category()
    {

    }
    public long CreatedOnUtc { get; }
    public long? ModifiedOnUtc { get; }
    public long? DeletedOnUtc { get; }
    public bool Deleted { get; }

    public string Name { get; private set; }

    public virtual ICollection<Course>? Courses { get; private set; }

    public static Category Create(string name)
    {
        return new Category(name);
    }
}
