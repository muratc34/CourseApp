namespace Application.DTOs;

public record UserDto(Guid Id, long CreatedOnUtc, string FullName, string Email, string UserName, string? ProfilePictureUrl);

public record UserCreateDto(string FirstName, string LastName, string UserName, string Email, string Password, string? ProfilePictureUrl);

public record UserUpdateDto(string? FirstName, string? LastName, string? UserName, string? Email, string? ProfilePictureUrl);
