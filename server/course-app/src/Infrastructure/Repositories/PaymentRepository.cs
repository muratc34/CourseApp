namespace Infrastructure.Repositories;

public class PaymentRepository : Repository<Domain.Entities.Payment>, IPaymentRepository
{
    public PaymentRepository(DatabaseContext context) : base(context)
    {
    }
}
