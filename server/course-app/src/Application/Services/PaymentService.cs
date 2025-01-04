namespace Application.Services;

public interface IPaymentService
{
    Task<Result<PaymentDto>> Create(PaymentCreateDto paymentCreateDto);
    Task<Result> Callback(string token);
}
internal class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IOrderService _orderService;
    private readonly IIyzicoService _iyzicoService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<PaymentCreateDto> _paymentCreateDtoValidator;
    private readonly ICourseService _courseService;
    private readonly IEventPublisher _eventPublisher;
    private readonly IUserContext _userContext;

    public PaymentService(
        IPaymentRepository paymentRepository, 
        IIyzicoService iyzicoService,
        IOrderService orderService,
        UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork,
        IValidator<PaymentCreateDto> paymentCreateDtoValidator,
        ICourseService courseService,
        IEventPublisher eventPublisher,
        IUserContext userContext)
    {
        _paymentRepository = paymentRepository;
        _iyzicoService = iyzicoService;
        _orderService = orderService;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _paymentCreateDtoValidator = paymentCreateDtoValidator;
        _courseService = courseService;
        _eventPublisher = eventPublisher;
        _userContext = userContext;
    }

    public async Task<Result<PaymentDto>> Create(PaymentCreateDto paymentCreateDto)
    {
        var validationResult = await _paymentCreateDtoValidator.ValidateAsync(paymentCreateDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => DomainErrors.User.CannotCreate(x.ErrorMessage)).ToList();
            return Result.Failure<PaymentDto>(errors);
        }
        var order = (await _orderService.GetOrderById(paymentCreateDto.OrderId)).Data;
        if (order is null)
        {
            return Result.Failure<PaymentDto>(DomainErrors.Order.NotFound);
        }
        if(order.User.Id == Guid.Empty || order.User.Id != _userContext.UserId)
        {
            return Result.Failure<PaymentDto>(DomainErrors.Authentication.InvalidPermissions);
        }
        if (order.Status == OrderStatuses.Completed)
        {
            return Result.Failure<PaymentDto>(DomainErrors.Payment.AlreadyPaid);
        }

        var user = await _userManager.FindByIdAsync(order.User.Id.ToString());
        var basketItems = new List<BasketItemDto>();
        foreach (var item in order.Courses)
        {
            var basketItem = new BasketItemDto(item.Id, item.Name, item.Category.Name, item.Price);
            basketItems.Add(basketItem);
        }

        var response = await _iyzicoService.InitializeCheckoutForm(
            new InitializeCheckoutFormDto(
                order.Id,
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                order.TcNo,
                order.Address,
                order.City,
                order.Country,
                order.ZipCode,
                basketItems
                ));

        return Result.Success(new PaymentDto(response.Token, response.PaymentPageUrl));
    }

    public async Task<Result> Callback(string token)
    {
        var confirm = await _iyzicoService.ConfirmPayment(token);
        var order = await _orderService.GetOrderById(new Guid(confirm.BasketId));
        if (order == null || order.Data == null)
        {
            return Result.Failure(DomainErrors.Order.NotFound);
        }
        if (order.Data.User.Id == Guid.Empty || order.Data.User.Id != _userContext.UserId)
        {
            return Result.Failure<PaymentDto>(DomainErrors.Authentication.InvalidPermissions);
        }
        if (!confirm.PaymentStatus.Equals("SUCCESS"))
        {
            await _orderService.UpdateStatusAsFailed(order.Data.Id);
            return Result.Failure(DomainErrors.Payment.Failed);

        }
        await _orderService.UpdateStatusAsCompleted(order.Data.Id);

        foreach (var course in order.Data.Courses)
        {
            await _courseService.RegisterUserToCourse(course.Id, order.Data.User.Id);
        }

        var payment = Payment.Create(new Guid(confirm.BasketId), confirm.PaymentReference, Convert.ToDecimal(confirm.PaidPrice));
        await _paymentRepository.CreateAsync(payment);
        await _unitOfWork.SaveChangesAsync();

        foreach (var course in order.Data.Courses)
        {
            var coursePurchaseEvent = new CoursePurchasedEvent(course.Name, order.Data.User.FullName, course.User.FullName, course.User.Email);
            await _eventPublisher.PublishAsync(coursePurchaseEvent);
        }
        return Result.Success();
    }
}
