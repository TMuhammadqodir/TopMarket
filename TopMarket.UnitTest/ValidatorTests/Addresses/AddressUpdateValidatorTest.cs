using FluentValidation.TestHelper;
using Service.DTOs.Addresses;
using Service.Validators.Addresses;

namespace TopMarket.UnitTest.ValidatorTests.Addresses;

public class AddressUpdateValidatorTest
{
    private readonly AddressUpdateValidator addressUpdateValidator;

    public AddressUpdateValidatorTest()
    {
        this.addressUpdateValidator = new AddressUpdateValidator();
    }

    [Theory]
    [InlineData(1, "ds", "dsds", "dsd", "dsd", 1, 1, 0)]
    [InlineData(1, "ds", "dsds", "dsd", "dsd", 0, 1, 2)]
    [InlineData(1, "dsd", "dsd", "dsd", "dsd", 1, 0, 1)]
    [InlineData(0, "dsd", "dsd", "dsd", "dsd", 1, 1, 1)]
    [InlineData(1, "ds", "ds", "ds", "", 1, 1, 1)]
    [InlineData(1, "ds", "ds", "", "ds", 1, 1, 1)]
    [InlineData(1, "", "ds", "ds", "ds", 1, 1, 1)]
    [InlineData(1, "ds", "", "ds", "ds", 1, 1, 1)]
    [InlineData(1, "", "", "", "", 0, 1, 0)]
    [InlineData(1, "", "", "", "", 0, 0, 0)]
    public void ShouldBeEqualToFalse(long id, string street, string floor, string home, string doorCode, long countryId, long regionId, long districtId)
    {
        var address = new AddressUpdateDto
        {
            Id = id,
            Street = street,
            Floor = floor,
            Home = home,
            DoorCode = doorCode,
            CountryId = countryId,
            RegionId = regionId,
            DistrictId = districtId
        };

        var result = addressUpdateValidator.Validate(address);

        Assert.False(result.IsValid);
    }


    [Fact]
    public void ShouldBeEqualToTrue()
    {
        var address = new AddressUpdateDto
        {
            Id = 1,
            Street = "dsds",
            Floor = "dsds",
            Home = "dsds",
            DoorCode = "dsds",
            CountryId = 1,
            RegionId = 2,
            DistrictId = 3
        };

        var result = addressUpdateValidator.TestValidate(address);

        result.ShouldNotHaveAnyValidationErrors();
    }

}