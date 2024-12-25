using Application.Abstractions.Authentication;
using Application.DTOs;
using Domain.Core.Errors;
using Domain.Core.Results;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public interface IUserService
{
    Task<Result<UserDto>> CreateAsync(UserCreateDto userCreateDto);
    Task<Result<UserDto>> GetUserByIdAsync(Guid userId);
}

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserContext _userContext;

    public UserService(UserManager<ApplicationUser> userManager, IUserContext userContext)
    {
        _userManager = userManager;
        _userContext = userContext;
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
        if (userId == Guid.Empty || userId != _userContext.UserId)
        {
            return Result.Failure<UserDto>(DomainErrors.User.Permission);
        }

        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
        {
            return Result.Failure<UserDto>(DomainErrors.User.NotFound(userId));
        }

        return Result.Success(new UserDto(user.Id, user.CreatedOnUtc, user.FullName, user.Email, user.UserName, user.ProfilePictureUrl));
    }
}
