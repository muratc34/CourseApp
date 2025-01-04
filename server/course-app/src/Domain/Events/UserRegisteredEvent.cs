namespace Domain.Events;

public class UserRegisteredEvent : BaseEvent
{
    public UserRegisteredEvent(Guid userId, string email, string fullName, int token)
    {
        UserId = userId;
        Email = email;
        FullName = fullName;
        Token = token;
    }

    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public int Token { get; set; }
}
