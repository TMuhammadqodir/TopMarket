using FluentValidation.TestHelper;
using Service.DTOs.VariationOptions;
using Service.Validators.VaritionOptions;

namespace TopMarket.UnitTest.ValidatorTests.VaritionOption;

public class VariationOptionCreationValidatorTest
{
    private readonly VariationOptionCreationValidator variationOptionCreationValidator;

    public VariationOptionCreationValidatorTest()
    {
        this.variationOptionCreationValidator = new VariationOptionCreationValidator();
    }

    [Theory]
    [InlineData("", 1, 1)]
    [InlineData("ds", 0, 1)]
    [InlineData("ds", 1, 0)]
    [InlineData(null, null, null)]
    public void ShouldBeEqualToFalse(string value, long varitionId, long productItemId)
    {
        var variationOption = new VariationOptionCreationDto
        {
            Value = value,
            VariationId = varitionId,
            ProductItemId = productItemId
        };

        var result = variationOptionCreationValidator.Validate(variationOption);

        Assert.False(result.IsValid);
    }


    [Fact]
    public void ShouldBeEqualToTrue()
    {
        var variationOption = new VariationOptionCreationDto
        {
            Value = "Valid Value",
            VariationId = 1,
            ProductItemId = 1
        };

        var result = variationOptionCreationValidator.TestValidate(variationOption);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
