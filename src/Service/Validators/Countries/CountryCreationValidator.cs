using FluentValidation;
using Service.DTOs.Countries;

namespace Service.Validators.Countries;

public class CountryCreationValidator : AbstractValidator<CountryCreationDto>
{
    public CountryCreationValidator() 
    {
        RuleFor(item => item.Name)
            .NotEmpty()
            .WithMessage("Country cannot be empty")
            .MaximumLength(128)
            .WithMessage("This lastname very long")
            .MinimumLength(2)
            .WithMessage("This lastname very short");

        RuleFor(item => item.CountryCode)
            .NotEmpty()
            .WithMessage("Country code cannot be empty")
            .MaximumLength(32)
            .WithMessage("This lastname very long")
            .MinimumLength(2)
            .WithMessage("This lastname very short");
    }
}
