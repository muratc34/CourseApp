namespace Application.FluentValidations;

public class UserCreateDtoValidator : AbstractValidator<UserCreateDto>
{
    public UserCreateDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithError(ValidationErrors.UserCreateDto.EmailIsRequired);
        RuleFor(x => x.FirstName).NotEmpty().WithError(ValidationErrors.UserCreateDto.FirstNameIsRequired);
        RuleFor(x => x.LastName).NotEmpty().WithError(ValidationErrors.UserCreateDto.LastNameIsRequired);
        RuleFor(x => x.Password).NotEmpty().WithError(ValidationErrors.UserCreateDto.PasswordIsRequired);
        RuleFor(x => x.UserName).NotEmpty().WithError(ValidationErrors.UserCreateDto.UserNameIsRequired);
    }
}