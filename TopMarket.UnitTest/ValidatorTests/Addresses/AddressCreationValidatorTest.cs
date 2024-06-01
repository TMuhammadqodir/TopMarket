using FluentValidation.TestHelper;
using Service.DTOs.Addresses;
using Service.Validators.Addresses;

namespace TopMarket.UnitTest.ValidatorTests.Addresses;

public class AddressCreationValidatorTest
{
    private readonly AddressCreationValidator addressCreationValidator;

    public AddressCreationValidatorTest()
    {
        this.addressCreationValidator = new AddressCreationValidator();
    }

    [Theory]
    [InlineData("ds", "dsds", "dsd", "dsd", 1, 1, 0)]
    [InlineData("ds", "dsds", "dsd", "dsd", 0, 1, 2)]
    [InlineData("dsd", "dsd", "dsd", "dsd", 1, 0, 1)]
    [InlineData("ds", "ds", "ds", "", 1, 1, 1)]
    [InlineData("ds", "ds", "", "ds", 1, 1, 1)]
    [InlineData("", "ds", "ds", "ds", 1, 1, 1)]
    [InlineData("ds", "", "ds", "ds", 1, 1, 1)]
    [InlineData("", "", "", "", 0, 1, 0)]
    [InlineData("", "", "", "", 0, 0, 0)]
    public void ShouldBeEqualToFalse(string street, string floor, string home, string doorCode, long countryId, long regionId, long districtId)
    {
        var address = new AddressCreationDto
        {
            Street = street,
            Floor = floor,
            Home = home,
            DoorCode = doorCode,
            CountryId = countryId,
            RegionId = regionId,
            DistrictId = districtId
        };

        var result = addressCreationValidator.Validate(address);

        Assert.False(result.IsValid);
    }


    [Fact]
    public void ShouldBeEqualToTrue()
    {
        var address = new AddressCreationDto
        {
            Street = "dsds",
            Floor = "dsds",
            Home = "dsds",
            DoorCode = "dsds",
            CountryId = 1,
            RegionId = 2,
            DistrictId = 3
        };

        var result = addressCreationValidator.TestValidate(address);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
