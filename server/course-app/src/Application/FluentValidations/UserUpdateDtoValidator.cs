using Application.Core.Errors;
using Application.Core.Extensions;
using FluentValidation;

namespace Application.FluentValidations;

public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
{
    public UserUpdateDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithError(ValidationErrors.UserUpdateDto.EmailIsRequired);
        RuleFor(x => x.FirstName).NotEmpty().WithError(ValidationErrors.UserUpdateDto.FirstNameIsRequired);
        RuleFor(x => x.LastName).NotEmpty().WithError(ValidationErrors.UserUpdateDto.LastNameIsRequired);
        RuleFor(x => x.UserName).NotEmpty().WithError(ValidationErrors.UserUpdateDto.UserNameIsRequired);
    }
}