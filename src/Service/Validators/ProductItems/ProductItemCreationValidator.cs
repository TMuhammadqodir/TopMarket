using FluentValidation;
using Service.DTOs.ProductItems;

namespace Service.Validators.ProductItems;

public class ProductItemCreationValidator : AbstractValidator<ProductItemCreationDto>
{
    public ProductItemCreationValidator()
    {
        RuleFor(item => item.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to zero.");

        RuleFor(item => item.ProductId)
            .GreaterThan(0).WithMessage("ProductId must be greater than zero.");
    }
}
