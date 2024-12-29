namespace Domain.Authentication;

public class RefreshToken : Entity, ISoftDeletableEntity, IAuditableEntity
{
    public RefreshToken(Guid userId, string code, DateTime expiration) : base()
    {
        Ensure.NotEmpty(userId, "The user id is requreid", nameof(userId));
        Ensure.NotEmpty(code, "The code is requreid", nameof(code));

        UserId = userId;
        Code = code;
        Expiration = expiration;
    }

    public long CreatedOnUtc { get; }
    public long? ModifiedOnUtc { get; }
    public long? DeletedOnUtc { get; }
    public bool Deleted { get; }

    public Guid UserId { get; private set; }
    public string Code { get; private set; }
    public DateTime Expiration { get; private set; }

    public static RefreshToken Create(Guid userId, string code, DateTime expiration) 
    {
        return new RefreshToken(userId, code, expiration);
    }

    public void UpdateToken(string code, DateTime expiration)
    {
        Ensure.NotEmpty(code, "The code is requreid", nameof(code));

        Code = code;
        Expiration = expiration;
    }
}
