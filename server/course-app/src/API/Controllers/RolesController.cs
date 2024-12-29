using API.Extensions;
using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetRoles(CancellationToken cancellationToken)
        {
            var result = await _roleService.GetRoles(cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(RoleSaveDto dto)
        {
            var result = await _roleService.CreateRole(dto);
            return result.IsSuccess ? CreatedAtAction(nameof(GetById), new { roleId = result.Data.Id }, result.Data) : result.ToProblemDetails();
        }

        [HttpDelete]
        [Route("{roleId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(Guid roleId)
        {
            var result = await _roleService.DeleteRole(roleId);
            return result.IsSuccess ? NoContent() : result.ToProblemDetails();
        }

        [HttpGet]
        [Route("{roleId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetById(Guid roleId)
        {
            var result = await _roleService.GetById(roleId);
            return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
        }
    }
}
