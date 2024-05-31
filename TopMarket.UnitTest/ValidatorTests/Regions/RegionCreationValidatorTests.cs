using Service.DTOs.Regions;

namespace Service.Validators.Regions;

public class RegionCreationValidatorTests
{
    private readonly RegionCreationValidator validator;

    public RegionCreationValidatorTests()
    {
        this.validator = new RegionCreationValidator();
    }

    [Theory]
    [InlineData(0)]
    public void CheckingForCountryId_ShouldReturnFalse(long countryId)
    {
        var region = new RegionCreationDto
        {
            CountryId = countryId
        };

        Assert.False(this.validator.Validate(region).IsValid);
    }

    [Theory]
    [InlineData(1)]
    public void CheckingForCountryId_ShouldReturnTrue(long countryId)
    {
        var region = new RegionCreationDto
        {
            CountryId = countryId
        };

        Assert.True(this.validator.Validate(region).IsValid);
    }
}