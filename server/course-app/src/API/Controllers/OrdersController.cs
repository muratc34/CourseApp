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
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Create(OrderCreateDto orderCreateDto)
        {
            var result = await _orderService.Create(orderCreateDto);
            return result.IsSuccess ? CreatedAtAction(nameof(GetById), new { orderId = result.Data.Id }, result.Data) : result.ToProblemDetails();
        }

        [HttpDelete]
        [Route("{orderId}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Delete(Guid orderId)
        {
            var result = await _orderService.Delete(orderId);
            return result.IsSuccess ? NoContent() : result.ToProblemDetails();
        }

        [HttpGet]
        [Route("Users/{userId}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> GetByUserId(Guid userId, CancellationToken cancellationToken)
        {
            var result = await _orderService.GetOrdersByUserId(userId, cancellationToken);
            return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
        }

        [HttpGet]
        [Route("{orderId}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> GetById(Guid orderId)
        {
            var result = await _orderService.GetOrderById(orderId);
            return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
        }
    }
}
