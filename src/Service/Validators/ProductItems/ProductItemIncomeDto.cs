using FluentValidation;
using Service.DTOs.ProductItems;

namespace Service.Validators.ProductItems;

public class ProductItemIncomeValidator : AbstractValidator<ProductItemIncomeDto>
{
    public ProductItemIncomeValidator()
    {
        RuleFor(item => item.Id)
                .GreaterThan(0)
                    .WithMessage("Id must be greater than zero.");

        RuleFor(item => item.QuantityInStock)
            .GreaterThanOrEqualTo(0)
                .WithMessage("Quantity in stock must be greater than or equal to zero.");
    }
}
