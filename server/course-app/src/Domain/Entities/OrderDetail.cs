namespace Domain.Entities;

public class OrderDetail
{
    public OrderDetail(Guid courseId, Guid orderId)
    {
        CourseId = courseId;
        OrderId = orderId;
    }
    public Guid CourseId { get; set; }
    public virtual Course? Course { get; set; }
    public Guid OrderId { get; set; }
    public virtual Order? Order { get; set; }
}
