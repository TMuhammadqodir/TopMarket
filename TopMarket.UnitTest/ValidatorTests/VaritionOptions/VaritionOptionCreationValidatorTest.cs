using FluentValidation.TestHelper;
using Service.DTOs.VariationOptions;
using Service.Validators.VaritionOptions;

public class VariationOptionCreationValidatorTest
{
    private readonly VariationOptionCreationValidator validator;

    public VariationOptionCreationValidatorTest()
    {
        validator = new VariationOptionCreationValidator();
    }

    [Fact]
    public void ShouldHaveErrorWhenValueIsEmpty()
    {
        var model = new VariationOptionCreationDto { Value = string.Empty };
        var result = validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Value);
    }

    [Fact]
    public void ShouldHaveErrorWhenValueIsTooLong()
    {
        var model = new VariationOptionCreationDto { Value = new string('a', 129) };
        var result = validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Value);
    }

    [Fact]
    public void ShouldHaveErrorWhenVariationIdIsZero()
    {
        var model = new VariationOptionCreationDto { VariationId = 0 };
        var result = validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.VariationId);
    }

    [Fact]
    public void ShouldHaveErrorWhenProductItemIdIsZero()
    {
        var model = new VariationOptionCreationDto { ProductItemId = 0 };
        var result = validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.ProductItemId);
    }

    [Fact]
    public void ShouldNotHaveErrorWhenModelIsValid()
    {
        var model = new VariationOptionCreationDto
        {
            Value = "Valid Value",
            VariationId = 1,
            ProductItemId = 1
        };
        var result = validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
