using API.Extensions;
using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> ChangePassword(Guid userId, ChangePasswordDto changePasswordDto)
    {
        var result = await _authenticationService.ChangePassword(userId, changePasswordDto);
        return result.IsSuccess ? NoContent() : result.ToProblemDetails();
    }

    [HttpPost]
    [Route("CreateTokenByRefreshToken")]
    public async Task<IActionResult> CreateTokenByRefreshToken(string refreshToken)
    {
        var result = await _authenticationService.CreateTokenByRefreshToken(refreshToken);
        return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
    }

    [HttpPost]
    [Route("ExterminateRefreshToken")]
    public async Task<IActionResult> ExterminateRefreshToken(string refreshToken)
    {
        var result = await _authenticationService.ExterminateRefreshToken(refreshToken);
        return result.IsSuccess ? NoContent() : result.ToProblemDetails();
    }

    [HttpPost]
    [Route("ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmail(Guid userId, string token)
    {
        var result = await _authenticationService.EmailConfirmation(userId, token);
        return result.IsSuccess ? NoContent() : result.ToProblemDetails();
    }
}
