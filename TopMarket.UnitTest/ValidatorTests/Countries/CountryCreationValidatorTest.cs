using Service.DTOs.Countries;
using Service.Validators.Countries;

namespace TopMarket.UnitTest.ValidatorTests.Countries;

public class CountryCreationValidatorTest
{
    private CountryCreationValidator countryCreationValidator;
    public CountryCreationValidatorTest() 
    {
        this.countryCreationValidator = new CountryCreationValidator();
    }

    [Theory]
    [InlineData("uzbekistan", "")]
    [InlineData("", "uz")]
    [InlineData("", "")]
    [InlineData("u", "u")]
    [InlineData("uz", null)]
    [InlineData("u", "uz")]
    [InlineData(null, null)]

    public void ShouldReturnFalseForName(string name, string countryCode)
    {
        var country = new CountryCreationDto(){
            Name = name,
            CountryCode = countryCode
        };

        var result = countryCreationValidator.Validate(country);

        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData('u')]
    public void ShouldReturnFalseForNameLenth(char character)
    {
        var country = new CountryCreationDto()
        {
            Name = new string('u', 130),
            CountryCode = new string('u', 33),
        };

        var result = countryCreationValidator.Validate(country);

        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData("uzbekistan", "uz")]
    [InlineData("uzbek", "uz")]
    [InlineData("uz", "uz")]
    [InlineData("uz", "uzzzzzzzzzzzzzzzzzzzzzzz")]
    [InlineData("ugggg", "uz")]

    public void ShouldReturnTrueName(string name, string countryCode)
    {
        var country = new CountryCreationDto()
        {
            Name = name,
            CountryCode = countryCode
        };

        var result = countryCreationValidator.Validate(country);

        Assert.True(result.IsValid);
    }
}
