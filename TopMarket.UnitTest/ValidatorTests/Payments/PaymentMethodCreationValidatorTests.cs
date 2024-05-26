using Service.DTOs.Payments.PaymentMethods;

namespace Service.Validators.Payments;

public class PaymentMethodCreationValidatorTests
{
    private PaymentMethodCreationValidator validator;

    public PaymentMethodCreationValidatorTests()
    {
        this.validator = new PaymentMethodCreationValidator();
    }

    [Theory]
    [InlineData(default, default, default, default, default)]
    [InlineData("Provider1", default, default, default, default)]
    [InlineData("Provider1", 1L, default, default, default)]
    [InlineData("Provider1", 1L, "Acc0001", default, default)]
    [InlineData("Provider1", 1L, "Acc0001", 2L, default)]
    public void ShouldBeEqualToFalse(string provider, long userId, string accountNumber, long paymentTypeId, DateTime expDate)
    {
        var paymentMethod = new PaymentMethodCreationDto
        {
            Provider = provider,
            UserId = userId,
            AccountNumber = accountNumber,
            PaymentTypeId = paymentTypeId,
            ExpiryDate = expDate,
        };

        Assert.False(this.validator.Validate(paymentMethod).IsValid);
    }

    [Theory]
    [InlineData("Provider1", 1L, "Acc0001", 2L, "2025-12-31")]
    public void ShouldBeEqualToTrue(string provider, long userId, string accountNumber, long paymentTypeId, DateTime expDate)
    {
        var paymentMethod = new PaymentMethodCreationDto
        {
            Provider = provider,
            UserId = userId,
            AccountNumber = accountNumber,
            PaymentTypeId = paymentTypeId,
            ExpiryDate = expDate
        };

        Assert.True(this.validator.Validate(paymentMethod).IsValid);
    }
}
