using Service.DTOs.Countries;
using Service.DTOs.Districts;
using Service.Validators.Countries;
using Service.Validators.Districts;

namespace TopMarket.UnitTest.ValidatorTests.Districts;

public class DistrictCreationValidatorTest
{
    private DistrictCreationValidator districtCreationValidator;
    public DistrictCreationValidatorTest()
    {
        this.districtCreationValidator = new DistrictCreationValidator();
    }

    [Theory]
    [InlineData("uzbekistan", "", "xsscsd", 1L)]
    [InlineData("", "uz", "dewfwef", 2L)]
    [InlineData("", "", "", 0)]
    [InlineData("fd", "fd", "vf", 0)]
    [InlineData("u", "u", "r", 1L)]
    [InlineData("uz", null, null, null)]
    [InlineData("u", "uz", "ds", 2)]
    [InlineData(null, null, "", null)]

    public void ShouldReturnFalseForName(string nameUz, string nameOz, string nameRu, long id)
    {
        var district = new DistrictCreationDto()
        {
            NameUz = nameUz,
            NameOz = nameOz,
            NameRu = nameRu,
            RegionId = id
        };

        var result = districtCreationValidator.Validate(district);

        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData('u')]
    public void ShouldReturnFalseForNameLenth(char character)
    {
        var district = new DistrictCreationDto()
        {
            NameUz = new string('u', 134),
            NameOz = new string('u', 123),
            NameRu = new string('u', 1306),
            RegionId = 6
        };

        var result = districtCreationValidator.Validate(district);

        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData("uzbekistan", "ded", "xsscsd", 1L)]
    [InlineData("ver", "uz", "dewfwef", 2L)]
    [InlineData("rr", "rr", "rr", 1L)]
    [InlineData("ugd", "udddddddddd", "rffffff", 1L)]

    public void ShouldReturnTrueName(string nameUz, string nameOz, string nameRu, long id)
    {
        var district = new DistrictCreationDto()
        {
            NameUz = nameUz,
            NameOz = nameOz,
            NameRu = nameRu,
            RegionId = id
        };

        var result = districtCreationValidator.Validate(district);

        Assert.True(result.IsValid);
    }
}
