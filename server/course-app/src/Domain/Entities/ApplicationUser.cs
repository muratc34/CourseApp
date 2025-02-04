﻿namespace Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>, IAuditableEntity
{
    public ApplicationUser(string firstName, string lastName, string email, string userName) : base()
    {
        Ensure.NotEmpty(firstName, "The first name is required.", nameof(firstName));
        Ensure.NotEmpty(lastName, "The first name is required.", nameof(firstName));
        Ensure.NotEmpty(email, "The first name is required.", nameof(firstName));
        Ensure.NotEmpty(userName, "The first name is required.", nameof(userName));

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        UserName = userName;
    }

    public long CreatedOnUtc { get; }
    public long? ModifiedOnUtc { get; }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string FullName => $"{FirstName} {LastName}";
    public string? ProfilePictureUrl { get; private set; }

    public virtual ICollection<Order>? Orders { get; private set; }
    public virtual ICollection<Course>? CoursesCreated { get; private set; }
    public virtual ICollection<Enrollment>? Enrollments { get; private set; }

    public static ApplicationUser Create(string firstName, string lastName, string email, string userName)
    {
        return new ApplicationUser(firstName, lastName, email, userName);
    }

    public ApplicationUser Update(string? firstName, string? lastName, string? email, string? userName)
    {
        if (!string.IsNullOrWhiteSpace(firstName))
        {
            FirstName = firstName;
        }
        if (!string.IsNullOrWhiteSpace(lastName))
        {
            LastName = lastName;
        }
        if (!string.IsNullOrWhiteSpace(email))
        {
            Email = email;
        }
        if (!string.IsNullOrWhiteSpace(userName))
        {
            UserName = userName;
        }
        return this;
    }

    public void EmailVerify()
    {
        EmailConfirmed = true;
    }

    public void UpdateUserProfilePictureUrl(string url)
    {
        ProfilePictureUrl = url;
    }
}
