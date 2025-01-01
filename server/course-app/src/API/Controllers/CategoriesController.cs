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
        return result.IsSuccess ? CreatedAtAction(nameof(GetById), new { categoryId = result.Data.Id}, result.Data) : result.ToProblemDetails();
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
    public async Task<IActionResult> Get(CancellationToken cancellationToekn, int pageIndex = 1, int pageSize = 12)
    {
        return Ok(await _categoryService.GetCategories(pageIndex, pageSize, cancellationToekn));
    }

    [HttpGet]
    [Route("{categoryId}")]
    public async Task<IActionResult> GetById(Guid categoryId)
    {
        var result = await _categoryService.GetCategoryById(categoryId);
        return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
    }
}
