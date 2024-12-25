namespace Infrastructure.Authentication.Encryption;

public static class SecurityKeyHelper
{
    public static SecurityKey CreateSecurityKey(string securityKey)
    {
        return new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(securityKey));
    }
}
