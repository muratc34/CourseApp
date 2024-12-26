namespace Infrastructure.Repositories;

public class PaymentRepository : Repository<Payment>, IPaymentRepository
{
    public PaymentRepository(DatabaseContext context) : base(context)
    {
    }
}
