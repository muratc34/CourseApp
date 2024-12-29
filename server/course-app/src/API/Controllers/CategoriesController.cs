using API.Extensions;
using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create(CategorySaveDto categoryCreateDto)
    {
        var result = await _categoryService.Create(categoryCreateDto);
        return result.IsSuccess ? CreatedAtAction(nameof(result.Data.Id), new { categoryId = result.Data.Id}, result.Data) : result.ToProblemDetails();
    }

    [HttpPut]
    [Route("{categoryId}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Update(Guid categoryId, CategorySaveDto categoryUpdateDto)
    {
        var result = await _categoryService.Update(categoryId, categoryUpdateDto);
        return result.IsSuccess ? NoContent() : result.ToProblemDetails();
    }

    [HttpDelete]
    [Route("{categoryId}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(Guid categoryId)
    {
        var result = await _categoryService.Delete(categoryId);
        return result.IsSuccess ? NoContent() : result.ToProblemDetails();
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToekn)
    {
        return Ok(await _categoryService.GetCategories(cancellationToekn));
    }
}
