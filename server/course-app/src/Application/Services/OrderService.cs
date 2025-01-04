namespace Application.Services;

public interface IOrderService
{
    Task<Result<OrderDto>> Create(OrderCreateDto orderCreateDto);
    Task<Result> Delete(Guid orderId);
    Task<Result<PagedList<OrderDetailDto>>> GetOrdersByUserId(Guid userId, int pageIndex, int pageSize, CancellationToken cancellationToken);
    Task<Result<OrderDetailDto>> GetOrderById(Guid orderId);
    Task<Result> UpdateStatusAsCompleted(Guid orderId);
    Task<Result> UpdateStatusAsFailed(Guid orderId);
}
internal class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICourseService _courseService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IValidator<OrderCreateDto> _orderCreateDtoValidator;
    private readonly IUserContext _userContext;

    public OrderService(
        IOrderRepository orderRepository,
        ICourseService courseService,
        IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager,
        IValidator<OrderCreateDto> orderCreateDtoValidator,
        IUserContext userContext)
    {
        _orderRepository = orderRepository;
        _courseService = courseService;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _orderCreateDtoValidator = orderCreateDtoValidator;
        _userContext = userContext;
    }

    public async Task<Result<OrderDto>> Create(OrderCreateDto orderCreateDto)
    {
        if (orderCreateDto.UserId == Guid.Empty || orderCreateDto.UserId != _userContext.UserId)
        {
            return Result.Failure<OrderDto>(DomainErrors.Authentication.InvalidPermissions);
        }
        var validationResult = await _orderCreateDtoValidator.ValidateAsync(orderCreateDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => DomainErrors.User.CannotCreate(x.ErrorMessage)).ToList();
            return Result.Failure<OrderDto>(errors);
        }
        var user = await _userManager.Users.Where(u => u.Id == orderCreateDto.UserId).Include(u => u.Enrollments).FirstOrDefaultAsync();
        if (user is null)
        {
            return Result.Failure<OrderDto>(DomainErrors.User.NotFound(orderCreateDto.UserId));
        }

        var order = Order.Create(orderCreateDto.UserId, orderCreateDto.TcNo, orderCreateDto.City, orderCreateDto.Country, orderCreateDto.Address, orderCreateDto.ZipCode);
        foreach (var courseId in orderCreateDto.CourseIds)
        {
            var course = await _courseService.GetCourseById(courseId);
            if (course == null)
            {
                return Result.Failure<OrderDto>(DomainErrors.Course.NotFound);
            }
            if(course.Data.User.Id == user.Id)
            {
                return Result.Failure<OrderDto>(DomainErrors.Course.OwnerCannotOrder);
            }
            if(user.Enrollments.Any(x => x.CourseId == courseId))
            {
                return Result.Failure<OrderDto>(DomainErrors.User.AlreadyEnrollment);
            }
            var hasPreviousOrder = await _orderRepository.FindAll()
                .Where(x => x.UserId == user.Id && (x.Status != OrderStatuses.Cancelled || x.Status != OrderStatuses.Failed || x.Status != OrderStatuses.Refunded))
                .AnyAsync(x => x.OrderDetails.Any(o => o.CourseId == courseId));
            if (hasPreviousOrder)
            {
                return Result.Failure<OrderDto>(DomainErrors.Order.AlreadyOrdered);
            }

            order.OrderDetails.Add(new OrderDetail(courseId, order.Id));
        }

        await _orderRepository.CreateAsync(order);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success(new OrderDto(order.Id, order.CreatedOnUtc, order.ModifiedOnUtc, order.UserId, order.Status, order.City, order.Country, order.Address, order.ZipCode, order.TcNo));
    }

    public async Task<Result> Delete(Guid orderId)
    {
        var order = await _orderRepository.GetAsync(x => x.Id == orderId);
        if (order is null)
        {
            return Result.Failure(DomainErrors.Order.NotFound);
        }
        if (order.UserId == Guid.Empty || order.UserId != _userContext.UserId)
        {
            return Result.Failure<OrderDto>(DomainErrors.Authentication.InvalidPermissions);
        }

        order.SetStatusAsCancelled();
        _orderRepository.Delete(order);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result<PagedList<OrderDetailDto>>> GetOrdersByUserId(Guid userId, int pageIndex, int pageSize, CancellationToken cancellationToken)
    {
        if (userId == Guid.Empty || userId != _userContext.UserId)
        {
            return Result.Failure<PagedList<OrderDetailDto>>(DomainErrors.Authentication.InvalidPermissions);
        }
        var pagedOrders = await _orderRepository.GetAllByPagingAsync(pageIndex, pageSize, cancellationToken,
            x => new OrderDetailDto(
                x.Id,
                x.CreatedOnUtc,
                x.ModifiedOnUtc,
                new UserDto(x.User.Id, x.User.CreatedOnUtc, x.User.FullName, x.User.Email, x.User.UserName, x.User.ProfilePictureUrl),
                x.Status,
                x.OrderDetails.Select(oc => new CourseDetailDto(
                    oc.Course.Id,
                    oc.Course.CreatedOnUtc,
                    oc.Course.ModifiedOnUtc,
                    oc.Course.Name,
                    oc.Course.Description,
                    oc.Course.Price,
                    oc.Course.ImageUrl,
                    new CategoryDto(
                        oc.Course.Category.Id,
                        oc.Course.Category.CreatedOnUtc,
                        oc.Course.Category.ModifiedOnUtc,
                        oc.Course.Category.Name),
                    new UserDto(
                        oc.Course.Instructor.Id,
                        oc.Course.Instructor.CreatedOnUtc,
                        oc.Course.Instructor.FullName,
                        oc.Course.Instructor.Email,
                        oc.Course.Instructor.UserName,
                        oc.Course.Instructor.ProfilePictureUrl)
                    )
                ).ToArray(),
                x.City,
                x.Country,
                x.Address,
                x.ZipCode,
                x.TcNo,
                x.OrderDetails.Sum(od => od.Course.Price)
            ),x => x.UserId == userId, x => x.OrderByDescending(o => o.CreatedOnUtc));
        
        return Result.Success(pagedOrders);
    }

    public async Task<Result<OrderDetailDto>> GetOrderById(Guid orderId)
    {
        var isExistorder = _orderRepository.Find(x => x.Id == orderId);
        if (isExistorder is null)
        {
            return Result.Failure<OrderDetailDto>(DomainErrors.Order.NotFound);
        }
        if (isExistorder.First().UserId == Guid.Empty || isExistorder.First().UserId != _userContext.UserId)
        {
            return Result.Failure<OrderDetailDto>(DomainErrors.Authentication.InvalidPermissions);
        }

        var order = await isExistorder.Select(x => new OrderDetailDto(
                x.Id ,
                x.CreatedOnUtc, 
                x.ModifiedOnUtc,
                new UserDto(x.User.Id, x.User.CreatedOnUtc, x.User.FullName, x.User.Email, x.User.UserName, x.User.ProfilePictureUrl), 
                x.Status, 
                x.OrderDetails.Select(oc => new CourseDetailDto(
                    oc.Course.Id,
                    oc.Course.CreatedOnUtc,
                    oc.Course.ModifiedOnUtc,
                    oc.Course.Name,
                    oc.Course.Description,
                    oc.Course.Price,
                    oc.Course.ImageUrl,
                    new CategoryDto(
                        oc.Course.Category.Id,
                        oc.Course.Category.CreatedOnUtc,
                        oc.Course.Category.ModifiedOnUtc,
                        oc.Course.Category.Name),
                    new UserDto(
                        oc.Course.Instructor.Id,
                        oc.Course.Instructor.CreatedOnUtc,
                        oc.Course.Instructor.FullName,
                        oc.Course.Instructor.Email,
                        oc.Course.Instructor.UserName,
                        oc.Course.Instructor.ProfilePictureUrl)
                    )
                ).ToArray(),
                x.City,
                x.Country,
                x.Address,
                x.ZipCode,
                x.TcNo,
                x.OrderDetails.Sum(od => od.Course.Price))).FirstAsync();
        
        return Result.Success(order);
    }

    public async Task<Result> UpdateStatusAsCompleted(Guid orderId)
    {
        var order = await _orderRepository.GetAsync(x => x.Id == orderId);
        if (order is null)
        {
            return Result.Failure(DomainErrors.Order.NotFound);
        }

        order.SetStatusAsCompleted();
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> UpdateStatusAsFailed(Guid orderId)
    {
        var order = await _orderRepository.GetAsync(x => x.Id == orderId);
        if (order is null)
        {
            return Result.Failure(DomainErrors.Order.NotFound);
        }

        order.SetStatusAsFailed();
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}