using Application.Abstractions.Iyzico;
using Application.DTOs;
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
            BaseUrl = _iyzicoSettings.SecretKey,
            SecretKey = _iyzicoSettings.BaseUrl,
        };
    }

    public async Task CreateCheckoutForm(OrderDto orderDto)
    {

        CreateCheckoutFormInitializeRequest request = new CreateCheckoutFormInitializeRequest();
        request.Locale = Locale.TR.ToString();
        request.ConversationId = Guid.NewGuid().ToString();
        request.Price = "30";
        request.PaidPrice = "55";
        request.Currency = Currency.TRY.ToString();
        request.BasketId = "B67832";
        request.PaymentGroup = PaymentGroup.PRODUCT.ToString();
        request.CallbackUrl = "https://www.merchant.com/callback";

        request.EnabledInstallments = new List<int> { 1 };
        List<BasketItem> basketItems = new List<BasketItem>();

        Buyer buyer = new Buyer();
        buyer.Id = "BY789";
        buyer.Name = "John";
        buyer.Surname = "Doe";
        buyer.GsmNumber = "+905350000000";
        buyer.Email = "email@email.com";
        buyer.IdentityNumber = "74300864791";
        buyer.LastLoginDate = "2015-10-05 12:43:35";
        buyer.RegistrationDate = "2013-04-21 15:12:09";
        buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
        buyer.Ip = "85.34.78.112";
        buyer.City = "Istanbul";
        buyer.Country = "Turkey";
        buyer.ZipCode = "34732";
        request.Buyer = buyer;

        Address shippingAddress = new Address();
        shippingAddress.ContactName = "Jane Doe";
        shippingAddress.City = "Istanbul";
        shippingAddress.Country = "Turkey";
        shippingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
        shippingAddress.ZipCode = "34742";
        request.ShippingAddress = shippingAddress;

        Address billingAddress = new Address();
        billingAddress.ContactName = "Jane Doe";
        billingAddress.City = "Istanbul";
        billingAddress.Country = "Turkey";
        billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
        billingAddress.ZipCode = "34742";
        request.BillingAddress = billingAddress;

        BasketItem firstBasketItem = new BasketItem();
        firstBasketItem.Id = "BI101";
        firstBasketItem.Name = "Binocular";
        firstBasketItem.Category1 = "Collectibles";
        firstBasketItem.Category2 = "Accessories";
        firstBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
        firstBasketItem.Price = "30";
        basketItems.Add(firstBasketItem);


        var checkout = await CheckoutFormInitialize.Create(request, _options);
    }
}
