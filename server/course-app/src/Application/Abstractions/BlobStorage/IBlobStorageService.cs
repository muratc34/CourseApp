namespace Application.Abstractions.BlobStorage;

public interface IBlobStorageService
{
    Task<string> UploadUserImageFileAsync(Guid userId, string fileExtension, byte[] fileData, CancellationToken cancellationToken);
    Task<string> UploadCourseImageFileAsync(Guid userId, Guid courseId, string fileExtension, byte[] fileData, CancellationToken cancellationToken);
    Task RemoveImageAsync(string key, CancellationToken cancellationToken);
}
