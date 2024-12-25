namespace Application.Services;

public interface IAuthenticationService
{
    Task<Result<AccessToken>> LoginAsync(LoginDto loginDto);
    Task<Result> ChangePassword(Guid userId, ChangePasswordDto changePasswordDto);
    Task<Result<AccessToken>> CreateTokenByRefreshToken(string refreshToken);
    Task<Result> ExterminateRefreshToken(string refreshToken);
}

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtProvider _jwtProvider;
    private readonly IRepository<RefreshToken> _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AuthenticationService(
        UserManager<ApplicationUser> userManager, 
        IJwtProvider jwtProvider, 
        IRepository<RefreshToken> refreshTokenRepository,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _jwtProvider = jwtProvider;
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AccessToken>> LoginAsync(LoginDto loginDto)
    {
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
        if (existRefreshToken.Expiration < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
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
}
