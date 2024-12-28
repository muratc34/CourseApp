namespace Domain.Entities;

public class Order : Entity, IAuditableEntity, ISoftDeletableEntity
{
    public Order(Guid userId, string tcNo, string city, string country, string address, string zipCode) : base()
    {
        Ensure.NotEmpty(userId, "The instructuor id is required.", nameof(userId));

        UserId = userId;
        Status = OrderStatuses.Pending;
        OrderDetails = [];
        TcNo = tcNo;
        City = city;
        Country = country;
        Address = address;
        ZipCode = zipCode;
    }
    public Order()
    {

    }
    public long CreatedOnUtc { get; }
    public long? ModifiedOnUtc { get; }
    public long? DeletedOnUtc { get; }
    public bool Deleted { get; }

    public string TcNo { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string Address { get; set; }
    public string ZipCode { get; set; }
    public Guid UserId { get; private set; }
    public string Status { get; private set; }

    public virtual ApplicationUser? User { get; set; }
    public virtual Payment? Payment { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; }

    public static Order Create(Guid userId, string tcNo, string city, string country, string address, string zipCode)
    {
        return new Order(userId, tcNo, city, country, address, zipCode);
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
