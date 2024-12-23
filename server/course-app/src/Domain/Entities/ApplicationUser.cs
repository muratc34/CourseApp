namespace Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>, IAuditableEntity, ISoftDeletableEntity
{
    public ApplicationUser(string firstName, string lastName, string email, string? profilePictureUrl) : base()
    {
        Ensure.NotEmpty(firstName, "The first name is required.", nameof(firstName));
        Ensure.NotEmpty(lastName, "The first name is required.", nameof(firstName));
        Ensure.NotEmpty(email, "The first name is required.", nameof(firstName));

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        ProfilePictureUrl = profilePictureUrl;
    }

    public long CreatedOnUtc { get; }
    public long? ModifiedOnUtc { get; }
    public long? DeletedOnUtc { get; }
    public bool Deleted { get; }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string FullName => $"{FirstName} {LastName}";
    public string? ProfilePictureUrl { get; private set; }

    public virtual ICollection<Order>? Orders { get; private set; }
    public virtual ICollection<Course>? CoursesCreated { get; private set; }
    public virtual ICollection<Course>? CoursesEnrolled { get; private set; }

    public static ApplicationUser Create(string firstName, string lastName, string email, string? profilePictureUrl)
    {
        return new ApplicationUser(firstName, lastName, email, profilePictureUrl);
    }
}
