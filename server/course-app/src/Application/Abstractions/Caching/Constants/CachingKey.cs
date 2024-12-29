namespace Application.Abstractions.Caching.Constants;

public static class CachingKey
{
    public static string EmailVerificationKey(Guid userId) => $"emailverification:{userId.ToString()}";
}
