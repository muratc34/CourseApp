namespace Application.Services;


public interface ICourseService
{
    Task<Result<CourseDetailDto>> Create(CourseCreateDto courseCreateDto);
    Task<Result> Update(Guid courseId, CourseUpdateDto courseUpdateDto);
    Task<Result> Delete(Guid courseId);
    Task<Result<List<CourseDto>>> GetCourses(CancellationToken cancellationToken);
    Task<Result<CourseDetailDto>> GetCourseById(Guid courseId);
    Task<Result<List<CourseDto>>> GetCourseByCategoryId(Guid categoryId, CancellationToken cancellationToken);
}

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CourseService(ICourseRepository courseRepository, ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _courseRepository = courseRepository;
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<CourseDetailDto>> Create(CourseCreateDto courseCreateDto)
    {
        var course = Course.Create(courseCreateDto.Name, courseCreateDto.Description, courseCreateDto.Price, courseCreateDto.ImageUrl, courseCreateDto.CategoryId, courseCreateDto.InstructorId);

        var isCategoryExist = await _categoryRepository.GetAsync(x => x.Id == courseCreateDto.CategoryId);
        if (isCategoryExist is null)
        {
            return Result.Failure<CourseDetailDto>(DomainErrors.Category.NotFound);
        }

        await _courseRepository.CreateAsync(course);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success(new CourseDetailDto(course.Id, course.CreatedOnUtc, course.ModifiedOnUtc, course.Name, course.Description, course.Price, course.ImageUrl, course.CategoryId, course.InstructorId));
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
        return Result.Success();
    }

    public async Task<Result<List<CourseDto>>> GetCourseByCategoryId(Guid categoryId, CancellationToken cancellationToken)
    {
        var isCategoryExist = await _categoryRepository.GetAsync(x => x.Id == categoryId);
        if (isCategoryExist is null)
        {
            return Result.Failure<List<CourseDto>>(DomainErrors.Category.NotFound);
        }

        var courses = await _courseRepository.FindAll()
            .Where(x => x.CategoryId == categoryId).
            Select(x => new CourseDto(
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
                    x.Category.Name),
                new UserDto(x.Instructor.Id,
                x.Instructor.CreatedOnUtc,
                x.Instructor.FullName,
                x.Instructor.Email,
                x.Instructor.UserName,
                x.Instructor.ProfilePictureUrl)
            )
            ).ToListAsync(cancellationToken);

        return Result.Success(courses);
    }

    public async Task<Result<CourseDetailDto>> GetCourseById(Guid courseId)
    {
        var course = await _courseRepository.GetAsync(x => x.Id == courseId);
        if (course is null)
        {
            return Result.Failure<CourseDetailDto>(DomainErrors.Course.NotFound);
        }
        return Result.Success(new CourseDetailDto(course.Id, course.CreatedOnUtc, course.ModifiedOnUtc, course.Name, course.Description, course.Price, course.ImageUrl, course.CategoryId, course.InstructorId));
    }

    public async Task<Result<List<CourseDto>>> GetCourses(CancellationToken cancellationToken)
    {
        var courses = await _courseRepository.FindAll()
            .Select(x => new CourseDto(
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
                    x.Category.Name), 
                new UserDto(x.Instructor.Id, 
                x.Instructor.CreatedOnUtc, 
                x.Instructor.FullName, 
                x.Instructor.Email, 
                x.Instructor.UserName, 
                x.Instructor.ProfilePictureUrl)
                )
            ).ToListAsync(cancellationToken);

        return Result.Success(courses);
    }

    public async Task<Result> Update(Guid courseId, CourseUpdateDto courseUpdateDto)
    {
        var course = await _courseRepository.GetAsync(x => x.Id == courseId);
        if (course is null)
        {
            return Result.Failure(DomainErrors.Course.NotFound);
        }
        if(courseUpdateDto.CategoryId != null && courseUpdateDto.CategoryId != Guid.Empty)
        {
            var isCategoryExist = await _categoryRepository.GetAsync(x => x.Id == courseUpdateDto.CategoryId);
            if (isCategoryExist is null)
            {
                return Result.Failure(DomainErrors.Category.NotFound);
            }
        }
        course.Update(courseUpdateDto.Name, courseUpdateDto.Description, courseUpdateDto.Price, courseUpdateDto.ImageUrl, courseUpdateDto.CategoryId);
        _courseRepository.Update(course);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
