namespace Application.FluentValidations;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty()
            .WithError(ValidationErrors.LoginDto.EmailRequired);

        RuleFor(x => x.Password).NotEmpty()
            .WithError(ValidationErrors.LoginDto.PasswordIsRequired);
    }
}
