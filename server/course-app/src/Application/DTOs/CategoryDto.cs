namespace Application.DTOs;

public sealed record CategoryDto(Guid Id, long CreatedOnUtc, string Name);
public sealed record CategoryCreateDto(string Name);
public sealed record CategoryUpdateDto(string Name);
