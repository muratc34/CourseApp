﻿namespace Domain.Entities;

public class Course : Entity, IAuditableEntity, ISoftDeletableEntity
{
    public Course(string name, string description, decimal price, Guid categoryId, Guid instructorId) : base()
    {
        Ensure.NotEmpty(name, "The name is required.", nameof(name));
        Ensure.NotEmpty(name, "The description is required.", nameof(name));
        Ensure.NotNegative(price, "The price is required", nameof(price));
        Ensure.NotEmpty(categoryId, "The category id is required.", nameof(categoryId));
        Ensure.NotEmpty(instructorId, "The instructuor id is required.", nameof(instructorId));

        Name = name;
        Description = description;
        Price = price;
        CategoryId = categoryId;
        InstructorId = instructorId;
        Enrollments = [];
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
    public virtual ApplicationUser? Instructor { get; private set; }
    public virtual ICollection<Enrollment>? Enrollments { get; private set; }

    public static Course Create(string name, string description, decimal price, Guid categoryId, Guid instructorId)
    {
        return new Course(name, description, price, categoryId, instructorId);
    }

    public Course Update(string? name, string? description, decimal? price, Guid? categoryId)
    {
        if (!string.IsNullOrEmpty(name)) 
        {
            Name = name;
        }
        if (!string.IsNullOrEmpty(description))
        {
            Description = description;
        }
        if (price != null && price > 0)
        {
            Price = (decimal)price;
        }
        if (categoryId != null && categoryId != Guid.Empty)
        {
            CategoryId = (Guid)categoryId;
        }
        return this;
    }

    public void AddUserToCourse(Enrollment enrollment)
    {
        if (Enrollments.Any(e => e.UserId == enrollment.UserId))
        {
            throw new InvalidOperationException("User is already enrolled in this course.");
        }
        Enrollments.Add(enrollment);
    }

    public void UpdateCourseImage(string url)
    {
        ImageUrl = url;
    }
}