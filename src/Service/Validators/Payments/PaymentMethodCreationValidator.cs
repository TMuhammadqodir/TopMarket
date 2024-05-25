using FluentValidation;
using Service.DTOs.Payments.PaymentMethods;

namespace Service.Validators.Payments;

public class PaymentMethodCreationValidator : AbstractValidator<PaymentMethodCreationDto>
{
    public PaymentMethodCreationValidator()
    {
        RuleFor(pm => pm.Provider)
            .NotEmpty()
                .WithMessage("Provider should NOT be empty.");

        RuleFor(pm => pm.UserId)
            .NotEmpty()
                .WithMessage("User id should NOT be empty.");

        RuleFor(pm => pm.PaymentTypeId)
            .NotEmpty()
                .WithMessage("Payment type id should NOT be empty.");

        RuleFor(pm => pm.ExpiryDate)
            .NotEmpty()
                .WithMessage("Expiration  date should NOT be empty.");

        RuleFor(pm => pm.AccountNumber)
            .NotEmpty()
                .WithMessage("Account number should NOT be empty.");
    }
}
