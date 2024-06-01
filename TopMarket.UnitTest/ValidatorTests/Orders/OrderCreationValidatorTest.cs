using FluentValidation.TestHelper;
using Service.DTOs.Orders;
using Service.Validators.Orders;

namespace TopMarket.UnitTest.ValidatorTests.Orders;

public class OrderCreationValidatorTest
{
    private readonly OrderCreationValidator orderCreationValidator;

    public OrderCreationValidatorTest()
    {
        this.orderCreationValidator = new OrderCreationValidator();
    }

    [Theory]
    [InlineData(100, 1, 1, 0, 1, 1)]
    [InlineData(100, 0, 1, 2, 1, 1)]
    [InlineData(100, 1, 0, 1, 1, 1)]
    [InlineData(-6, 1, 1, 1, 1, 1)]
    [InlineData(100, 0, 1, 0, 1, 1)]
    [InlineData(100, 0, 0, 0, 1, 1)]
    public void ShouldBeEqualToFalse(decimal total, long userId, long paymentMethodId, long addressId, long shippingMethodId, long statusId)
    {
        var order = new OrderCreationDto
        {
            Date = DateTime.Now,
            Total = total,
            UserId = userId,
            PaymentMethodId = paymentMethodId,
            AddressId = addressId,
            ShippingMethodId = shippingMethodId,
            StatusId = statusId
        };

        var result = orderCreationValidator.Validate(order);

        Assert.False(result.IsValid);
    }


    [Fact]
    public void ShouldBeEqualToTrue()
    {
        var order = new OrderCreationDto
        {
            Date = DateTime.Now,
            Total = 122,
            UserId = 11,
            PaymentMethodId = 22,
            AddressId = 3,
            ShippingMethodId = 33,
            StatusId = 2
        };

        var result = orderCreationValidator.TestValidate(order);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
