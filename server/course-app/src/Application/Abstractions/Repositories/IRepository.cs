using Domain.Core.Pagination;

namespace Application.Abstractions.Repositories;
public interface IRepository<TEntity> 
    where TEntity : Entity
{
    public Task CreateAsync(TEntity entity);
    public void Delete(TEntity entity);
    public TEntity Update(TEntity entity);
    public Task<int> CountAsync(CancellationToken cancellationToken = default, Expression<Func<TEntity, bool>>? predicate = null);
    Task<PagedList<TDto>> GetAllByPagingAsync<TDto>(
       int currentPage, int pageSize,
        CancellationToken cancellationToken = default,
        Expression<Func<TEntity, TDto>>? selector = null,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);
    public Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null);
    public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
    public IQueryable<TEntity> FindAll();
}
