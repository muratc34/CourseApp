namespace Infrastructure.Authentication;

public class JwtProvider : IJwtProvider
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtSettings _jwtSettings;
    private long _accessTokenExpiration;

    public JwtProvider(UserManager<ApplicationUser> userManager, IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<AccessToken> CreateToken(ApplicationUser user)
    {
        _accessTokenExpiration = DateTimeOffset.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationInMinutes).ToUnixTimeSeconds();
        var refreshTokenExpiration = DateTimeOffset.UtcNow.AddMinutes(_jwtSettings.RefreshTokenExpirationInMinutes).ToUnixTimeSeconds();
        var securityKey = SecurityKeyHelper.CreateSecurityKey(_jwtSettings.SecurityKey);
        var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
        var jwt = await CreateJwtSecurityToken(user, signingCredentials);

        string token = new JwtSecurityTokenHandler().WriteToken(jwt);
        return new AccessToken(token, CreateRefreshToken(), _accessTokenExpiration, refreshTokenExpiration);
    }

    private async Task<JwtSecurityToken> CreateJwtSecurityToken(ApplicationUser user, SigningCredentials signingCredentials)
    {
        var jwt = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: await GetClaims(user),
            notBefore: DateTime.UtcNow,
            expires: DateTimeOffset.FromUnixTimeSeconds(_accessTokenExpiration).DateTime,
            signingCredentials: signingCredentials
        );
        return jwt;
    }

    private async Task<IEnumerable<Claim>> GetClaims(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.GivenName, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var userRoles = await _userManager.GetRolesAsync(user);
        foreach (var role in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }

    private string CreateRefreshToken()
    {
        var numberByte = new byte[32];
        using var rnd = RandomNumberGenerator.Create();
        rnd.GetBytes(numberByte);
        return Convert.ToBase64String(numberByte);
    }
}
