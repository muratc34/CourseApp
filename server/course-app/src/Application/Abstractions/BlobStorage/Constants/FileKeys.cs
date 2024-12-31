namespace Application.Abstractions.BlobStorage.Constants;

public static class FileKeys
{
    public static string UserImageKey(Guid userId, string fileExtension) => $"{userId}{fileExtension}";
    public static string CourseImageKey(Guid courseId, string fileExtension) => $"{courseId}{fileExtension}";
}
