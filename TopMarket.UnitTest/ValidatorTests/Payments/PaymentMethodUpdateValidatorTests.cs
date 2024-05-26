using Service.DTOs.Payments.PaymentMethods;

namespace Service.Validators.Payments;

public class PaymentMethodUpdateValidatorTests
{
    private readonly PaymentMethodUpdateValidator validator;

    public PaymentMethodUpdateValidatorTests()
    {
        this.validator = new PaymentMethodUpdateValidator();
    }

    [Theory]
    [InlineData(default, default, default, default)]
    [InlineData(1L, default, default, default)]
    [InlineData(1L, "Provider1", default, default)]
    [InlineData(1L, "Provider1", "Acc0001", default)]
    [InlineData(1L, default, default, "2025-12-31")]
    public void ShouldBeEqualToFalse(long id, string provider, string accountNumber, DateTime expDate)
    {
        var paymentMethod = new PaymentMethodUpdateDto
        {   
            Id = id,
            Provider = provider,
            AccountNumber = accountNumber,
            ExpiryDate = expDate
        };

        Assert.False(this.validator.Validate(paymentMethod).IsValid);
    }

    [Theory]
    [InlineData(1L, "Provider1", "Acc0001", "2025-12-31")]
    public void ShouldBeEqualToTrue(long id, string provider, string accountNumber, DateTime expDate)
    {
        var paymentMethod = new PaymentMethodUpdateDto
        {
            Id = id,
            Provider = provider,
            AccountNumber = accountNumber,
            ExpiryDate = expDate
        };

        Assert.True(this.validator.Validate(paymentMethod).IsValid);
    }
}
