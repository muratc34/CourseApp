namespace Infrastructure.Repositories;

internal class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(DatabaseContext context) 
        : base(context)
    {
    }
}
