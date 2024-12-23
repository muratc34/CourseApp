namespace Domain.Entities;

public class Order : Entity, IAuditableEntity, ISoftDeletableEntity
{
    public Order(Guid userId) : base()
    {
        Ensure.NotEmpty(userId, "The instructuor id is required.", nameof(userId));

        UserId = userId;
        Status = OrderStatuses.Pending;
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
    public decimal TotalAmount => OrderDetails.Sum(od => od.Course.Price);

    public virtual ApplicationUser? User { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; set; }

    public static Order Create(Guid userId)
    {
        return new Order(userId);
    }

    public void SetStatusAsProcessing() => SetStatus(OrderStatuses.Processing);
    public void SetStatusAsCompleted() => SetStatus(OrderStatuses.Completed);
    public void SetStatusAsCancelled() => SetStatus(OrderStatuses.Cancelled);
    public void SetStatusAsFailed() => SetStatus(OrderStatuses.Failed);
    public void SetStatusAsRefunded() => SetStatus(OrderStatuses.Refunded);

    private void SetStatus(string newStatus)
    {
        Ensure.NotNull(newStatus, "The status is required.", nameof(newStatus));
        Status = newStatus;
    }
}
