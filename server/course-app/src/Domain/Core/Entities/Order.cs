namespace Domain.Core.Entities;

public class Order : Entity, IAuditableEntity, ISoftDeletableEntity
{
    public Order(Guid userId, string status, ICollection<Course> courses) : base()
    {
        UserId = userId;
        Status = status;
        Courses = courses;
    }
    public Order()
    {
        
    }
    public long CreatedOnUtc { get; }
    public long? ModifiedOnUtc { get; }
    public long? DeletedOnUtc { get; }
    public bool Deleted { get; }

    public Guid UserId { get; private set; }
    public string Status { get; private set; }
    public decimal TotalAmount => Courses.Sum(od => od.Price);

    public virtual User? User { get; set; }
    public virtual ICollection<Course> Courses { get; set; }

    public static Order Create(Guid userId, string status, ICollection<Course> courses)
    {
        return new Order(userId, status, courses);
    }
}
