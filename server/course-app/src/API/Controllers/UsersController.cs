using API.Extensions;
using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
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
    }
}
