using FluentValidation;
using Service.DTOs.VariationOptions;

namespace Service.Validators.VaritionOptions;

public class VariationOptionUpdateValidator : AbstractValidator<VariationOptionUpdateDto>
{
    public VariationOptionUpdateValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than zero.");

        RuleFor(x => x.Value)
            .NotEmpty().WithMessage("Value must not be empty.")
            .Length(1, 100).WithMessage("Value length must be between 1 and 100 characters.");
    }
}