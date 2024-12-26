namespace Infrastructure.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(DatabaseContext context) : base(context)
    {
    }
}
