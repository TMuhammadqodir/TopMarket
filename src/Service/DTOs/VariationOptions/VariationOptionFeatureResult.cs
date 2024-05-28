using FluentValidation;
using Service.DTOs.Variations;

namespace Service.DTOs.VariationOptions;

public class VariationOptionUpdateValidator : AbstractValidator<VariationOptionUpdateDto>
{
    public VariationOptionUpdateValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
                .WithMessage("Id must be greater than zero.");

        RuleFor(x => x.Value)
            .NotEmpty()
                .WithMessage("Value must not be empty.")
            .Length(1, 128)
                .WithMessage("Value length must be between 1 and 128 characters.");
    }
}