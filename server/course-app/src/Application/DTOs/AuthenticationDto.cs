namespace Application.DTOs;

public record LoginDto(string Email, string Password);

public record ChangePasswordDto(string OldPassword, string NewPassword);

public record EmailConfirmDto(Guid UserId, string Token);
public record RefreshTokenDto(string Token);