namespace Domain.Entities;

public class CourseOrder
{
    public CourseOrder(Guid courseId, Guid orderId)
    {
        CourseId = courseId;
        OrderId = orderId;
    }
    public Guid CourseId { get; set; }
    public Course Course { get; set; }
    public Guid OrderId { get; set; }
    public Order Order { get; set; }

}
