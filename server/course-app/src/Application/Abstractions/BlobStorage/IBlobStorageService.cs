namespace Application.Abstractions.BlobStorage;

public interface IBlobStorageService
{
    Task RemoveUserImageAsync(Guid userId, string fileUrl, CancellationToken cancellationToken);
    Task RemoveCourseImageAsync(Guid courseId, string fileUrl, CancellationToken cancellationToken);
    Task<string> UploadUserImageFileAsync(Guid userId, string fileExtension, byte[] fileData, CancellationToken cancellationToken);
    Task<string> UploadCourseImageFileAsync(Guid userId, Guid courseId, string fileExtension, byte[] fileData, CancellationToken cancellationToken);
}
