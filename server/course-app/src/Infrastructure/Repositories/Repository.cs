using Domain.Core.Pagination;

namespace Infrastructure.Repositories;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : Entity
{
    private readonly DatabaseContext _context;
    public Repository(DatabaseContext context) => _context = context;

    public async Task<int> CountAsync(CancellationToken cancellationToken = default, Expression<Func<TEntity, bool>>? predicate = null)
    {
        _context.Set<TEntity>().AsNoTracking();
        if (predicate is null)
        {
            return await _context.Set<TEntity>().CountAsync();
        }
        return await _context.Set<TEntity>().Where(predicate).CountAsync(cancellationToken);
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
        => _context.Set<TEntity>().Where(predicate);

    public IQueryable<TEntity> FindAll()
        => _context.Set<TEntity>().AsNoTracking();

    public async Task<PagedList<TDto>> GetAllByPagingAsync<TDto>(
        int currentPage, int pageSize,
        CancellationToken cancellationToken = default,
        Expression<Func<TEntity, TDto>>? selector = null,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
    {
        IQueryable<TEntity> queryable = _context.Set<TEntity>().AsNoTracking();
        if (predicate is not null)
        {
            queryable = queryable.Where(predicate);
        }
        var totalCount = await queryable.CountAsync(cancellationToken);
        if (orderBy is not null)
        {
           queryable = orderBy(queryable);
        }
        selector ??= x => (TDto)(object)x;

        var items = await queryable
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .Select(selector)
            .ToListAsync(cancellationToken);

        return new PagedList<TDto>(items, currentPage, pageSize, totalCount);
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
    {
        IQueryable<TEntity> queryable = _context.Set<TEntity>();
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
