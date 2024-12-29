namespace Domain.Authentication;

public class AccessToken
{
    public AccessToken(string token, string refreshToken, DateTime tokenExpiration, DateTime refreshTokenExpiration)
    {
        Token = token;
        RefreshToken = refreshToken;
        TokenExpiration = tokenExpiration;
        RefreshTokenExpiration = refreshTokenExpiration;
    }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime TokenExpiration { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }
}
