namespace Application.DTOs;

public record UserDto(Guid Id, long CreatedOnUtc, string FullName, string Email, string UserName, string? ProfilePictureUrl);
public record UserDetailDto(Guid Id, long CreatedOnUtc, string FullName, string Email, string UserName, string? ProfilePictureUrl);
public record UserCoursesDetailDto(Guid Id, long CreatedOnUtc, string FullName, string Email, string UserName, string? ProfilePictureUrl, List<CourseDetailDto> Courses);
public record UserCreateDto(string FirstName, string LastName, string UserName, string Email, string Password);
public record UserUpdateDto(string? FirstName, string? LastName, string? UserName, string? Email);
