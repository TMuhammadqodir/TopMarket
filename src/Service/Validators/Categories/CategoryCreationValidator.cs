using FluentValidation;
using Service.DTOs.Categories;

namespace Service.Validators.Categories;

public class CategoryCreationValidator : AbstractValidator<CategoryCreationDto>
{
    public CategoryCreationValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .Length(4, 100);
    }
}
