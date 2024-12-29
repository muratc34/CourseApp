using Application.Core.Errors;
using Application.Core.Extensions;
using FluentValidation;

namespace Application.FluentValidations;

public class CourseUpdateDtoValidator : AbstractValidator<CourseUpdateDto>
{
    public CourseUpdateDtoValidator()
    {
        RuleFor(x => x.CategoryId).NotEmpty().WithError(ValidationErrors.CourseCreateDto.NameIsRequired);
        RuleFor(x => x.Description).NotEmpty().WithError(ValidationErrors.CourseCreateDto.NameIsRequired);
        RuleFor(x => x.Name).NotEmpty().WithError(ValidationErrors.CourseCreateDto.NameIsRequired);
        RuleFor(x => x.Price).NotEmpty().WithError(ValidationErrors.CourseCreateDto.NameIsRequired)
            .GreaterThan(0).WithError(ValidationErrors.CourseCreateDto.PriceMustBeGreaterThanZero);
    }
}