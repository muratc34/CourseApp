namespace Application.DTOs;

public record CourseDto(Guid Id, long CreatedOnUtc, long? ModifiedOnUtc, string Name, string Description, decimal Price, string? ImageUrl, Guid CategoryId, Guid InstructorId);
public record CourseDetailDto(Guid Id, long CreatedOnUtc, long? ModifiedOnUtc, string Name, string Description, decimal Price, string? ImageUrl, CategoryDto Category, UserDto User);
public record CourseCreateDto(string Name, string Description, decimal Price, Guid CategoryId, Guid InstructorId);
public record CourseUpdateDto(string? Name, string? Description, decimal? Price, Guid? CategoryId);
