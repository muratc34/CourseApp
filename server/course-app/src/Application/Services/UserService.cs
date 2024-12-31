using Application.Abstractions.BlobStorage;

namespace Application.Services;

public interface IUserService
{
    Task<Result<AccessToken>> CreateAsync(UserCreateDto userCreateDto);
    Task<Result<UserDto>> GetUserByIdAsync(Guid userId);
    Task<Result> UpdateAsync(Guid userId, UserUpdateDto userUpdateDto);
    Task<Result> DeleteAsync(Guid userId);
    Task<Result> AddRoleToUser(Guid userId, Guid roleId);
    Task<Result> RemoveRoleFromUser(Guid userId, Guid roleId);
    Task<Result> UpdateUserPicture(Guid userId, string fileExtension, byte[] fileData, CancellationToken cancellationToken);
}

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IValidator<UserCreateDto> _userCreateDtoValidator;
    private readonly IEventPublisher _eventPublisher;
    private readonly IJwtProvider _jwtProvider;
    private readonly IRepository<RefreshToken> _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobStorageService _blobStorageService;

    public UserService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager, 
        IValidator<UserCreateDto> userCreateDtoValidator,
        IEventPublisher eventPublisher,
        IJwtProvider jwtProvider,
        IRepository<RefreshToken> refreshTokenRepository,
        IUnitOfWork unitOfWork,
        IBlobStorageService blobStorageService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _userCreateDtoValidator = userCreateDtoValidator;
        _eventPublisher = eventPublisher;
        _jwtProvider = jwtProvider;
        _unitOfWork = unitOfWork;
        _refreshTokenRepository = refreshTokenRepository;
        _blobStorageService = blobStorageService;
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

    public async Task<Result> UpdateUserPicture(Guid userId, string fileExtension, byte[] fileData, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return Result.Failure<UserDto>(DomainErrors.User.NotFound(userId));
        }
        var url = await _blobStorageService.UploadUserImageFileAsync(user.Id, fileExtension, fileData, cancellationToken);
        user.UpdateUserProfilePictureUrl(url);
        await _userManager.UpdateAsync(user);   
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}