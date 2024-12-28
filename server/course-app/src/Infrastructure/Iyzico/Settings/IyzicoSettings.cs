namespace Infrastructure.Iyzıco.Settings;

public class IyzicoSettings
{
    public const string SettingsKey = "Iyzico";
    public string ApiKey { get; set; }
    public string SecretKey { get; set; }
    public string BaseUrl { get; set; }
    public string CallbackUrl { get; set; }
}
