namespace Infrastructure.Messaging.Settings;

public class MessageBrokerSettings
{
    public const string SettingsKey = "MessageBroker";
    public string Host { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
