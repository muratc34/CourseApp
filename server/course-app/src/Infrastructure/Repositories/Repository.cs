namespace Infrastructure.Repositories;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : Entity
{
    private readonly DatabaseContext _context;
    public Repository(DatabaseContext context) => _context = context;

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate)
    {
        _context.Set<TEntity>().AsNoTracking();
        if (predicate is null)
        {
            return await _context.Set<TEntity>().CountAsync();
        }
        return await _context.Set<TEntity>().Where(predicate).CountAsync();
    }

    public async Task CreateAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
    }

    public void Delete(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }

    public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        => _context.Set<TEntity>().AsNoTracking().Where(predicate);

    public IQueryable<TEntity> FindAll()
        => _context.Set<TEntity>().AsNoTracking();

    public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = _context.Set<TEntity>().AsNoTracking();
        if (predicate is not null)
        {
            queryable = queryable.Where(predicate);
        }
        return await queryable.ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
    {
        IQueryable<TEntity> queryable = _context.Set<TEntity>().AsNoTracking();
        if (include is not null)
        {
            queryable = include(queryable);
        }
        return await queryable.FirstOrDefaultAsync(predicate);
    }

    public TEntity Update(TEntity entity)
    {
         _context.Set<TEntity>().Update(entity);
        return entity;
    }
}
