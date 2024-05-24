using FluentValidation;
using Service.DTOs.Categories;

namespace Service.Validators.Categories;

public class CategoryCreationValidator : AbstractValidator<CategoryCreationDto>
{
    public CategoryCreationValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
                .WithMessage("Name should NOT be null or empty.")
            .MinimumLength(4)
                .WithMessage("Minimum 'Name' length is 4.")
            .MaximumLength(100)
                .WithMessage("Maximum 'Name' length is 100.");
    }
}
