using Application.Abstractions.Iyzico;
using Infrastructure.Iyzıco.Settings;
using Iyzipay.Model;
using Iyzipay.Request;

namespace Infrastructure.Iyzıco;

internal class IyzicoService : IIyzicoService
{
    private readonly IyzicoSettings _iyzicoSettings;
    private readonly Iyzipay.Options _options;

    public IyzicoService(IOptions<IyzicoSettings> options)
    {
        _iyzicoSettings = options.Value;
        _options = new Iyzipay.Options
        {
            ApiKey = _iyzicoSettings.ApiKey,
            SecretKey = _iyzicoSettings.SecretKey,
            BaseUrl = _iyzicoSettings.BaseUrl,
        };
    }

    public async Task<InitializeCheckoutFormResponseDto> InitializeCheckoutForm(InitializeCheckoutFormDto dto)
    {
        CreateCheckoutFormInitializeRequest request = new CreateCheckoutFormInitializeRequest();
        var totalPrice = dto.basketItems.Sum(x => x.Price);

        request.Price = totalPrice.ToString();
        request.PaidPrice = (totalPrice * 120 / 100).ToString();
        request.Currency = Currency.TRY.ToString();
        request.BasketId = dto.OrderId.ToString();
        request.CallbackUrl = _iyzicoSettings.CallbackUrl;

        Buyer buyer = new Buyer();
        buyer.Id = dto.UserId.ToString();
        buyer.Name = dto.FirstName;
        buyer.Surname = dto.LastName;
        buyer.Email = dto.Email;
        buyer.IdentityNumber = dto.TcNo;
        buyer.RegistrationAddress = dto.Address;
        buyer.Ip = "85.34.78.112";
        buyer.City = dto.City;
        buyer.Country = dto.Country;
        request.Buyer = buyer;

        Address billingAddress = new Address();
        billingAddress.ContactName = $"{dto.FirstName} {dto.LastName}";
        billingAddress.City = dto.City;
        billingAddress.Country = dto.Country;
        billingAddress.Description = dto.Address;
        billingAddress.ZipCode = dto.ZipCode;
        request.BillingAddress = billingAddress;

        request.BasketItems = new List<BasketItem>();
        foreach (var item in dto.basketItems)
        {
            var basketItem = new BasketItem()
            {
                Id = item.Id.ToString(),
                Name = item.Name,
                Category1 = item.Category1,
                ItemType = BasketItemType.VIRTUAL.ToString(),
                Price = item.Price.ToString()
            };
            request.BasketItems.Add(basketItem);
        }

        CheckoutFormInitialize checkoutFormInitialize = await CheckoutFormInitialize.Create(request, _options);
        return new InitializeCheckoutFormResponseDto(checkoutFormInitialize.Token, checkoutFormInitialize.PaymentPageUrl);
    }

    public async Task<ConfirmResponseDto> ConfirmPayment(string token)
    {
        RetrieveCheckoutFormRequest request = new RetrieveCheckoutFormRequest();
        request.Token = token;
        CheckoutForm checkoutForm = await CheckoutForm.Retrieve(request, _options);
        return new ConfirmResponseDto(checkoutForm.BasketId, checkoutForm.PaymentStatus, checkoutForm.PaymentId, checkoutForm.PaidPrice);
    }
}
