namespace Infrastructure.UnitOfWorks;

public class UnitOfWork : IUnitOfWork
{
    private readonly DatabaseContext _context;

    public UnitOfWork(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
