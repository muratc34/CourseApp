namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> Create(PaymentCreateDto paymentCreateDto)
    {
        var result = await _paymentService.Create(paymentCreateDto);
        return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
    }

    [HttpPost]
    [Route("Callback")]
    public IActionResult PaymentCallback([FromForm] IFormCollection collection)
    {
        var token = collection["token"];
        return Redirect($"http://localhost:5173/payment-result?token={token}");
    }

    [HttpPost]
    [Route("CheckoutConfirm")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> PaymentCheckoutConfirm(PaymentConfirm paymentConfirm)
    {
        var result = await _paymentService.Callback(paymentConfirm.Token);
        return result.IsSuccess ? NoContent() : result.ToProblemDetails();
    }
}
