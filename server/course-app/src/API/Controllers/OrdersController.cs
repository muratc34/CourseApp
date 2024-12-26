using API.Extensions;
using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateDto orderCreateDto)
        {
            var result = await _orderService.Create(orderCreateDto);
            return result.IsSuccess ? Created(nameof(result.Data.Id), result) : result.ToProblemDetails();
        }

        [HttpDelete]
        [Route("{orderId}")]
        public async Task<IActionResult> Delete(Guid orderId)
        {
            var result = await _orderService.Delete(orderId);
            return result.IsSuccess ? NoContent() : result.ToProblemDetails();
        }

        [HttpGet]
        [Route("Users/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId, CancellationToken cancellationToken)
        {
            var result = await _orderService.GetOrderByUserId(userId, cancellationToken);
            return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
        }
    }
}
