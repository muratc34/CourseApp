namespace Application.DTOs;

public sealed record CategoryDto(Guid Id, long CreatedOnUtc, long? ModifiedOnUtc, string Name);
public sealed record CategorySaveDto(string Name);
