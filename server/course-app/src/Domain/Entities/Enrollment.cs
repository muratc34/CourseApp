namespace Domain.Entities;

public class Enrollment : IAuditableEntity
{
    public Enrollment(Guid userId, Guid courseId)
    {
        Ensure.NotEmpty(userId, "The user id is required.", nameof(userId));
        Ensure.NotEmpty(courseId, "The course id is required.", nameof(courseId));

        UserId = userId;
        CourseId = courseId;
    }
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; }

    public Guid CourseId { get; set; }
    public Course Course { get; set; }

    public long CreatedOnUtc { get; }
    public long? ModifiedOnUtc { get; }
}
