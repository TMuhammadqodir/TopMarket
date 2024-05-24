using FluentValidation;
using Service.DTOs.Carts;

namespace Service.Validators.Carts;

public class CartItemUpdateValidator : AbstractValidator<CartItemUpdateDto>
{
    public CartItemUpdateValidator()
    {
        RuleFor(dto => dto.Quantity)
            .NotEqual(0)
            .WithMessage("Quantity should NOT be equal to 0.");

        RuleFor(dto => dto.Price)
            .NotEqual(0)
            .WithMessage("Price should NOT be equal to 0");

        RuleFor(dto => dto.ProductItemId)
            .NotEqual(0)
            .WithMessage("Product Item Id is NOT valid");

        RuleFor(dto => dto.CartId)
            .NotEqual(0)
            .WithMessage("Cart Id is NOT valid");
    }
}