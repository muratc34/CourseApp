namespace Infrastructure.BlobStorage.Settings;

public class BlobSettings
{
    public const string SettingsKey = "CloudflareR2";
    public string ServiceUrl { get; set; }
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
    public string CourseImgsBucketName { get; set; }
    public string UserImgsBucketName { get; set; }
    public string PublicCourseUrl { get; set; }
    public string PublicUserUrl { get; set; }
}
