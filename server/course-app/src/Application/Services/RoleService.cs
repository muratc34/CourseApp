using FluentValidation;

namespace Application.Services;

public interface IRoleService
{
    Task<Result<RoleDto>> CreateRole(RoleSaveDto roleCreateDto);
    Task<Result> DeleteRole(Guid roleId);
    Task<Result<List<RoleDto>>> GetRoles(CancellationToken cancellationToken);
    Task<Result<RoleDto>> GetById(Guid roleId);
}
public class RoleService : IRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IValidator<RoleSaveDto> _roleValidator;

    public RoleService(RoleManager<ApplicationRole> roleManager, IValidator<RoleSaveDto> roleValidator)
    {
        _roleManager = roleManager;
        _roleValidator = roleValidator;
    }

    public async Task<Result<RoleDto>> CreateRole(RoleSaveDto roleCreateDto)
    {
        var validationResult = await _roleValidator.ValidateAsync(roleCreateDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => DomainErrors.User.CannotCreate(x.ErrorMessage)).ToList();
            return Result.Failure<RoleDto>(errors);
        }

        var role = ApplicationRole.Create(roleCreateDto.Name);
        var result = await _roleManager.CreateAsync(role);

        if (!result.Succeeded)
        {
            var errors = result.Errors
                .Select(x => DomainErrors.User.CannotCreate(x.Description))
                .ToList();
            return Result.Failure<RoleDto>(errors);
        }
        return Result.Success(new RoleDto(role.Id, role.CreatedOnUtc, role.ModifiedOnUtc, role.Name));
    }
    public async Task<Result> DeleteRole(Guid roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId.ToString());
        if (role is null)
        {
            return Result.Failure(DomainErrors.Role.NotFound(roleId));
        }
        var result = await _roleManager.DeleteAsync(role);
        if (!result.Succeeded)
        {
            var errors = result.Errors
                .Select(x => DomainErrors.User.CannotCreate(x.Description))
                .ToList();
            return Result.Failure(errors);
        }
        return Result.Success();
    }
    public async Task<Result<List<RoleDto>>> GetRoles(CancellationToken cancellationToken)
    {
        var roles = await _roleManager.Roles
            .Select(x => 
            new RoleDto(
                x.Id, 
                x.CreatedOnUtc, 
                x.ModifiedOnUtc, 
                x.Name)
            ).ToListAsync(cancellationToken);
        return Result.Success(roles);
    }
    public async Task<Result<RoleDto>> GetById(Guid roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId.ToString());
        if (role is null)
        {
            return Result.Failure<RoleDto>(DomainErrors.Role.NotFound(roleId));
        }
        return Result.Success(new RoleDto(role.Id, role.CreatedOnUtc, role.ModifiedOnUtc, role.Name));
    }
}
