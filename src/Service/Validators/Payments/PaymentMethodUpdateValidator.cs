using FluentValidation;
using Service.DTOs.Payments.PaymentMethods;

namespace Service.Validators.Payments;

public class PaymentMethodUpdateValidator : AbstractValidator<PaymentMethodUpdateDto>
{
    public PaymentMethodUpdateValidator()
    {
        RuleFor(pm => pm.Id)
          .NotEmpty()
              .WithMessage("Id should NOT be empty.");

        RuleFor(pm => pm.Provider)
            .NotEmpty()
                .WithMessage("Provider should NOT be empty.");

        RuleFor(pm => pm.ExpiryDate)
            .NotEmpty()
                .WithMessage("Expiration  date should NOT be empty.");

        RuleFor(pm => pm.AccountNumber)
            .NotEmpty()
                .WithMessage("Account number should NOT be empty.");
    }
}
