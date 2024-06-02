using FluentValidation;
using Service.DTOs.Addresses;

namespace Service.Validators.Addresses
{
    public class AddressCreationValidator : AbstractValidator<AddressCreationDto>
    {
        public AddressCreationValidator()
        {
            RuleFor(x => x.Street)
                .NotEmpty()
                    .WithMessage("Street must not be empty.")
                .Length(1, 255)
                    .WithMessage("Street length must be between 1 and 255 characters.");

            RuleFor(x => x.Floor)
                .NotEmpty()
                    .WithMessage("Street must not be empty.")
                .MaximumLength(50)
                    .WithMessage("Floor length must be less than or equal to 50 characters.");

            RuleFor(x => x.Home)
                .NotEmpty()
                    .WithMessage("Street must not be empty.")
                .MaximumLength(50)
                    .WithMessage("Home length must be less than or equal to 50 characters.");

            RuleFor(x => x.DoorCode)
                .NotEmpty()
                    .WithMessage("Street must not be empty.")
                .MaximumLength(10)
                    .WithMessage("DoorCode length must be less than or equal to 10 characters.");

            RuleFor(x => x.CountryId)
                .GreaterThan(0)
                    .WithMessage("CountryId must be greater than zero.");

            RuleFor(x => x.RegionId)
                .GreaterThan(0)
                    .WithMessage("RegionId must be greater than zero.");

            RuleFor(x => x.DistrictId)
                .GreaterThan(0)
                    .WithMessage("DistrictId must be greater than zero.");
        }
    }
}
