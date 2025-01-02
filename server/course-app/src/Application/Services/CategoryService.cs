namespace Application.Services;

public interface ICategoryService
{
    Task<Result<CategoryDto>> Create(CategorySaveDto categoryCreateDto);
    Task<Result> Update(Guid categoryId, CategorySaveDto categoryUpdateDto);
    Task<Result> Delete(Guid categoryId);
    Task<Result<List<CategoryDto>>> GetCategories(CancellationToken cancellationToken);
    Task<Result<CategoryDto>> GetCategoryById(Guid categoryId);
}

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CategorySaveDto> _categorySaveDtoValidator;
    private readonly ICacheService _cacheService;

    public CategoryService(
        ICategoryRepository categoryRepository, 
        IUnitOfWork unitOfWork, 
        IValidator<CategorySaveDto> categorySaveDtoValidator,
        ICacheService cacheService)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _categorySaveDtoValidator = categorySaveDtoValidator;
        _cacheService = cacheService;
    }

    public async Task<Result<CategoryDto>> Create(CategorySaveDto categoryCreateDto)
    {
        var validationResult = await _categorySaveDtoValidator.ValidateAsync(categoryCreateDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => DomainErrors.User.CannotCreate(x.ErrorMessage)).ToList();
            return Result.Failure<CategoryDto>(errors);
        }

        var category = Category.Create(categoryCreateDto.Name);
        await _categoryRepository.CreateAsync(category);
        await _unitOfWork.SaveChangesAsync();

        await _cacheService.RemoveAsync(CachingKeys.CategoriesKey);

        return Result.Success(new CategoryDto(category.Id, category.CreatedOnUtc, category.ModifiedOnUtc, category.Name));
    }

    public async Task<Result> Delete(Guid categoryId)
    {
        var category = await _categoryRepository.GetAsync(x => x.Id == categoryId);
        if (category is null)
        {
            return Result.Failure(DomainErrors.Category.NotFound);
        }
        _categoryRepository.Delete(category);
        await _unitOfWork.SaveChangesAsync();

        
        await _cacheService.RemoveAsync(CachingKeys.CategoriesKey);
        await _cacheService.RemoveAsync(CachingKeys.CategoryByIdKey(category.Id));
        
        return Result.Success();
    }

    public async Task<Result<List<CategoryDto>>> GetCategories(CancellationToken cancellationToken)
    {
        var cachedCategories = await _cacheService.GetAsync<List<CategoryDto>?>(CachingKeys.CategoriesKey);
        if (cachedCategories is not null)
        {
            return Result.Success(cachedCategories);
        }
        var categories = await _categoryRepository.FindAll()
            .Select(x => new CategoryDto(x.Id, x.CreatedOnUtc, x.ModifiedOnUtc, x.Name)).ToListAsync(cancellationToken);

        await _cacheService.SetAsync(CachingKeys.CategoriesKey, categories, TimeSpan.FromMinutes(60));
        return Result.Success(categories);
    }

    public async Task<Result> Update(Guid categoryId, CategorySaveDto categoryUpdateDto)
    {
        var validationResult = await _categorySaveDtoValidator.ValidateAsync(categoryUpdateDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => DomainErrors.User.CannotCreate(x.ErrorMessage)).ToList();
            return Result.Failure(errors);
        }

        var category = await _categoryRepository.GetAsync(x => x.Id == categoryId);
        if (category is null)
        {
            return Result.Failure<CategoryDto>(DomainErrors.Category.NotFound);
        }
        category.Update(categoryUpdateDto.Name);
        _categoryRepository.Update(category);
        await _unitOfWork.SaveChangesAsync();

        await _cacheService.RemoveAsync(CachingKeys.CategoriesKey);
        await _cacheService.RemoveAsync(CachingKeys.CategoryByIdKey(category.Id));

        return Result.Success();
    }

    public async Task<Result<CategoryDto>> GetCategoryById(Guid categoryId)
    {
        var cachedCategory = await _cacheService.GetAsync<CategoryDto>(CachingKeys.CategoryByIdKey(categoryId));
        if (cachedCategory != null)
        {
            return Result.Success(cachedCategory);
        }

        var category = await _categoryRepository.GetAsync(x => x.Id == categoryId);
        if (category is null)
        {
            return Result.Failure<CategoryDto>(DomainErrors.Category.NotFound);
        }

        await _cacheService.SetAsync(CachingKeys.CategoryByIdKey(category.Id), category, TimeSpan.FromMinutes(60));
        return Result.Success(new CategoryDto(category.Id, category.CreatedOnUtc, category.ModifiedOnUtc, category.Name));
    }

}
