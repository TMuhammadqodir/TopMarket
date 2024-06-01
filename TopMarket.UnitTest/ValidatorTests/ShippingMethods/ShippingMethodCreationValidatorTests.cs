using Service.DTOs.ShippingMethods;

namespace Service.Validators.ShippingMethods;

public class ShippingMethodCreationValidatorTests
{
    private readonly ShippingMethodCreationValidator validator;

    public ShippingMethodCreationValidatorTests()
    {
        this.validator = new ShippingMethodCreationValidator();
    }

    [Theory]
    [InlineData(default, default)]
    [InlineData("ShippingMethod1", 0)]
    [InlineData("", 14.4)]
    [InlineData("abc", 14.4)] // checking for min length
    public void CheckingForShippingMethodCreationDto_ShouldNotBeValid(string name, decimal price)
    {
        var shippingMethod = new ShippingMethodCreationDto
        {
            Name = name,
            Price = price
        };

        Assert.False(this.validator.Validate(shippingMethod).IsValid);
    }

    [Theory]
    [InlineData('c', 14.4)]
    public void CheckingForDtoNameMaxLength_ShouldNotBeValid(char c, decimal price)
    {
        var shippingMethod = new ShippingMethodCreationDto
        {
            Name = new string(c, 101),
            Price = price
        };

        Assert.False(this.validator.Validate(shippingMethod).IsValid);
    }

    [Theory]
    [InlineData("abcd", 1.4)]
    public void CheckingForShippingMethodCreationDto_ShouldBeValid(string name, decimal price)
    {
        var shippingMethod = new ShippingMethodCreationDto
        {
            Name = name,
            Price = price
        };

        Assert.True(this.validator.Validate(shippingMethod).IsValid);
    }
}
