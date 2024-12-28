﻿namespace Application.DTOs;

public record OrderDto(Guid Id, long CreatedOnUtc, long? ModifiedOnUtc, Guid UserId, string Status);
public record OrderDetailDto(Guid Id, long CreatedOnUtc, long? ModifiedOnUtc, Guid UserId, string Status, ICollection<CourseDto> Courses, string City, string Country, string Address, string ZipCode, string TcNo);
public record OrderCreateDto(Guid UserId, ICollection<Guid> CourseIds, string City, string Country, string Address, string ZipCode, string TcNo);
