namespace Application.DTOs;

public record RoleDto(Guid Id, long CreatedOnUtc, long? ModifiedOnUtc, string Name);
public record RoleSaveDto(string Name);
