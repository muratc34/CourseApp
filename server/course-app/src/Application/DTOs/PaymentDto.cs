namespace Application.DTOs;

public record PaymentDto(string Test, string Url);
public record PaymentCreateDto(Guid orderId);

