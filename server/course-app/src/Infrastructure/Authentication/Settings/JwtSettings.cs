namespace Infrastructure.Authentication.Settings;

public class JwtSettings
{
    public const string SettingsKey = "Jwt";
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string SecurityKey { get; set; }
    public int AccessTokenExpirationInMinutes { get; set; }
    public int RefreshTokenExpirationInMinutes { get; set; }
}
