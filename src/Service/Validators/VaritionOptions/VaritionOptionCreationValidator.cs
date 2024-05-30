using FluentValidation;
using Service.DTOs.VariationOptions;

namespace Service.Validators.VaritionOptions;

public class VariationOptionCreationValidator : AbstractValidator<VariationOptionCreationDto>
{
    public VariationOptionCreationValidator()
    {
        RuleFor(x => x.Value)
            .NotEmpty()
                .WithMessage("Value must not be empty.")
            .Length(1, 128)
                .WithMessage("Value length must be between 1 and 128 characters.");

        RuleFor(x => x.VariationId)
            .GreaterThan(0)
                .WithMessage("VariationId must be greater than zero.");

        RuleFor(x => x.ProductItemId)
            .GreaterThan(0)
                .WithMessage("ProductItemId must be greater than zero.");
    }
}