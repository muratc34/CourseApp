namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpPost]
    [Authorize(Roles = "admin, instructor")]
    public async Task<IActionResult> Create(CourseCreateDto courseCreateDto)
    {
        var result = await _courseService.Create(courseCreateDto);
        return result.IsSuccess ? CreatedAtAction(nameof(GetById), new { courseId = result.Data.Id}, result.Data) : result.ToProblemDetails();
    }

    [HttpPut]
    [Route("{courseId}")]
    [Authorize(Roles = "admin, instructor")]
    public async Task<IActionResult> Update(Guid courseId, CourseUpdateDto courseUpdateDto)
    {
        var result = await _courseService.Update(courseId, courseUpdateDto);
        return result.IsSuccess ?  NoContent() : result.ToProblemDetails();
    }

    [HttpDelete]
    [Route("{courseId}")]
    [Authorize(Roles = "admin, instructor")]
    public async Task<IActionResult> Delete(Guid courseId)
    {
        var result = await _courseService.Delete(courseId);
        return result.IsSuccess ? NoContent() : result.ToProblemDetails();
    }

    [HttpGet]
    [Route("{courseId}")]
    public async Task<IActionResult> GetById(Guid courseId)
    {
        var result = await _courseService.GetCourseById(courseId);
        return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return Ok(await _courseService.GetCourses(cancellationToken));
    }

    [HttpGet]
    [Route("Categories/{categoryId}")]
    public async Task<IActionResult> Get(Guid categoryId, CancellationToken cancellationToken)
    {
        var result = await _courseService.GetCourseByCategoryId(categoryId, cancellationToken);
        return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
    }

    [HttpGet]
    [Route("Users/{userId}")]
    [Authorize(Roles = "admin, user")]
    public async Task<IActionResult> GetUserCourses(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _courseService.GetCoursesByUserId(userId, cancellationToken);
        return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
    }

    [HttpPost]
    [Route("UploadImage/{courseId}")]
    [Authorize(Roles = "admin, instructor")]
    public async Task<IActionResult> UploadImage(Guid courseId, IFormFile formFile, CancellationToken cancellationToken)
    {
        byte[] fileBytes;
        using (var memoryStream = new MemoryStream())
        {
            await formFile.CopyToAsync(memoryStream);
            fileBytes = memoryStream.ToArray();
        }
        var result = await _courseService.UpdateCourseImage(courseId, Path.GetExtension(formFile.FileName), fileBytes, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblemDetails();
    }

    [HttpDelete]
    [Route("UploadImage/{courseId}")]
    [Authorize(Roles = "admin, instructor")]
    public async Task<IActionResult> RemoveImageFile(Guid courseId, CancellationToken cancellationToken)
    {
        var result = await _courseService.RemoveCourseImage(courseId, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblemDetails();
    }
}
