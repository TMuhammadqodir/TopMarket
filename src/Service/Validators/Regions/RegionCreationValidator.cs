using FluentValidation;
using Service.DTOs.Regions;

namespace Service.Validators.Regions;

public class RegionCreationValidator : AbstractValidator<RegionCreationDto>
{
    public RegionCreationValidator()
    {
        RuleFor(r => r.CountryId)
            .NotEmpty();
    }
}
