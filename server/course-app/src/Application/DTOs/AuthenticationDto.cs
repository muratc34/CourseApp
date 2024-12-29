namespace Application.DTOs;

public record LoginDto(string Email, string Password);

public record ChangePasswordDto(string OldPassword, string NewPassword);