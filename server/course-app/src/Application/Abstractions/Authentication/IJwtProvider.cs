namespace Application.Abstractions.Authentication;

public interface IJwtProvider
{
    Task<AccessToken> CreateToken(ApplicationUser user);
}
