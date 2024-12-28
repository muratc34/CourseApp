namespace Application.Abstractions.Iyzico;

public interface IIyzicoService
{
    Task<InitializeCheckoutFormResponseDto> InitializeCheckoutForm(InitializeCheckoutFormDto dto);
    Task<ConfirmResponseDto> ConfirmPayment(string token);
}
