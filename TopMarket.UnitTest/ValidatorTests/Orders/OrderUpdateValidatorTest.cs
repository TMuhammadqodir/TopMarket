using FluentValidation.TestHelper;
using Service.DTOs.Orders;
using Service.Validators.Orders;
using Xunit;

namespace TopMarket.UnitTest.ValidatorTests.Orders;

public class OrderUpdateValidatorTest
{
    private readonly OrderUpdateValidator orderUpdateValidator;

    public OrderUpdateValidatorTest()
    {
        this.orderUpdateValidator = new OrderUpdateValidator();
    }

    [Theory]
    [InlineData(1,100, 1, 1, 0, 1, 1)]
    [InlineData(1,100, 0, 1, 2, 1, 1)]
    [InlineData(1,100, 1, 0, 1, 1, 1)]
    [InlineData(1,-6, 1, 1, 1, 1, 1)]
    [InlineData(0,0, 1, 1, 1, 1, 1)]
    [InlineData(1,100, 0, 1, 0, 1, 1)]
    [InlineData(1,100, 0, 0, 0, 1, 1)]
    public void ShouldBeEqualToFalse(long id, decimal total, long userId, long paymentMethodId, long addressId, long shippingMethodId, long statusId)
    {
        var order = new OrderUpdateDto
        {
            Id = id,
            Date = DateTime.Now,
            Total = total,
            UserId = userId,
            PaymentMethodId = paymentMethodId,
            AddressId = addressId,
            ShippingMethodId = shippingMethodId,
            StatusId = statusId
        };

        var result = orderUpdateValidator.Validate(order);

        Assert.False(result.IsValid);
    }


    [Fact]
    public void ShouldBeEqualToTrue()
    {
        var order = new OrderUpdateDto
        {
            Id = 2,
            Date = DateTime.Now,
            Total = 122,
            UserId = 11,
            PaymentMethodId = 22,
            AddressId = 3,
            ShippingMethodId = 33,
            StatusId = 2
        };

        var result = orderUpdateValidator.TestValidate(order);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
