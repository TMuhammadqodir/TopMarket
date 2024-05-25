using FluentValidation;
using Service.DTOs.Districts;

namespace Service.Validators.Districts;

public class DistrictCreationValidator : AbstractValidator<DistrictCreationDto>
{
    public DistrictCreationValidator()
    {
        RuleFor(item => item.NameUz)
            .NotEmpty()
                .WithMessage("Country cannot be empty")
            .MaximumLength(128)
                .WithMessage("This lastname very long")
            .MinimumLength(2)
                .WithMessage("This lastname very short");

        RuleFor(item => item.NameOz)
            .NotEmpty()
                .WithMessage("Country code cannot be empty")
            .MaximumLength(128)
                .WithMessage("This lastname very long")
            .MinimumLength(2)
                .WithMessage("This lastname very short");

        RuleFor(item => item.NameRu)
            .NotEmpty()
                .WithMessage("Country code cannot be empty")
            .MaximumLength(128)
                .WithMessage("This lastname very long")
            .MinimumLength(2)
                .WithMessage("This lastname very short");

        RuleFor(item => item.RegionId)
            .GreaterThan(0)
                .WithMessage("id cannot be negative or 0");
    }
}