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
            .Length(1, 128)
                .WithMessage("Name length must be between 1 and 128 characters.");

        RuleFor(item => item.Description)
            .NotEmpty()
                .WithMessage("Description must not be empty.")
            .Length(1, 512)
                .WithMessage("Description length must be between 1 and 512 characters.");

        RuleFor(item => item.DiscountRate)
            .InclusiveBetween(0, 128)
                .WithMessage("Discount rate must be between 0 and 128.");

        RuleFor(item => item.StartDate)
            .LessThan(item => item.EndDate)
                .WithMessage("Start date must be before end date.");

        RuleFor(item => item.EndDate)
            .GreaterThan(item => item.StartDate)
                .WithMessage("End date must be after start date.");
    }
}
