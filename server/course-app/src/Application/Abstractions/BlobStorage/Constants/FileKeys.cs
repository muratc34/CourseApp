namespace Application.Abstractions.BlobStorage.Constants;

public static class FileKeys
{
    public static string UserImageKey(Guid userId, string fileExtension) => $"{userId}-{DateTime.UtcNow.Ticks}{fileExtension}";
    public static string CourseImageKey(Guid courseId, string fileExtension) => $"{courseId}-{DateTime.UtcNow.Ticks}{fileExtension}";
}
