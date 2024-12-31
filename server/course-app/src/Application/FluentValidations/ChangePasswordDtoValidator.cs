namespace Application.FluentValidations;

public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
{
    public ChangePasswordDtoValidator()
    {
        RuleFor(x => x.OldPassword).NotEmpty().WithError(ValidationErrors.ChangePasswordDto.OldPasswordIsRequired);
        RuleFor(x => x.NewPassword).NotEmpty().WithError(ValidationErrors.ChangePasswordDto.NewPasswordIsRequired);
    }
}