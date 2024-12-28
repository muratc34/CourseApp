namespace Application.Services;

public interface IOrderService
{
    Task<Result<OrderDto>> Create(OrderCreateDto orderCreateDto);
    Task<Result> Delete(Guid orderId);
    Task<Result<List<OrderDetailDto>>> GetOrderByUserId(Guid userId, CancellationToken cancellationToken);
    Task<Result<OrderDetailDto>> GetOrderById(Guid orderId);
    Task<Result> UpdateStatusAsCompleted(Guid orderId);
}
internal class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICourseService _courseService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;

    public OrderService(
        IOrderRepository orderRepository,
        ICourseService courseService,
        IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager)
    {
        _orderRepository = orderRepository;
        _courseService = courseService;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<Result<OrderDto>> Create(OrderCreateDto orderCreateDto)
    {
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
            if(user.Enrollments.Any(x => x.CourseId == courseId))
            {
                return Result.Failure<OrderDto>(DomainErrors.User.AlreadyEnrollment);
            }
            order.OrderDetails.Add(new OrderDetail(courseId, order.Id));
        }

        await _orderRepository.CreateAsync(order);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(new OrderDto(order.Id, order.CreatedOnUtc, order.ModifiedOnUtc, order.UserId, order.Status));
    }

    public async Task<Result> Delete(Guid orderId)
    {
        var order = await _orderRepository.GetAsync(x => x.Id == orderId);
        if (order is null)
        {
            return Result.Failure(DomainErrors.Order.NotFound);
        }
        _orderRepository.Delete(order);
        order.SetStatusAsCancelled();
        _orderRepository.Update(order);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<List<OrderDetailDto>>> GetOrderByUserId(Guid userId, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.FindAll()
            .Where(x => x.UserId == userId)
            .Select(x => new OrderDetailDto(
                x.Id,
                x.CreatedOnUtc,
                x.ModifiedOnUtc,
                x.UserId,
                x.Status,
                x.OrderDetails.Select(oc => new CourseDto(
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
                x.TcNo
            )
        ).ToListAsync(cancellationToken);


        return Result.Success(orders);
    }

    public async Task<Result<OrderDetailDto>> GetOrderById(Guid orderId)
    {
        var order = await _orderRepository.Find(x => x.Id == orderId)
            .Select(x => new OrderDetailDto(
                x.Id ,
                x.CreatedOnUtc, 
                x.ModifiedOnUtc,
                x.UserId, 
                x.Status, 
                x.OrderDetails.Select(oc => new CourseDto(
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
                x.TcNo)).FirstAsync();

        if (order is null)
        {
            return Result.Failure<OrderDetailDto>(DomainErrors.Order.NotFound);
        }
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
}