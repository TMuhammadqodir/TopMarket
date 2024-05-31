using FluentValidation;
using Service.DTOs.Variations;

namespace Service.Validators.Variations;

public class VariationUpdateValidator : AbstractValidator<VariationUpdateDto>
{
    public VariationUpdateValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty();

        RuleFor(v => v.Name)
            .NotEmpty()
            .Length(4, 100);

        RuleFor(v => v.CategoryId)
            .NotEmpty();
    }
}
