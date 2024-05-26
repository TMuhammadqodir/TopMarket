using FluentValidation;
using Service.DTOs.ProductItems;

namespace Service.Validators.ProductItems;

public class ProductItemUpdateValidator : AbstractValidator<ProductItemUpdateDto>
{
    public ProductItemUpdateValidator()
    {
        RuleFor(item => item.Id)
                .GreaterThan(0)
                    .WithMessage("Id must be greater than zero.");

        RuleFor(item => item.SKU)
            .NotEmpty()
                .WithMessage("SKU must not be empty.")
            .Length(1, 64)
                .WithMessage("SKU length must be between 1 and 50 characters.");

        RuleFor(item => item.Price)
            .GreaterThanOrEqualTo(0)
                .WithMessage("Price must be greater than or equal to zero.");

        RuleFor(item => item.QuantityInStock)
            .GreaterThanOrEqualTo(0)
                .WithMessage("Quantity in stock must be greater than or equal to zero.");

        RuleFor(item => item.ProductId)
            .GreaterThan(0)
                .WithMessage("ProductId must be greater than zero.");
    }
}