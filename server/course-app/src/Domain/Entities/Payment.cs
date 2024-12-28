namespace Domain.Entities;

public class Payment : Entity, IAuditableEntity, ISoftDeletableEntity
{
    public Payment(Guid orderId, string paymentReference, decimal amount) : base()
    {
        Ensure.NotEmpty(orderId, "The order id is required.", nameof(orderId));
        Ensure.NotEmpty(paymentReference, "The payment reference is required.", nameof(paymentReference));
        Ensure.NotNegative(amount, "The amount is required.", nameof(amount));

        OrderId = orderId;
        PaymentReference = paymentReference;
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
    public string PaymentReference { get; set; }
    public decimal Amount { get; set; }

    public virtual Order Order { get; set; }

    public static Payment Create(Guid orderId, string paymentReference, decimal amount)
    {
        return new Payment(orderId, paymentReference, amount);
    }
}
