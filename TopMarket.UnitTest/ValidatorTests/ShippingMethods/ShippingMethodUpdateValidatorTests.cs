using Service.DTOs.ShippingMethods;

namespace Service.Validators.ShippingMethods;

public class ShippingMethodUpdateValidatorTests
{
    private readonly ShippingMethodUpdateValidator validator;

    public ShippingMethodUpdateValidatorTests()
    {
        this.validator = new ShippingMethodUpdateValidator();
    }

    [Theory]
    [InlineData(default, default, default)]
    [InlineData(1L, "", 0)]
    [InlineData(1L, "ShippingMethod1", 0)]
    [InlineData(1L, "abc", 14.4)] // checking for min length
    public void CheckingForShippingMethodCreationDto_ShouldNotBeValid(long id, string name, decimal price)
    {
        var shippingMethod = new ShippingMethodUpdateDto
        {
            Id = id,
            Name = name,
            Price = price
        };

        Assert.False(this.validator.Validate(shippingMethod).IsValid);
    }

    [Theory]
    [InlineData(1L, 'c', 14.4)]
    public void CheckingForDtoNameMaxLength_ShouldNotBeValid(long id, char c, decimal price)
    {
        var shippingMethod = new ShippingMethodUpdateDto
        {
            Id = id,
            Name = new string(c, 101),
            Price = price
        };

        Assert.False(this.validator.Validate(shippingMethod).IsValid);
    }

    [Theory]
    [InlineData(1L, "abcd", 1.4)]
    public void CheckingForShippingMethodCreationDto_ShouldBeValid(long id, string name, decimal price)
    {
        var shippingMethod = new ShippingMethodUpdateDto
        {
            Id = id,
            Name = name,
            Price = price
        };

        Assert.True(this.validator.Validate(shippingMethod).IsValid);
    }
}