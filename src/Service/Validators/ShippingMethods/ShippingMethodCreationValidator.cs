using FluentValidation;
using Service.DTOs.ShippingMethods;

namespace Service.Validators.ShippingMethods;

public class ShippingMethodCreationValidator : AbstractValidator<ShippingMethodCreationDto>
{
    public ShippingMethodCreationValidator()
    {
        RuleFor(sm => sm.Name)
            .NotEmpty()
            .Length(4, 100);

        RuleFor(sm => sm.Price)
            .NotEmpty();
    }
}
