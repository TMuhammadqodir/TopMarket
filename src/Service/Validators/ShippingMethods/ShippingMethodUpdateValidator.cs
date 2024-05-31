using FluentValidation;
using Service.DTOs.ShippingMethods;

namespace Service.Validators.ShippingMethods;

public class ShippingMethodUpdateValidator : AbstractValidator<ShippingMethodUpdateDto>
{
    public ShippingMethodUpdateValidator()
    {
        RuleFor(sm => sm.Id)
            .NotEmpty();

        RuleFor(sm => sm.Name)
            .NotEmpty()
            .Length(4, 100);

        RuleFor(sm => sm.Price)
            .NotEmpty();
    }
}