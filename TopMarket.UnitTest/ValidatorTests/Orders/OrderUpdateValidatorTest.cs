using FluentValidation.TestHelper;
using Service.DTOs.Orders;
using Service.Validators.Orders;
using Xunit;

public class OrderUpdateValidatorTest
{
    private readonly OrderUpdateValidator orderUpdateValidator;

    public OrderUpdateValidatorTest()
    {
        this.orderUpdateValidator = new OrderUpdateValidator();
    }

    [Fact]
    public void ShouldHaveErrorWhenIdIsZero()
    {
        var model = new OrderUpdateDto { Id = 0 };
        var result = this.orderUpdateValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void ShouldHaveErrorWhenDateIsEmpty()
    {
        var model = new OrderUpdateDto { Date = default };
        var result = this.orderUpdateValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Date);
    }

    [Fact]
    public void ShouldHaveErrorWhenDateIsInFuture()
    {
        var model = new OrderUpdateDto { Date = DateTime.Now.AddDays(1) };
        var result = this.orderUpdateValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Date);
    }

    [Fact]
    public void ShouldHaveErrorWhenTotalIsNegative()
    {
        var model = new OrderUpdateDto { Total = -1 };
        var result = this.orderUpdateValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Total);
    }

    [Fact]
    public void ShouldHaveErrorWhenUserIdIsZero()
    {
        var model = new OrderUpdateDto { UserId = 0 };
        var result = this.orderUpdateValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void ShouldHaveErrorWhenPaymentMethodIdIsZero()
    {
        var model = new OrderUpdateDto { PaymentMethodId = 0 };
        var result = this.orderUpdateValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.PaymentMethodId);
    }

    [Fact]
    public void ShouldHaveErrorWhenAddressIdIsZero()
    {
        var model = new OrderUpdateDto { AddressId = 0 };
        var result = this.orderUpdateValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.AddressId);
    }

    [Fact]
    public void ShouldHaveErrorWhenShippingMethodIdIsZero()
    {
        var model = new OrderUpdateDto { ShippingMethodId = 0 };
        var result = this.orderUpdateValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.ShippingMethodId);
    }

    [Fact]
    public void ShouldHaveErrorWhenStatusIdIsZero()
    {
        var model = new OrderUpdateDto { StatusId = 0 };
        var result = this.orderUpdateValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.StatusId);
    }

    [Fact]
    public void ShouldNotHaveErrorWhenModelIsValid()
    {
        var model = new OrderUpdateDto
        {
            Id = 1,
            Date = DateTime.Now,
            Total = 100,
            UserId = 1,
            PaymentMethodId = 1,
            AddressId = 1,
            ShippingMethodId = 1,
            StatusId = 1
        };
        var result = this.orderUpdateValidator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
