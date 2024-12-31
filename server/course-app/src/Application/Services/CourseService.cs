using Application.Abstractions.Caching;
using Application.Abstractions.Caching.Constants;
using FluentValidation;

namespace Application.Services;
public interface ICourseService
{
    Task<Result<CourseDto>> Create(CourseCreateDto courseCreateDto);
    Task<Result> Update(Guid courseId, CourseUpdateDto courseUpdateDto);
    Task<Result> Delete(Guid courseId);
    Task<Result<List<CourseDetailDto>>> GetCourses(CancellationToken cancellationToken);
    Task<Result<CourseDetailDto>> GetCourseById(Guid courseId);
    Task<Result<List<CourseDetailDto>>> GetCourseByCategoryId(Guid categoryId, CancellationToken cancellationToken);
    Task<Result> RegisterUserToCourse(Guid courseId, Guid userId);
    Task<Result<List<UserCoursesDetailDto>>> GetCoursesByUserId(Guid userId, CancellationToken cancellationToken);
}

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly ICategoryService _categoryService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IValidator<CourseCreateDto> _courseCreateDtoValidator;
    private readonly IValidator<CourseUpdateDto> _courseUpdateDtoValidator;
    private readonly ICacheService _cacheService;

    public CourseService(
        ICourseRepository courseRepository,
        ICategoryService categoryService, 
        IUnitOfWork unitOfWork, 
        UserManager<ApplicationUser> userManager,
        IValidator<CourseCreateDto> courseCreateDtoValidator,
        IValidator<CourseUpdateDto> courseUpdateDtoValidator,
        ICacheService cacheService)
    {
        _courseRepository = courseRepository;
        _unitOfWork = unitOfWork;
        _categoryService = categoryService;
        _userManager = userManager;
        _courseCreateDtoValidator = courseCreateDtoValidator;
        _courseUpdateDtoValidator = courseUpdateDtoValidator;
        _cacheService = cacheService;
    }

    public async Task<Result<CourseDto>> Create(CourseCreateDto courseCreateDto)
    {
        var validationResult = await _courseCreateDtoValidator.ValidateAsync(courseCreateDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => DomainErrors.User.CannotCreate(x.ErrorMessage)).ToList();
            return Result.Failure<CourseDto>(errors);
        }

        var category = await _categoryService.GetCategoryById(courseCreateDto.CategoryId);
        if (category is null)
        {
            return Result.Failure<CourseDto>(DomainErrors.Category.NotFound);
        }

        var course = Course.Create(courseCreateDto.Name, courseCreateDto.Description, courseCreateDto.Price, courseCreateDto.ImageUrl, courseCreateDto.CategoryId, courseCreateDto.InstructorId);

        await _courseRepository.CreateAsync(course);
        await _unitOfWork.SaveChangesAsync();

        await _cacheService.RemoveAsync(CachingKeys.CoursesKey);
        await _cacheService.RemoveAsync(CachingKeys.CoursesByCagetoryIdKey(course.CategoryId));

        return Result.Success(new CourseDto(course.Id, course.CreatedOnUtc, course.ModifiedOnUtc, course.Name, course.Description, course.Price, course.ImageUrl, course.CategoryId, course.InstructorId));
    }

    public async Task<Result> Delete(Guid courseId)
    {
        var course = await _courseRepository.GetAsync(x => x.Id == courseId);
        if(course is null)
        {
            return Result.Failure(DomainErrors.Course.NotFound);
        }
        _courseRepository.Delete(course);
        await _unitOfWork.SaveChangesAsync();

        await _cacheService.RemoveAsync(CachingKeys.CoursesKey);
        await _cacheService.RemoveAsync(CachingKeys.CourseByIdKey(course.Id));
        await _cacheService.RemoveAsync(CachingKeys.CoursesByCagetoryIdKey(course.CategoryId));
        foreach (var item in course.Enrollments)
        {
            await _cacheService.RemoveAsync(CachingKeys.UserCoursesByUserIdKey(item.User.Id));
        }
        return Result.Success();
    }

    public async Task<Result<List<CourseDetailDto>>> GetCourseByCategoryId(Guid categoryId, CancellationToken cancellationToken)
    {
        var cachedCourses = await _cacheService.GetAsync<List<CourseDetailDto>>(CachingKeys.CoursesByCagetoryIdKey(categoryId));
        if (cachedCourses is not null)
        {
            return Result.Success(cachedCourses);
        }

        var category = await _categoryService.GetCategoryById(categoryId);
        if (category is null)
        {
            return Result.Failure<List<CourseDetailDto>>(DomainErrors.Category.NotFound);
        }

        var courses = await _courseRepository.FindAll()
            .Where(x => x.CategoryId == categoryId).
            Select(x => new CourseDetailDto(
                x.Id,
                x.CreatedOnUtc,
                x.ModifiedOnUtc,
                x.Name,
                x.Description,
                x.Price,
                x.ImageUrl,
                new CategoryDto(
                    x.Category.Id,
                    x.Category.CreatedOnUtc,
                    x.Category.ModifiedOnUtc,
                    x.Category.Name),
                new UserDto(
                    x.Instructor.Id,
                    x.Instructor.CreatedOnUtc,
                    x.Instructor.FullName,
                    x.Instructor.Email,
                    x.Instructor.UserName,
                    x.Instructor.ProfilePictureUrl)
            )
            ).ToListAsync(cancellationToken);

        await _cacheService.SetAsync(CachingKeys.CoursesByCagetoryIdKey(categoryId), courses, TimeSpan.FromMinutes(60));
        return Result.Success(courses);
    }

    public async Task<Result<CourseDetailDto>> GetCourseById(Guid courseId)
    {
        var cachedCourse = await _cacheService.GetAsync<CourseDetailDto>(CachingKeys.CourseByIdKey(courseId));
        if (cachedCourse is not null)
        {
            return Result.Success(cachedCourse);
        }

        var course = await _courseRepository.Find(x => x.Id == courseId)
            .Select(course => new CourseDetailDto(
                course.Id,
                course.CreatedOnUtc,
                course.ModifiedOnUtc,
                course.Name,
                course.Description,
                course.Price,
                course.ImageUrl,
                new CategoryDto(
                        course.Category.Id,
                        course.Category.CreatedOnUtc,
                        course.Category.ModifiedOnUtc,
                        course.Category.Name),
                new UserDto(
                    course.Instructor.Id,
                    course.Instructor.CreatedOnUtc,
                    course.Instructor.FullName,
                    course.Instructor.Email,
                    course.Instructor.UserName,
                    course.Instructor.ProfilePictureUrl)
            )).FirstAsync();
        if (course is null)
        {
            return Result.Failure<CourseDetailDto>(DomainErrors.Course.NotFound);
        }
        await _cacheService.SetAsync(CachingKeys.CourseByIdKey(courseId), course, TimeSpan.FromMinutes(60));
        return Result.Success(course);
    }

    public async Task<Result<List<CourseDetailDto>>> GetCourses(CancellationToken cancellationToken)
    {
        var cachedCourses = await _cacheService.GetAsync<List<CourseDetailDto>>(CachingKeys.CoursesKey);
        if(cachedCourses is not null)
        {
            return Result.Success(cachedCourses);
        }

        var courses = await _courseRepository.FindAll()
            .Select(course => new CourseDetailDto(
                course.Id,
                course.CreatedOnUtc,
                course.ModifiedOnUtc,
                course.Name,
                course.Description,
                course.Price,
                course.ImageUrl,
                 new CategoryDto(
                        course.Category.Id,
                        course.Category.CreatedOnUtc,
                        course.Category.ModifiedOnUtc,
                        course.Category.Name),
                new UserDto(
                    course.Instructor.Id,
                    course.Instructor.CreatedOnUtc,
                    course.Instructor.FullName,
                    course.Instructor.Email,
                    course.Instructor.UserName,
                    course.Instructor.ProfilePictureUrl)
            )).ToListAsync(cancellationToken);

        await _cacheService.SetAsync(CachingKeys.CoursesKey, courses, TimeSpan.FromMinutes(60));
        return Result.Success(courses);
    }

    public async Task<Result> Update(Guid courseId, CourseUpdateDto courseUpdateDto)
    {
        var validationResult = await _courseUpdateDtoValidator.ValidateAsync(courseUpdateDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => DomainErrors.Course.CannotUpdate(x.ErrorMessage)).ToList();
            return Result.Failure(errors);
        }

        var course = await _courseRepository.GetAsync(x => x.Id == courseId);
        if (course is null)
        {
            return Result.Failure(DomainErrors.Course.NotFound);
        }
        if(courseUpdateDto.CategoryId != null)
        {
            var category = await _categoryService.GetCategoryById((Guid)courseUpdateDto.CategoryId);
            if (category is null)
            {
                return Result.Failure(DomainErrors.Category.NotFound);
            }
        }
        course.Update(courseUpdateDto.Name, courseUpdateDto.Description, courseUpdateDto.Price, courseUpdateDto.ImageUrl, courseUpdateDto.CategoryId);
        _courseRepository.Update(course);
        await _unitOfWork.SaveChangesAsync();
       
        await _cacheService.RemoveAsync(CachingKeys.CoursesKey);
        await _cacheService.RemoveAsync(CachingKeys.CourseByIdKey(course.Id));
        await _cacheService.RemoveAsync(CachingKeys.CoursesByCagetoryIdKey(course.CategoryId));
        foreach (var item in course.Enrollments)
        {
            await _cacheService.RemoveAsync(CachingKeys.UserCoursesByUserIdKey(item.User.Id));
        }
        return Result.Success();
    }

    public async Task<Result> RegisterUserToCourse(Guid courseId, Guid userId)
    {
        var course = await _courseRepository.GetAsync(x => x.Id == courseId, x => x.Include(x => x.Enrollments));
        if (course is null)
        {
            return Result.Failure(DomainErrors.Course.NotFound);
        }
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return Result.Failure(DomainErrors.User.NotFound(userId));
        }

        course.AddUserToCourse(new Enrollment(userId, courseId));
        _courseRepository.Update(course);
        await _unitOfWork.SaveChangesAsync();
       
        await _cacheService.RemoveAsync(CachingKeys.CoursesKey);
        await _cacheService.RemoveAsync(CachingKeys.CourseByIdKey(course.Id));
        await _cacheService.RemoveAsync(CachingKeys.CoursesByCagetoryIdKey(course.CategoryId));
        await _cacheService.RemoveAsync(CachingKeys.UserCoursesByUserIdKey(user.Id));

        return Result.Success();
    }

    public async Task<Result<List<UserCoursesDetailDto>>> GetCoursesByUserId(Guid userId, CancellationToken cancellationToken)
    {
        var cachedCourses = await _cacheService.GetAsync<List<UserCoursesDetailDto>>(CachingKeys.UserCoursesByUserIdKey(userId));
        if (cachedCourses is not null)
        {
            return Result.Success(cachedCourses);
        }

        var user = await _userManager.Users
            .Where(x => x.Id == userId)
            .Select(user =>
                new UserCoursesDetailDto(
                    user.Id,
                    user.CreatedOnUtc,
                    user.FullName,
                    user.Email,
                    user.UserName,
                    user.ProfilePictureUrl,
                    user.Enrollments.Select(x =>
                        new CourseDetailDto(
                            x.Course.Id,
                            x.Course.CreatedOnUtc,
                            x.Course.ModifiedOnUtc,
                            x.Course.Name,
                            x.Course.Description,
                            x.Course.Price,
                            x.Course.ImageUrl,
                            new CategoryDto(
                                x.Course.Category.Id,
                                x.Course.Category.CreatedOnUtc,
                                x.Course.ModifiedOnUtc,
                                x.Course.Name
                            ),
                            new UserDto(
                                x.Course.Instructor.Id,
                                x.Course.Instructor.CreatedOnUtc,
                                x.Course.Instructor.FullName,
                                x.Course.Instructor.Email,
                                x.Course.Instructor.UserName,
                                x.Course.Instructor.ProfilePictureUrl
                            )
                        )
                ).ToList())).ToListAsync(cancellationToken);

        await _cacheService.SetAsync(CachingKeys.UserCoursesByUserIdKey(userId), user, TimeSpan.FromMinutes(60));
        return Result.Success(user);
    }
}
