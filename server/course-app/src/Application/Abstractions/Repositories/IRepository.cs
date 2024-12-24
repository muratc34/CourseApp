namespace Application.Abstractions.Repositories;
public interface IRepository<TEntity> 
    where TEntity : Entity
{
    public Task CreateAsync(TEntity entity);
    public void Delete(TEntity entity);
    public TEntity Update(TEntity entity);
    public Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate);
    public Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);
    public Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null);
    public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
    public IQueryable<TEntity> FindAll();
}
