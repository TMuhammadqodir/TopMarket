using Service.DTOs.Variations;

namespace Service.Validators.Variations;

public class VariationUpdateValidatorTests
{
    private readonly VariationUpdateValidator validator;

    public VariationUpdateValidatorTests()
    {
        this.validator = new VariationUpdateValidator();
    }

    [Theory]
    [InlineData(default, default, default)]
    [InlineData(1L, default, default)]
    [InlineData(1L, "Variation", 0L)]
    [InlineData(1L, "abc", 1L)] // checking for min length
    public void CheckingForVariationCreationDto_ShouldNotBeValid(long id, string name, long categoryId)
    {
        var variation = new VariationUpdateDto
        {
            Id = id,
            Name = name,
            CategoryId = categoryId
        };

        Assert.False(this.validator.Validate(variation).IsValid);
    }

    [Theory]
    [InlineData(1L, 'a', 2L)]
    public void CheckingForVariationCreationDtoNameMaxLength_ShouldNotBeValid(long id, char c, long categoryId)
    {
        var variation = new VariationUpdateDto
        {
            Id = id,
            Name = new string(c, 101),
            CategoryId = categoryId
        };

        Assert.False(this.validator.Validate(variation).IsValid);
    }

    [Theory]
    [InlineData(1L, "abcd", 1L)]
    public void CheckingForVariationCreationDto_ShouldBeValid(long id, string name, long categoryId)
    {
        var variation = new VariationUpdateDto
        {
            Id = id,
            Name = name,
            CategoryId = categoryId
        };

        Assert.True(this.validator.Validate(variation).IsValid);
    }
}