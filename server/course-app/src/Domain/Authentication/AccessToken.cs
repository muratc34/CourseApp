namespace Domain.Authentication;

public class AccessToken
{
    public AccessToken(string token, string refreshToken, long tokenExpiration, long refreshTokenExpiration)
    {
        Token = token;
        RefreshToken = refreshToken;
        TokenExpiration = tokenExpiration;
        RefreshTokenExpiration = refreshTokenExpiration;
    }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public long TokenExpiration { get; set; }
    public long RefreshTokenExpiration { get; set; }
}
