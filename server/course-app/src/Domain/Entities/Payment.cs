namespace Domain.Entities;

public class Payment : Entity, IAuditableEntity, ISoftDeletableEntity
{
    public Payment(Guid orderId, decimal amount) : base()
    {
        Ensure.NotEmpty(orderId, "The order id is required.", nameof(orderId));
        Ensure.NotNegative(amount, "The amount is required.", nameof(amount));

        OrderId = orderId;
        Amount = amount;
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

    public static Payment Create(Guid orderId, decimal amount)
    {
        return new Payment(orderId, amount);
    }
}
