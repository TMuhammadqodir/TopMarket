using FluentValidation;
using Service.DTOs.UserRewiev;

namespace Service.Validators.UserRewievs;

public class UserRewievCreationValidator : AbstractValidator<UserRewievCreationDto>
{
    public UserRewievCreationValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("UserId must be greater than zero.");

        RuleFor(x => x.OrderProductId)
            .GreaterThan(0).WithMessage("OrderProductId must be greater than zero.");

        RuleFor(x => x.RatingValue)
            .IsInEnum().WithMessage("RatingValue must be a valid rating.");

        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("Comment must not be empty.")
            .MaximumLength(500).WithMessage("Comment must be less than or equal to 500 characters.");
    }
}

