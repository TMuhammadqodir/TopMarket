using Service.DTOs.Countries;
using Service.Validators.Countries;

namespace TopMarket.UnitTest.ValidatorTests.Countries;

public class CountryUpdateValidatorTest
{
    private CountryUpdateValidator countryUpdateValidator;
    public CountryUpdateValidatorTest()
    {
        this.countryUpdateValidator = new CountryUpdateValidator();
    }

    [Theory]
    [InlineData(1L, "uzbekistan", "")]
    [InlineData(null, "uzbekistan", "uz")]
    [InlineData(-2L, "uzbekistan", "uz")]
    [InlineData(2L, "", "uz")]
    [InlineData(3L, "", "")]
    [InlineData(4L, "u", "u")]
    [InlineData(5L, "uz", null)]
    [InlineData(6L, "u", "uz")]
    [InlineData(700000000000000000L, null, null)]

    public void ShouldReturnFalseForName(long id, string name, string countryCode)
    {
        var country = new CountryUpdateDto()
        {
            Id = id,
            Name = name,
            CountryCode = countryCode
        };

        var result = countryUpdateValidator.Validate(country);

        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData('u')]
    public void ShouldReturnFalseForNameLenth(char character)
    {
        var country = new CountryUpdateDto()
        {
            Id = 2L,
            Name = new string('u', 130),
            CountryCode = new string('u', 33),
        };

        var result = countryUpdateValidator.Validate(country);

        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData(1L, "uzbekistan", "uz")]
    [InlineData(2L, "uzbek", "uz")]
    [InlineData(3L, "uz", "uz")]
    [InlineData(3L, "uz", "uzzzzzzzzzzzzzzzzzzzzzzz")]
    [InlineData(4L, "ugggg", "uz")]

    public void ShouldReturnTrueName(long id, string name, string countryCode)
    {
        var country = new CountryUpdateDto()
        {
            Id = id,
            Name = name,
            CountryCode = countryCode
        };

        var result = countryUpdateValidator.Validate(country);

        Assert.True(result.IsValid);
    }
}
