namespace Application.Abstractions.Iyzico;


public record InitializeCheckoutFormDto(Guid OrderId, Guid UserId, string FirstName, string LastName, string Email, string TcNo, string Address, string City, string Country, string ZipCode, ICollection<BasketItemDto> basketItems);
public record BasketItemDto(Guid Id, string Name, string Category1, decimal Price);

public record InitializeCheckoutFormResponseDto(string Token, string PaymentPageUrl);

public record ConfirmResponseDto(string BasketId, string PaymentStatus, string PaymentReference, string PaidPrice);