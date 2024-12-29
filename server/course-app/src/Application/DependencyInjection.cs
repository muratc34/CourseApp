using Application.FluentValidations;
using FluentValidation;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {

        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<IValidator<CategorySaveDto>, CategorySaveDtoValidator>();
        services.AddScoped<IValidator<ChangePasswordDto>, ChangePasswordDtoValidator>();
        services.AddScoped<IValidator<CourseCreateDto>, CourseCreateDtoValidator>();
        services.AddScoped<IValidator<CourseUpdateDto>, CourseUpdateDtoValidator>();
        services.AddScoped<IValidator<LoginDto>, LoginDtoValidator>();
        services.AddScoped<IValidator<OrderCreateDto>, OrderCreateDtoValidator>();
        services.AddScoped<IValidator<PaymentCreateDto>, PaymentCreateDtoValidator>();
        services.AddScoped<IValidator<UserCreateDto>, UserCreateDtoValidator>();
        services.AddScoped<IValidator<UserUpdateDto>, UserUpdateDtoValidator>();


        return services;
    }
}