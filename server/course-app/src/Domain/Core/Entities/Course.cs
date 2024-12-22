namespace Domain.Core.Entities;

public class Course : Entity, IAuditableEntity, ISoftDeletableEntity
{
    public Course(string name, string description, decimal price, string? imageUrl, Guid categoryId, Guid instructorId) : base()
    {
        Name = name;
        Description = description;
        Price = price;
        CategoryId = categoryId;
        ImageUrl = imageUrl;
        InstructorId = instructorId;
    }
    public Course()
    {
        
    }

    public long CreatedOnUtc { get; }
    public long? ModifiedOnUtc { get; }
    public long? DeletedOnUtc { get; }
    public bool Deleted { get; }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public string? ImageUrl { get; private set; }

    public Guid CategoryId { get; private set; }
    public Guid InstructorId { get; private set; }
    public virtual Category? Category { get; private set; }
    public virtual User? Instructor { get; private set; }
    public virtual ICollection<User>? EnrolledUsers { get; private set; }
    // Buraya bakacağım
    public virtual ICollection<Order>? Orders { get; private set; }

    public static Course Create(string name, string description, decimal price, string? imageUrl, Guid categoryId, Guid instructorId)
    {
        return new Course(name, description, price, imageUrl, categoryId, instructorId);
    }
}
