namespace Domain.Core.Entities;

public class Payment : Entity, IAuditableEntity, ISoftDeletableEntity
{
    public Payment(Guid orderId, decimal amount, string status) : base()
    {
        OrderId = orderId;
        Amount = amount;
        Status = status;
    }

    public Payment()
    {
        
    }

    public long CreatedOnUtc { get; }
    public long? ModifiedOnUtc { get; }
    public long? DeletedOnUtc { get; }
    public bool Deleted { get; }

    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; }

    public static Payment Create(Guid orderId, decimal amount, string status)
    {
        return new Payment(orderId, amount, status);
    }
}
