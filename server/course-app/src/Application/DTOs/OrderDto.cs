namespace Application.DTOs;

public record OrderDto(Guid Id, long CreatedOnUtc, long? ModifiedOnUtc, Guid UserId, string Status, string City, string Country, string Address, string ZipCode, string TcNo);
public record OrderDetailDto(Guid Id, long CreatedOnUtc, long? ModifiedOnUtc, UserDto User, string Status, ICollection<CourseDetailDto> Courses, string City, string Country, string Address, string ZipCode, string TcNo, decimal Amount);
public record OrderCreateDto(Guid UserId, ICollection<Guid> CourseIds, string City, string Country, string Address, string ZipCode, string TcNo);
