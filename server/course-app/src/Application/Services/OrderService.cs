namespace Application.Services;

public interface IOrderService
{
    Task<Result<OrderDto>> Create(OrderCreateDto orderCreateDto);
    Task<Result> Delete(Guid categoryId);
    Task<Result<List<OrderDetaiDto>>> GetOrderByUserId(Guid userId, CancellationToken cancellationToken);
}
internal class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;

    public OrderService(
        IOrderRepository orderRepository, 
        ICourseRepository courseRepository,
        IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager)
    {
        _orderRepository = orderRepository;
        _courseRepository = courseRepository;
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

        var order = Order.Create(orderCreateDto.UserId);
        foreach (var courseId in orderCreateDto.CourseIds)
        {
            var course = await _courseRepository.GetAsync(x => x.Id == courseId);
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

    public async Task<Result> Delete(Guid categoryId)
    {
        var order = await _orderRepository.GetAsync(x => x.Id == categoryId);
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

    public async Task<Result<List<OrderDetaiDto>>> GetOrderByUserId(Guid userId, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.FindAll()
            .Where(x => x.UserId == userId)
            .Select(x => new OrderDetaiDto(
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
                ).ToArray()
            )
        ).ToListAsync(cancellationToken);


        return Result.Success(orders);
    }
}