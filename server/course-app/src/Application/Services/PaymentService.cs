using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Application.Services;

public interface IPaymentService
{
    Task<PaymentDto> Create(PaymentCreateDto paymentCreateDto);
}
internal class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;

    public Task<PaymentDto> Create(PaymentCreateDto paymentCreateDto)
    {
        throw new NotImplementedException();
    }
}
