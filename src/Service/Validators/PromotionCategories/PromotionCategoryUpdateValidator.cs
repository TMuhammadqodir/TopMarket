using FluentValidation;
using Service.DTOs.PromotionCategories;

namespace Service.Validators.PromotionCategories;

public class PromotionCategoryUpdateValidator : AbstractValidator<PromotionCategoryUpdateDto>
{
    public PromotionCategoryUpdateValidator()
    {
        RuleFor(pc => pc.Id)
            .NotEmpty();

        RuleFor(pc => pc.CategoryId)
            .NotEmpty();

        RuleFor(pc => pc.PromotionId)
            .NotEmpty();
    }
}
