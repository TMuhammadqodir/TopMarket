using FluentValidation;
using Service.DTOs.Payments.PaymentType;
using Xunit.Sdk;

namespace Service.Validators.Payments;

public class PaymentTypeUpdateValidatorTests
{
    private readonly PaymentTypeUpdateValidator validator;

    public PaymentTypeUpdateValidatorTests()
    {
        this.validator = new PaymentTypeUpdateValidator();
    }

    [Theory]
    [InlineData(default, default)]
    [InlineData(1L, default)]
    [InlineData(default, "SomeValue")]
    [InlineData(1L, "abc")] // checking for min length
    public void PaymentTypeUpdateDto_ShouldNotBeValid(long id, string value)
    {
        var paymentType = new PaymentTypeUpdateDto
        {
            Id = id,
            Value = value
        };

        Assert.False(this.validator.Validate(paymentType).IsValid);
    }

    [Theory]
    [InlineData(1L, 'a')] // checking for max length
    public void PaymentTypeUpdateDto_ShouldNotBeValid2(long id, char c)
    {
        var paymentType = new PaymentTypeUpdateDto
        {
            Id = id,
            Value = new string(c, 101)
        };

        Assert.False(this.validator.Validate(paymentType).IsValid);
    }

    [Theory]
    [InlineData(1L, "abcd")]
    [InlineData(1L, "SomeValue")]
    public void PaymentTypeUpdateDto_ShouldBeValid(long id, string value)
    {
        var paymentType = new PaymentTypeUpdateDto { Id = id, Value = value };

        Assert.True(this.validator.Validate(paymentType).IsValid);
    }
}
