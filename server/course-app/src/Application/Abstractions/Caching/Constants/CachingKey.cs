namespace Application.Abstractions.Caching.Constants;

public static class CachingKeys
{
    public static string EmailVerificationKey(Guid userId) => $"emailverification:{userId}";
    public static string CategoriesKey => $"categories";
}
