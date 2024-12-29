﻿using Application.Abstractions.Caching;
using Application.Abstractions.Caching.Constants;
using FluentValidation;

namespace Application.Services;


public interface ICourseService
{
    Task<Result<CourseDto>> Create(CourseCreateDto courseCreateDto);
    Task<Result> Update(Guid courseId, CourseUpdateDto courseUpdateDto);
    Task<Result> Delete(Guid courseId);
    Task<Result<List<CourseDto>>> GetCourses(CancellationToken cancellationToken);
    Task<Result<CourseDetailDto>> GetCourseById(Guid courseId);
    Task<Result<List<CourseDetailDto>>> GetCourseByCategoryId(Guid categoryId, CancellationToken cancellationToken);
    Task<Result> RegisterUserToCourse(Guid courseId, Guid userId);
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

        var course = await _courseRepository.GetAsync(x => x.Id == courseId);
        if (course is null)
        {
            return Result.Failure<CourseDetailDto>(DomainErrors.Course.NotFound);
        }
        await _cacheService.SetAsync(CachingKeys.CourseByIdKey(courseId), course, TimeSpan.FromMinutes(60));
        return Result.Success(
            new CourseDetailDto(
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
            )
        );

    }

    public async Task<Result<List<CourseDto>>> GetCourses(CancellationToken cancellationToken)
    {
        var cachedCourses = await _cacheService.GetAsync<List<CourseDto>>(CachingKeys.CoursesKey);
        if(cachedCourses is not null)
        {
            return Result.Success(cachedCourses);
        }

        var courses = await _courseRepository.FindAll()
            .Select(x => new CourseDto(
                x.Id,
                x.CreatedOnUtc, 
                x.ModifiedOnUtc, 
                x.Name, 
                x.Description, 
                x.Price, 
                x.ImageUrl, 
                x.CategoryId,
                x.InstructorId)
            ).ToListAsync(cancellationToken);

        await _cacheService.SetAsync(CachingKeys.CoursesKey, courses, TimeSpan.FromMinutes(60));
        return Result.Success(courses);
    }

    public async Task<Result> Update(Guid courseId, CourseUpdateDto courseUpdateDto)
    {
        var validationResult = await _courseUpdateDtoValidator.ValidateAsync(courseUpdateDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => DomainErrors.User.CannotCreate(x.ErrorMessage)).ToList();
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

        return Result.Success();
    }
}
