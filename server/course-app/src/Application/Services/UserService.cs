using Application.FluentValidations;
using FluentValidation;

namespace Application.Services;

public interface IUserService
{
    Task<Result<UserDto>> CreateAsync(UserCreateDto userCreateDto);
    Task<Result<UserDto>> GetUserByIdAsync(Guid userId);
    Task<Result> UpdateAsync(Guid userId, UserUpdateDto userUpdateDto);
    Task<Result> DeleteAsync(Guid userId);
}

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IValidator<UserCreateDto> _userCreateDtoValidator;
    private readonly IValidator<UserUpdateDto> _userUpdateDtoValidator;

    public UserService(
        UserManager<ApplicationUser> userManager, 
        IValidator<UserCreateDto> userCreateDtoValidator, 
        IValidator<UserUpdateDto> userUpdateDtoValidator)
    {
        _userManager = userManager;
        _userCreateDtoValidator = userCreateDtoValidator;
        _userUpdateDtoValidator = userUpdateDtoValidator;
    }

    public async Task<Result<UserDto>> CreateAsync(UserCreateDto userCreateDto)
    {
        var validationResult = await _userCreateDtoValidator.ValidateAsync(userCreateDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => DomainErrors.User.CannotCreate(x.ErrorMessage)).ToList();
            return Result.Failure<UserDto>(errors);
        }

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

    public async Task<Result> UpdateAsync(Guid userId, UserUpdateDto userUpdateDto)
    {
        var validationResult = await _userUpdateDtoValidator.ValidateAsync(userUpdateDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => DomainErrors.User.CannotCreate(x.ErrorMessage)).ToList();
            return Result.Failure<UserDto>(errors);
        }
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
        return Result.Success();
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