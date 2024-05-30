using FluentValidation.TestHelper;
using Service.DTOs.Orders;
using Service.Validators.Orders;
using Xunit;

public class OrderCreationValidatorTest
{
    private readonly OrderCreationValidator orderCreationValidator;

    public OrderCreationValidatorTest()
    {
        this.orderCreationValidator = new OrderCreationValidator();
    }

    [Fact]
    public void ShouldHaveErrorWhenDateIsEmpty()
    {
        var model = new OrderCreationDto { Date = default };
        var result = this.orderCreationValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Date);
    }

    [Fact]
    public void ShouldHaveErrorWhenDateIsInFuture()
    {
        var model = new OrderCreationDto { Date = DateTime.Now.AddDays(1) };
        var result = this.orderCreationValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Date);
    }

    [Fact]
    public void ShouldHaveErrorWhenTotalIsNegative()
    {
        var model = new OrderCreationDto { Total = -1 };
        var result = this.orderCreationValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Total);
    }

    [Fact]
    public void ShouldHaveErrorWhenUserIdIsZero()
    {
        var model = new OrderCreationDto { UserId = 0 };
        var result = this.orderCreationValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void ShouldHaveErrorWhenPaymentMethodIdIsZero()
    {
        var model = new OrderCreationDto { PaymentMethodId = 0 };
        var result = this.orderCreationValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.PaymentMethodId);
    }

    [Fact]
    public void ShouldHaveErrorWhenAddressIdIsZero()
    {
        var model = new OrderCreationDto { AddressId = 0 };
        var result = this.orderCreationValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.AddressId);
    }

    [Fact]
    public void ShouldHaveErrorWhenShippingMethodIdIsZero()
    {
        var model = new OrderCreationDto { ShippingMethodId = 0 };
        var result = this.orderCreationValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.ShippingMethodId);
    }

    [Fact]
    public void ShouldHaveErrorWhenStatusIdIsZero()
    {
        var model = new OrderCreationDto { StatusId = 0 };
        var result = this.orderCreationValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.StatusId);
    }

    [Fact]
    public void ShouldNotHaveErrorWhenModelIsValid()
    {
        var model = new OrderCreationDto
        {
            Date = DateTime.Now,
            Total = 100,
            UserId = 1,
            PaymentMethodId = 1,
            AddressId = 1,
            ShippingMethodId = 1,
            StatusId = 1
        };
        var result = this.orderCreationValidator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
