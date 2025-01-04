namespace API.Settings;

public class AppSettings
{
    public CorsSettings Cors { get; set; }
    public sealed class CorsSettings
    {
        public string[] Origins { get; set; }
        public string[] Headers { get; set; }
        public string[] Methods { get; set; }
    }
}
