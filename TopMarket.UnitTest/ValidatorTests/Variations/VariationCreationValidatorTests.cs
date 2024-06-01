using Service.DTOs.Variations;

namespace Service.Validators.Variations;

public class VariationCrationValidatorTests
{
    private readonly VariationCreationValidator validator;

    public VariationCrationValidatorTests()
    {
        this.validator = new VariationCreationValidator();
    }

    [Theory]
    [InlineData(default, default)]
    [InlineData("Variation", 0L)]
    [InlineData("abc", 1L)] // checking for min length
    public void CheckingForVariationCreationDto_ShouldNotBeValid(string name, long categoryId)
    {
        var variation = new VariationCreationDto
        {
            Name = name,
            CategoryId = categoryId
        };

        Assert.False(this.validator.Validate(variation).IsValid);
    }

    [Theory]
    [InlineData('a', 1L)]
    public void CheckingForVariationCreationDtoNameMaxLength_ShouldNotBeValid(char c, long categoryId)
    {
        var variation = new VariationCreationDto
        {
            Name = new string(c, 101),
            CategoryId = categoryId
        };

        Assert.False(this.validator.Validate(variation).IsValid);
    }

    [Theory]
    [InlineData("abcd", 1L)]
    public void CheckingForVariationCreationDto_ShouldBeValid(string name, long categoryId)
    {
        var variation = new VariationCreationDto
        {
            Name = name,
            CategoryId = categoryId
        };
        
        Assert.True(this.validator.Validate(variation).IsValid);
    }
}
