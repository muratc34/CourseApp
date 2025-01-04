namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var result = await _authenticationService.LoginAsync(loginDto);
        return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
    }

    [HttpPatch]
    [Route("ChangePassword/{userId}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> ChangePassword(Guid userId, ChangePasswordDto changePasswordDto)
    {
        var result = await _authenticationService.ChangePassword(userId, changePasswordDto);
        return result.IsSuccess ? NoContent() : result.ToProblemDetails();
    }

    [HttpPost]
    [Route("CreateTokenByRefreshToken")]
    public async Task<IActionResult> CreateTokenByRefreshToken(RefreshTokenDto refreshTokenDto)
    {
        var result = await _authenticationService.CreateTokenByRefreshToken(refreshTokenDto.Token);
        return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
    }

    [HttpPost]
    [Route("ExterminateRefreshToken")]
    public async Task<IActionResult> ExterminateRefreshToken(RefreshTokenDto refreshTokenDto)
    {
        var result = await _authenticationService.ExterminateRefreshToken(refreshTokenDto.Token);
        return result.IsSuccess ? NoContent() : result.ToProblemDetails();
    }

    [HttpPost]
    [Route("ConfirmEmail")]
    [Authorize]
    public async Task<IActionResult> ConfirmEmail(EmailConfirmDto emailConfirmDto)
    {
        var result = await _authenticationService.EmailConfirmation(emailConfirmDto);
        return result.IsSuccess ? NoContent() : result.ToProblemDetails();
    }

    [HttpPost]
    [Route("ResendEmailConfirmationToken/{userId}")]
    [Authorize]
    public async Task<IActionResult> ResendEmailConfirmationToken(Guid userId)
    {
        var result = await _authenticationService.ResendEmailConfirmationToken(userId);
        return result.IsSuccess ? NoContent() : result.ToProblemDetails();
    }
}
