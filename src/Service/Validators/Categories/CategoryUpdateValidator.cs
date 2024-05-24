using FluentValidation;
using Service.DTOs.Categories;

namespace Service.Validators.Categories;

public class CategoryUpdateValidator : AbstractValidator<CategoryUpdateDto>
{
    public CategoryUpdateValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
                .WithMessage("Id should NOT be equal to null or 0.");

        RuleFor(c => c.Name)
            .NotEmpty()
                .WithMessage("Name should NOT be null or empty.")
            .MinimumLength(4)
                .WithMessage("Minimum 'Name' length is 4.")
            .MaximumLength(100)
                .WithMessage("Maximum 'Name' length is 100.");

        RuleFor(c => c.ParentId)
            .NotEqual(c => c.Id)
                .WithMessage("Parent Id can NOT be equal to Id.");
    }
}
