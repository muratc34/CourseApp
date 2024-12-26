namespace Application.Services;

public interface IUserService
{
    Task<Result<UserDto>> CreateAsync(UserCreateDto userCreateDto);
    Task<Result<UserDto>> GetUserByIdAsync(Guid userId);
    Task<Result<UserDto>> UpdateAsync(Guid userId, UserUpdateDto userUpdateDto);
    Task<Result> DeleteAsync(Guid userId);
}

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<UserDto>> CreateAsync(UserCreateDto userCreateDto)
    {
        var user = ApplicationUser.Create(userCreateDto.FirstName, userCreateDto.LastName, userCreateDto.Email, userCreateDto.UserName, userCreateDto.ProfilePictureUrl);
        var result = await _userManager.CreateAsync(user, userCreateDto.Password);

        if(!result.Succeeded)
        {
            var errors = result.Errors
                .Select(x => DomainErrors.User.CannotCreate(x.Description))
                .ToList();
            return Result.Failure<UserDto>(errors);
        }

        return Result.Success(new UserDto(user.Id, user.CreatedOnUtc, user.FullName, user.Email, user.UserName, user.ProfilePictureUrl));
    }

    public async Task<Result<UserDto>> GetUserByIdAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return Result.Failure<UserDto>(DomainErrors.User.NotFound(userId));
        }

        return Result.Success(new UserDto(user.Id, user.CreatedOnUtc, user.FullName, user.Email, user.UserName, user.ProfilePictureUrl));
    }

    public async Task<Result<UserDto>> UpdateAsync(Guid userId, UserUpdateDto userUpdateDto)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return Result.Failure<UserDto>(DomainErrors.User.NotFound(userId));
        }
        user.Update(userUpdateDto.FirstName,userUpdateDto.LastName, userUpdateDto.Email, userUpdateDto.UserName, userUpdateDto.ProfilePictureUrl);
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = result.Errors
                .Select(x => DomainErrors.User.CannotUpdate(x.Description))
                .ToList();
            return Result.Failure<UserDto>(errors);
        }
        return Result.Success(new UserDto(user.Id, user.CreatedOnUtc, user.FullName, user.Email, user.UserName, user.ProfilePictureUrl));
    }

    public async Task<Result> DeleteAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return Result.Failure<UserDto>(DomainErrors.User.NotFound(userId));
        }
        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            var errors = result.Errors
                .Select(x => DomainErrors.User.CannotDelete(x.Description))
                .ToList();
            return Result.Failure(errors);
        }
        return Result.Success();
    }
}
