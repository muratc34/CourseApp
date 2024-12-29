namespace Application.Abstractions.Notifications.Contracts;

public sealed class WelcomeEmail
{
    public WelcomeEmail(string emailTo, string name)
    {
        EmailTo = emailTo;
        Name = name;
    }

    public string EmailTo { get; }
    public string Name { get; }
}
