using Application.Abstractions.Messaging;
using Domain.Events;
using FluentValidation;

namespace Application.Services;

public interface IUserService
{
    Task<Result<AccessToken>> CreateAsync(UserCreateDto userCreateDto);
    Task<Result<UserDto>> GetUserByIdAsync(Guid userId);
    Task<Result> UpdateAsync(Guid userId, UserUpdateDto userUpdateDto);
    Task<Result> DeleteAsync(Guid userId);
    Task<Result> AddRoleToUser(Guid userId, Guid roleId);
    Task<Result> RemoveRoleFromUser(Guid userId, Guid roleId);
}

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IValidator<UserCreateDto> _userCreateDtoValidator;
    private readonly IValidator<UserUpdateDto> _userUpdateDtoValidator;
    private readonly IEventPublisher _eventPublisher;
    private readonly IJwtProvider _jwtProvider;
    private readonly IRepository<RefreshToken> _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager, 
        IValidator<UserCreateDto> userCreateDtoValidator, 
        IValidator<UserUpdateDto> userUpdateDtoValidator,
        IEventPublisher eventPublisher,
        IJwtProvider jwtProvider,
        IRepository<RefreshToken> refreshTokenRepository,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _userCreateDtoValidator = userCreateDtoValidator;
        _userUpdateDtoValidator = userUpdateDtoValidator;
        _eventPublisher = eventPublisher;
        _jwtProvider = jwtProvider;
        _unitOfWork = unitOfWork;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<Result<AccessToken>> CreateAsync(UserCreateDto userCreateDto)
    {
        var validationResult = await _userCreateDtoValidator.ValidateAsync(userCreateDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => DomainErrors.User.CannotCreate(x.ErrorMessage)).ToList();
            return Result.Failure<AccessToken>(errors);
        }

        var user = ApplicationUser.Create(userCreateDto.FirstName, userCreateDto.LastName, userCreateDto.Email, userCreateDto.UserName, userCreateDto.ProfilePictureUrl);
        var result = await _userManager.CreateAsync(user, userCreateDto.Password);

        if(!result.Succeeded)
        {
            var errors = result.Errors
                .Select(x => DomainErrors.User.CannotCreate(x.Description))
                .ToList();
            return Result.Failure<AccessToken>(errors);
        }

        var emailVerificationToken = new Random().Next(100000, 1000000);

        var userRegisteredEvent = new UserRegisteredEvent(user.Id, user.Email, user.FullName, emailVerificationToken);
        await _eventPublisher.PublishAsync(userRegisteredEvent);

        var token = await _jwtProvider.CreateToken(user);
        var userRefreshToken = await _refreshTokenRepository.GetAsync(x => x.UserId == user.Id);
        if (userRefreshToken == null)
        {
            await _refreshTokenRepository.CreateAsync(RefreshToken.Create(user.Id, token.RefreshToken, token.RefreshTokenExpiration));
        }
        else
        {
            userRefreshToken.UpdateToken(token.RefreshToken, token.RefreshTokenExpiration);
        }

        await _unitOfWork.SaveChangesAsync();
        return Result.Success(token);
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

    public async Task<Result> AddRoleToUser(Guid userId, Guid roleId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return Result.Failure(DomainErrors.User.NotFound(userId));
        }
        var role = await _roleManager.FindByIdAsync(roleId.ToString());
        if (role is null)
        {
            return Result.Failure(DomainErrors.User.NotFound(roleId));
        }
        var result = await _userManager.AddToRoleAsync(user, role.Name);
        if (!result.Succeeded)
        {
            var errors = result.Errors
                .Select(x => DomainErrors.User.CannotCreate(x.Description))
                .ToList();
            return Result.Failure(errors);
        }
        return Result.Success();
    }

    public async Task<Result> RemoveRoleFromUser(Guid userId, Guid roleId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return Result.Failure(DomainErrors.User.NotFound(userId));
        }
        var role = await _roleManager.FindByIdAsync(roleId.ToString());
        if (role is null)
        {
            return Result.Failure(DomainErrors.User.NotFound(roleId));
        }
        var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
        if (!result.Succeeded)
        {
            var errors = result.Errors
                .Select(x => DomainErrors.User.CannotCreate(x.Description))
                .ToList();
            return Result.Failure(errors);
        }
        return Result.Success();
    }
}