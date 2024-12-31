namespace Application.Services;

public interface IAuthenticationService
{
    Task<Result<AccessToken>> LoginAsync(LoginDto loginDto);
    Task<Result> ChangePassword(Guid userId, ChangePasswordDto changePasswordDto);
    Task<Result<AccessToken>> CreateTokenByRefreshToken(string refreshToken);
    Task<Result> ExterminateRefreshToken(string refreshToken);
    Task<Result> EmailConfirmation(EmailConfirmDto emailConfirmDto);
    Task<Result> ResendEmailConfirmationToken(Guid userId);
}

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IJwtProvider _jwtProvider;
    private readonly IRepository<RefreshToken> _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<LoginDto> _loginDtoValidator;
    private readonly IValidator<ChangePasswordDto> _changePasswordDtoValidator;
    private readonly ICacheService _cacheService;
    private readonly IEventPublisher _eventPublisher;

    public AuthenticationService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IJwtProvider jwtProvider, 
        IRepository<RefreshToken> refreshTokenRepository,
        IUnitOfWork unitOfWork,
        IValidator<LoginDto> loginDtoValidator,
        IValidator<ChangePasswordDto> changePasswordDtoValidator,
        ICacheService cacheService,
        IEventPublisher eventPublisher)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtProvider = jwtProvider;
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
        _loginDtoValidator = loginDtoValidator;
        _changePasswordDtoValidator = changePasswordDtoValidator;
        _cacheService = cacheService;
        _eventPublisher = eventPublisher;
    }

    public async Task<Result<AccessToken>> LoginAsync(LoginDto loginDto)
    {
        var validationResult = await _loginDtoValidator.ValidateAsync(loginDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => DomainErrors.User.CannotCreate(x.ErrorMessage)).ToList();
            return Result.Failure<AccessToken>(errors);
        }

        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
        {
            return Result.Failure<AccessToken>(DomainErrors.Authentication.InvalidEmailOrPassword);
        }
        var checkPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!checkPassword)
        {
            return Result.Failure<AccessToken>(DomainErrors.Authentication.InvalidEmailOrPassword);
        }
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

    public async Task<Result> ChangePassword(Guid userId, ChangePasswordDto changePasswordDto)
    {
        var validationResult = await _changePasswordDtoValidator.ValidateAsync(changePasswordDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => DomainErrors.User.CannotCreate(x.ErrorMessage)).ToList();
            return Result.Failure(errors);
        }

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return Result.Failure(DomainErrors.User.NotFound(userId));
        }

        var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.OldPassword, changePasswordDto.NewPassword);
        if (!result.Succeeded)
        {
            var errors = result.Errors
                .Select(x => DomainErrors.Authentication.CannotChangePassword(x.Description))
                .ToList();
            return Result.Failure(errors);
        }
        return Result.Success();
    }

    public async Task<Result<AccessToken>> CreateTokenByRefreshToken(string refreshToken)
    {
        var existRefreshToken = await _refreshTokenRepository.GetAsync(x => x.Code.Equals(refreshToken));
        if (existRefreshToken is null)
        {
            return Result.Failure<AccessToken>(DomainErrors.RefreshToken.NotFound);
        }
        if (existRefreshToken.Expiration < DateTime.UtcNow)
        {
            return Result.Failure<AccessToken>(DomainErrors.RefreshToken.TokenExpired);
        }

        var user = await _userManager.FindByIdAsync(existRefreshToken.UserId.ToString());
        if (user is null)
        {
            return Result.Failure<AccessToken>(DomainErrors.User.NotFound(existRefreshToken.UserId));
        }
        var token = await _jwtProvider.CreateToken(user);
        existRefreshToken.UpdateToken(token.RefreshToken, token.RefreshTokenExpiration);

        await _unitOfWork.SaveChangesAsync();
        return Result.Success(token);
    }

    public async Task<Result> ExterminateRefreshToken(string refreshToken)
    {
        var existRefreshToken = await _refreshTokenRepository.GetAsync(x => x.Code.Equals(refreshToken));
        if (existRefreshToken is null)
        {
            return Result.Failure<AccessToken>(DomainErrors.RefreshToken.NotFound);
        }

        _refreshTokenRepository.Delete(existRefreshToken);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> EmailConfirmation(EmailConfirmDto emailConfirmDto)
    {
        var user = await _userManager.FindByIdAsync(emailConfirmDto.UserId.ToString());
        if(user is null)
        {
            return Result.Failure(DomainErrors.User.NotFound(emailConfirmDto.UserId));
        }
        var verificationToken = await _cacheService.GetAsync<string>(CachingKeys.EmailVerificationKey(user.Id));
        if (verificationToken is null || !verificationToken.Equals(emailConfirmDto.Token))
        {
            return Result.Failure(DomainErrors.User.EmailConfirmationOTPInvalid);
        }
        user.EmailVerify();

        const string defaultRole = "user";
        var roleExists = await _roleManager.RoleExistsAsync(defaultRole);
        if (!roleExists)
        {
            var roleCreateResult = await _roleManager.CreateAsync(ApplicationRole.Create(defaultRole));
            if (!roleCreateResult.Succeeded)
            {
                var errors = roleCreateResult.Errors
                    .Select(x => DomainErrors.User.CannotCreate(x.Description))
                    .ToList();
                return Result.Failure(errors);
            }
        }

        var roleAssignResult = await _userManager.AddToRoleAsync(user, defaultRole);
        if (!roleAssignResult.Succeeded)
        {
            if (!roleAssignResult.Succeeded)
            {
                var errors = roleAssignResult.Errors
                    .Select(x => DomainErrors.User.CannotCreate(x.Description))
                    .ToList();
                return Result.Failure(errors);
            }
        }

        await _unitOfWork.SaveChangesAsync();
        await _cacheService.RemoveAsync(CachingKeys.EmailVerificationKey(user.Id));
        return Result.Success();
    }
    public async Task<Result> ResendEmailConfirmationToken(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return Result.Failure(DomainErrors.User.NotFound(userId));
        }
        if (user.EmailConfirmed)
        {
            return Result.Failure(DomainErrors.User.EmailAlreadyConfirmed);
        }

        var emailVerificationToken = new Random().Next(100000, 1000000);
        var userRegisteredEvent = new UserRegisteredEvent(user.Id, user.Email, user.FullName, emailVerificationToken);
        await _eventPublisher.PublishAsync(userRegisteredEvent);

        return Result.Success();
    }
}
