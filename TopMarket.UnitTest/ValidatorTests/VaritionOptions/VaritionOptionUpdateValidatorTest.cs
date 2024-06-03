using FluentValidation.TestHelper;
using Service.DTOs.VariationOptions;
using Service.Validators.VaritionOptions;
using Xunit;
namespace TopMarket.UnitTest.ValidatorTests.VaritionOption;

public class VariationOptionUpdateValidatorTests
{
    private readonly VariationOptionUpdateValidator variationOptionUpdateValidator;

    public VariationOptionUpdateValidatorTests()
    {
        this.variationOptionUpdateValidator = new VariationOptionUpdateValidator();
    }

    [Theory]
    [InlineData(1, "")]
    [InlineData(0, "ds")]
    [InlineData(null, "ds")]
    [InlineData(null, null)]
    public void ShouldBeEqualToFalse(long id, string value)
    {
        var variationOption = new VariationOptionUpdateDto
        {
            Id = id,
            Value = value
        };

        var result = variationOptionUpdateValidator.Validate(variationOption);

        Assert.False(result.IsValid);
    }


    [Fact]
    public void ShouldBeEqualToTrue()
    {
        var variationOption = new VariationOptionUpdateDto
        {
            Id = 1,
            Value = "Valid Value"
        };

        var result = variationOptionUpdateValidator.TestValidate(variationOption);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
