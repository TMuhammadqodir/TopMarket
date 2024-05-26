using FluentValidation;
using Service.DTOs.Products;

namespace Service.Validators.Products;

public class ProductCreationValidator : AbstractValidator<ProductCreationDto>
{
    public ProductCreationValidator()
    {
        RuleFor(item => item.Name)
            .NotEmpty()
                .WithMessage("Name must not be empty.")
            .Length(1, 128)
                .WithMessage("Name length must be between 1 and 100 characters.");

        RuleFor(item => item.Description)
            .NotEmpty()
                .WithMessage("Description must not be empty.")
            .Length(1, 512)
                .WithMessage("Description length must be between 1 and 500 characters.");

        RuleFor(item => item.CategoryId)
            .GreaterThan(0)
                .WithMessage("CategoryId must be greater than zero.");
    }
}
