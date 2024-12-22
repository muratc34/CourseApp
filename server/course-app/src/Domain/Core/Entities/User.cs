namespace Domain.Core.Entities;

public class User : Entity, IAuditableEntity, ISoftDeletableEntity
{
    public User(string firstName, string lastName, string email, byte[] passwordHash, string? profilePictureUrl) : base()
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
        ProfilePictureUrl = profilePictureUrl;
    }
    public User()
    {
        
    }
    public long CreatedOnUtc { get; }
    public long? ModifiedOnUtc { get; }
    public long? DeletedOnUtc { get; }
    public bool Deleted { get; }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string FullName => $"{FirstName} {LastName}";
    public string Email { get; private set; }
    public string? ProfilePictureUrl { get; private set; }
    public byte[] PasswordHash { get; private set; }

    public virtual ICollection<Order>? Orders{ get; private set; }
    public virtual ICollection<Course>? CoursesCreated { get; private set; }
    public virtual ICollection<Course>? CoursesEnrolled { get; private set; }

    public static User Create(string firstName, string lastName, string email, byte[] passwordHash, string? profilePictureUrl)
    {
        return new User(firstName, lastName, email, passwordHash, profilePictureUrl);
    }

}
