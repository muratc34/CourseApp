namespace Application.FluentValidations;

public class PaymentCreateDtoValidator : AbstractValidator<PaymentCreateDto>
{
    public PaymentCreateDtoValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty().WithError(ValidationErrors.PaymentCreateDto.OrderIdIsRequired);
    }
}