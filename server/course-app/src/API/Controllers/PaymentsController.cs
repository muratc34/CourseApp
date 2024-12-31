using API.Extensions;
using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> PaymentCallback([FromForm] IFormCollection collection)
    {
        await _paymentService.Callback(collection["token"]);
        return Ok();
    }
}
