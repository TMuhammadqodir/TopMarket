using FluentValidation;
using Service.DTOs.Payments.PaymentType;

namespace Service.Validators.Payments;

public class PaymentTypeCreationValidator : AbstractValidator<PaymentTypeCreationDto>
{
    public PaymentTypeCreationValidator()
    {
        RuleFor(pt => pt.Value)
            .NotEmpty()
                .WithMessage("Value should NOT be empty.");
    }
}