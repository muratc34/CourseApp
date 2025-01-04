namespace Application.Abstractions.Notifications.Contracts;

public sealed class EmailVerification
{
    public EmailVerification(string emailTo, string name, string token)
    {
        EmailTo = emailTo;
        Name = name;
        Token = token;
    }

    public string EmailTo { get; }
    public string Name { get; }
    public string Token { get; set; }
}