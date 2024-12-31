namespace Infrastructure.Repositories;

public class OrderRepository : Repository<Domain.Entities.Order>, IOrderRepository
{
    public OrderRepository(DatabaseContext context) : base(context)
    {
    }
}
