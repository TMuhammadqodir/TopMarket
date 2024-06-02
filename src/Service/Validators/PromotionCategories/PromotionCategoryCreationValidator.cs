using FluentValidation;
using Service.DTOs.PromotionCategories;

namespace Service.Validators.PromotionCategories;

public class PromotionCategoryCreationValidator : AbstractValidator<PromotionCategoryCreationDto>
{
    public PromotionCategoryCreationValidator()
    {
        RuleFor(pc => pc.CategoryId)
            .NotEmpty();

        RuleFor(pc => pc.PromotionId)
            .NotEmpty();
    }
}
