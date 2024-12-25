using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWorks;
using Application.DTOs;
using Domain.Core.Errors;
using Domain.Core.Results;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public interface ICategoryService
{
    Task<Result<CategoryDto>> Create(CategoryCreateDto categoryCreateDto);
    Task<Result> Update(Guid categoryId, CategoryUpdateDto categoryUpdateDto);
    Task<Result> Delete(Guid categoryId);
    Task<Result<List<CategoryDto>>> GetCategories(CancellationToken cancellationToken);
}

public class CategoryService : ICategoryService
{
    private readonly IRepository<Category> _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IRepository<Category> categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CategoryDto>> Create(CategoryCreateDto categoryCreateDto)
    {
        var category = Category.Create(categoryCreateDto.Name);
        await _categoryRepository.CreateAsync(category);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success(new CategoryDto(category.Id, category.CreatedOnUtc, category.Name));
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
            .Select(x => new CategoryDto(x.Id, x.CreatedOnUtc, x.Name))
            .ToListAsync(cancellationToken);

        return Result.Success(categories);
    }

    public async Task<Result> Update(Guid categoryId, CategoryUpdateDto categoryUpdateDto)
    {
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
}
