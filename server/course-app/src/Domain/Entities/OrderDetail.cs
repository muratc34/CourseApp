namespace Domain.Entities;

public class OrderDetail
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CourseId { get; set; }

    public ApplicationUser User { get; set; }
    public Course Course { get; set; }
}
