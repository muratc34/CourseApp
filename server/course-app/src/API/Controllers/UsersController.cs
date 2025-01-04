namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> CreateUser(UserCreateDto userCreateDto)
    {
        var result = await _userService.CreateAsync(userCreateDto);
        return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
    }

    [HttpGet]
    [Route("{userId}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> GetUserById(Guid userId)
    {
        var result = await _userService.GetUserByIdAsync(userId);
        return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
    }

    [HttpPut]
    [Route("{userId}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> UpdateUser(Guid userId, UserUpdateDto userUpdateDto)
    {
        var result = await _userService.UpdateAsync(userId, userUpdateDto);
        return result.IsSuccess ? NoContent() : result.ToProblemDetails();
    }

    [HttpDelete]
    [Route("{userId}")]
    [Authorize(Roles = "admin,user")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        var result = await _userService.DeleteAsync(userId);
        return result.IsSuccess ? NoContent() : result.ToProblemDetails();
    }

    [HttpPut]
    [Route("{userId}/role/{roleId}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> AddRoleToUser(Guid userId, Guid roleId)
    {
        var result = await _userService.AddRoleToUser(userId, roleId);
        return result.IsSuccess ? NoContent() : result.ToProblemDetails(); 
    }

    [HttpDelete]
    [Route("{userId}/role/{roleId}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> RemoveRoleFromUser(Guid userId, Guid roleId)
    {
        var result = await _userService.RemoveRoleFromUser(userId, roleId);
        return result.IsSuccess ? NoContent() : result.ToProblemDetails();
    }

    [HttpPost]
    [Route("UploadImage/{userId}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> UploadImageFile(Guid userId, IFormFile formFile, CancellationToken cancellationToken)
    {
        byte[] fileBytes;
        using (var memoryStream = new MemoryStream())
        {
            await formFile.CopyToAsync(memoryStream);
            fileBytes = memoryStream.ToArray();
        }
        var result = await _userService.UpdateUserPicture(userId, Path.GetExtension(formFile.FileName), fileBytes, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblemDetails();
    }

    [HttpDelete]
    [Route("UploadImage/{userId}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> RemoveImageFile(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _userService.RemoveUserPicture(userId, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblemDetails();
    }
}
