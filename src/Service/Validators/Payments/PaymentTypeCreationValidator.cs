using FluentValidation;
using Service.DTOs.Payments.PaymentType;

namespace Service.Validators.Payments;

public class PaymentTypeCreationValidator : AbstractValidator<PaymentTypeCreationDto>
{
    public PaymentTypeCreationValidator()
    {
        RuleFor(pt => pt.Value)
            .NotEmpty()
                .WithMessage("Value should NOT be empty.")
            .MinimumLength(4)
                .WithMessage("Value length should NOT be less than 4.")
            .MaximumLength(100)
                .WithMessage("Value length should NOT be more than 100.");
    }
}