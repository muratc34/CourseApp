namespace Application.FluentValidations;

public class CourseUpdateDtoValidator : AbstractValidator<CourseUpdateDto>
{
    public CourseUpdateDtoValidator()
    {
        RuleFor(x => x.Price).GreaterThan(0).WithError(ValidationErrors.CourseCreateDto.PriceMustBeGreaterThanZero);
    }
}