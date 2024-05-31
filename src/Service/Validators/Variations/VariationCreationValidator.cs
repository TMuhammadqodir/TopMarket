using FluentValidation;
using Service.DTOs.Variations;

namespace Service.Validators.Variations;

public class VariationCreationValidator : AbstractValidator<VariationCreationDto>
{
    public VariationCreationValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty()
            .Length(4, 100);

        RuleFor(v => v.CategoryId)
            .NotEmpty();
    }
}
