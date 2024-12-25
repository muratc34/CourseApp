using API.Extensions;
using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryCreateDto categoryCreateDto)
    {
        var result = await _categoryService.Create(categoryCreateDto);
        return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
    }

    [HttpPut]
    [Route("{categoryId}")]
    public async Task<IActionResult> Update(Guid categoryId, CategoryUpdateDto categoryUpdateDto)
    {
        var result = await _categoryService.Update(categoryId, categoryUpdateDto);
        return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
    }

    [HttpDelete]
    [Route("{categoryId}")]
    public async Task<IActionResult> Delete(Guid categoryId)
    {
        var result = await _categoryService.Delete(categoryId);
        return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToekn)
    {
        return Ok(await _categoryService.GetCategories(cancellationToekn));
    }
}
