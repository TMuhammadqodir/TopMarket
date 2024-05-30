using FluentValidation;
using Service.DTOs.Orders;

namespace Service.Validators.Orders
{
    public class OrderCreationValidator : AbstractValidator<OrderCreationDto>
    {
        public OrderCreationValidator()
        {
            RuleFor(x => x.Date)
                .NotEmpty()
                    .WithMessage("Date must not be empty.")
                .LessThanOrEqualTo(DateTime.Now)
                    .WithMessage("Date cannot be in the future.");

            RuleFor(x => x.Total)
                .GreaterThanOrEqualTo(0)
                    .WithMessage("Total must be greater than or equal to zero.");

            RuleFor(x => x.UserId)
                .GreaterThan(0)
                    .WithMessage("UserId must be greater than zero.");

            RuleFor(x => x.PaymentMethodId)
                .GreaterThan(0)
                    .WithMessage("PaymentMethodId must be greater than zero.");

            RuleFor(x => x.AddressId)
                .GreaterThan(0)
                    .WithMessage("AddressId must be greater than zero.");

            RuleFor(x => x.ShippingMethodId)
                .GreaterThan(0)
                    .WithMessage("ShippingMethodId must be greater than zero.");

            RuleFor(x => x.StatusId)
                .GreaterThan(0)
                    .WithMessage("StatusId must be greater than zero.");
        }
    }
}
