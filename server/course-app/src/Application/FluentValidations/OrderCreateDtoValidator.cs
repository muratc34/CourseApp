using Application.Core.Errors;
using Application.Core.Extensions;
using FluentValidation;

namespace Application.FluentValidations;

public class OrderCreateDtoValidator : AbstractValidator<OrderCreateDto>
{
    public OrderCreateDtoValidator()
    {
        RuleFor(x => x.UserId).NotEmpty()
            .WithError(ValidationErrors.OrderCreateDto.UserIdRequired);

        RuleFor(x => x.CourseIds).NotEmpty()
            .WithError(ValidationErrors.OrderCreateDto.CourseIdsRequired);

        RuleFor(x => x.City).NotEmpty()
            .WithError(ValidationErrors.OrderCreateDto.CityIsRequired);
            
        RuleFor(x => x.Country).NotEmpty()
            .WithError(ValidationErrors.OrderCreateDto.CountryIsRequired);

        RuleFor(x => x.Address).NotEmpty()
            .WithError(ValidationErrors.OrderCreateDto.AddressIsRequired);

        RuleFor(x => x.ZipCode).NotEmpty()
            .WithError(ValidationErrors.OrderCreateDto.ZipCodeIsRequired)
            .Matches(@"^\d{5}$")
            .WithMessage("Zip code must be a 5-digit number.");

        RuleFor(x => x.TcNo).NotEmpty()
            .WithError(ValidationErrors.OrderCreateDto.TcNoIsRequired)
            .Length(11)
            .WithError(ValidationErrors.OrderCreateDto.TcNoLength)
            .Matches(@"^\d+$")
            .WithError(ValidationErrors.OrderCreateDto.TcNoDigitsMustBeNumber); ;
    }
}