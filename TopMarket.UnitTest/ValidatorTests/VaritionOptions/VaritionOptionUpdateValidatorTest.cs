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

    [Fact]
    public void ShouldHaveErrorWhenIdIsZero()
    {
        var model = new VariationOptionUpdateDto { Id = 0 };
        var result = this.variationOptionUpdateValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void ShouldHaveErrorWhenValueIsEmpty()
    {
        var model = new VariationOptionUpdateDto { Value = string.Empty };
        var result = this.variationOptionUpdateValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Value);
    }

    [Fact]
    public void ShouldHaveErrorWhenValueIsTooLong()
    {
        var model = new VariationOptionUpdateDto { Value = new string('a', 129) };
        var result = this.variationOptionUpdateValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Value);
    }

    [Fact]
    public void ShouldNotHaveErrorWhenModelIsValid()
    {
        var model = new VariationOptionUpdateDto
        {
            Id = 1,
            Value = "Valid Value"
        };
        var result = this.variationOptionUpdateValidator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
