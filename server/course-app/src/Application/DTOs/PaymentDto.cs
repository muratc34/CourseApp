namespace Application.DTOs;

public record PaymentDto(string Token, string Url);
public record PaymentCreateDto(Guid OrderId);
public record PaymentConfirm(string Token);

