using Application.FluentValidations;
using FluentValidation;

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

    public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, IValidator<CategorySaveDto> categorySaveDtoValidator)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _categorySaveDtoValidator = categorySaveDtoValidator;
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
        return Result.Success();
    }

    public async Task<Result<List<CategoryDto>>> GetCategories(CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.FindAll()
            .Select(x => new CategoryDto(x.Id, x.CreatedOnUtc, x.ModifiedOnUtc, x.Name))
            .ToListAsync(cancellationToken);

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

        return Result.Success();
    }

    public async Task<Result<CategoryDto>> GetCategoryById(Guid categoryId)
    {
        var category = await _categoryRepository.GetAsync(x => x.Id == categoryId);
        if (category is null)
        {
            return Result.Failure<CategoryDto>(DomainErrors.Category.NotFound);
        }
        return Result.Success(new CategoryDto(category.Id, category.CreatedOnUtc, category.ModifiedOnUtc, category.Name));
    }

}
