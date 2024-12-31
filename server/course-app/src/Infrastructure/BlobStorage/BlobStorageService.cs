namespace Infrastructure.BlobStorage;

internal class BlobStorageService : IBlobStorageService
{
    private readonly BlobSettings _blobSettings;
    private readonly IAmazonS3 _s3Client;

    public BlobStorageService(IOptions<BlobSettings> blobSettings, IUserContext userContext)
    {
        _blobSettings = blobSettings.Value;

        var credentials = new BasicAWSCredentials(
            _blobSettings.AccessKey,
            _blobSettings.SecretKey);

        _s3Client = new AmazonS3Client(credentials,
            new AmazonS3Config
            {
                ServiceURL = _blobSettings.ServiceUrl
            });
    }

    public async Task RemoveImageAsync(string key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<string> UploadCourseImageFileAsync(Guid userId, Guid courseId, string fileExtension, byte[] fileData, CancellationToken cancellationToken)
    {
        var key = FileKeys.CourseImageKey(courseId, fileExtension);

        var request = new PutObjectRequest
        {
            Key = key,
            InputStream = new MemoryStream(fileData),
            BucketName = _blobSettings.CourseImgsBucketName,
            DisablePayloadSigning = true
        };
        request.Metadata.Add("userId", userId.ToString());
        request.Metadata.Add("courseId", courseId.ToString());

        var response = await _s3Client.PutObjectAsync(request, cancellationToken);

        if (response.HttpStatusCode != HttpStatusCode.OK)
            throw new Exception($"Failed to upload blog image. Status code: {response.HttpStatusCode}");

        return $"{_blobSettings.PublicUrl}/{key}";
    }

    public async Task<string> UploadUserImageFileAsync(Guid userId, string fileExtension, byte[] fileData, CancellationToken cancellationToken)
    {
        var key = FileKeys.UserImageKey(userId, fileExtension);

        var request = new PutObjectRequest
        {
            Key = key,
            InputStream = new MemoryStream(fileData),
            BucketName = _blobSettings.UserImgsBucketName,
            DisablePayloadSigning = true
        };
        request.Metadata.Add("userId", userId.ToString());

        var response = await _s3Client.PutObjectAsync(request, cancellationToken);

        if (response.HttpStatusCode != HttpStatusCode.OK)
            throw new Exception($"Failed to upload blog image. Status code: {response.HttpStatusCode}");

        return $"{_blobSettings.PublicUrl}/{key}";
    }

}
