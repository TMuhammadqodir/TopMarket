using FluentValidation;
using Service.DTOs.Promotions;

namespace Service.Validators.Promations;

public class PromotionCreationValidator : AbstractValidator<PromotionCreationDto>
{
    public PromotionCreationValidator()
    {
        RuleFor(item => item.Name)
            .NotEmpty()
                .WithMessage("Name must not be empty.")
            .Length(1, 100)
                .WithMessage("Name length must be between 1 and 100 characters.");

        RuleFor(item => item.Description)
            .NotEmpty()
                .WithMessage("Description must not be empty.")
            .Length(1, 500)
                .WithMessage("Description length must be between 1 and 500 characters.");

        RuleFor(item => item.DiscountRate)
            .InclusiveBetween(0, 100)
                .WithMessage("Discount rate must be between 0 and 100.");

        RuleFor(item => item.StartDate)
            .LessThan(item => item.EndDate)
                .WithMessage("Start date must be before end date.");

        RuleFor(item => item.EndDate)
            .GreaterThan(item => item.StartDate)
                .WithMessage("End date must be after start date.");
    }
}
