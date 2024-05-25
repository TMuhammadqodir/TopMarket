using FluentValidation;
using Service.DTOs.Payments.PaymentType;

namespace Service.Validators.Payments;

public class PaymentTypeUpdateValidator : AbstractValidator<PaymentTypeUpdateDto>
{
    public PaymentTypeUpdateValidator()
    {
        RuleFor(pt => pt.Id)
            .NotEmpty()
                .WithMessage("Id should NOT be empty.");

        RuleFor(pt => pt.Value)
            .NotEmpty()
                .WithMessage("Value should NOT be empty.");
    }
}
