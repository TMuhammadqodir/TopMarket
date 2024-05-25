using Service.DTOs.Payments.PaymentType;

namespace Service.Validators.Payments;

public class PaymentTypeCreationValidatorTests
{
    private readonly PaymentTypeCreationValidator validator;

    public PaymentTypeCreationValidatorTests()
    {
        this.validator = new PaymentTypeCreationValidator();
    }

    [Theory]
    [InlineData(default)]
    [InlineData("")]
    [InlineData("abc")]
    public void PaymentTypeCreationDto_ShouldNotBeValid(string value)
    {
        var paymentType = new PaymentTypeCreationDto
        {
            Value = value
        };

        Assert.False(this.validator.Validate(paymentType).IsValid);
    }

    [Theory]
    [InlineData('a')]
    public void PaymentTypeCreationDto_ShouldNotBeValid2(char c)
    {
        var paymentType = new PaymentTypeCreationDto
        {
            Value = new string(c, 101)
        };

        Assert.False(this.validator.Validate(paymentType).IsValid);
    }

    [Theory]
    [InlineData("abcd")]
    [InlineData("SomeValue")]
    public void PaymentTypeCreationDto_ShouldBeValid(string value)
    {
        var paymentType = new PaymentTypeCreationDto
        {
            Value = value
        };

        Assert.True(this.validator.Validate(paymentType).IsValid);
    }
}
