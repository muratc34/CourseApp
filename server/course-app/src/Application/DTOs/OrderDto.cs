namespace Application.DTOs;

public record OrderDto(Guid Id, long CreatedOnUtc, long? ModifiedOnUtc, Guid UserId, string Status);
public record OrderDetaiDto(Guid Id, long CreatedOnUtc, long? ModifiedOnUtc, Guid UserId, string Status, ICollection<CourseDto> Courses);
public record OrderCreateDto(Guid UserId, ICollection<Guid> CourseIds);
