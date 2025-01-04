namespace Application.FluentValidations;

public class RoleSaveDtoValidator: AbstractValidator<RoleSaveDto>
{
    public RoleSaveDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithError(ValidationErrors.RoleSaveDto.NameIsRequired);
    }
}