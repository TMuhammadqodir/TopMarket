using FluentValidation.TestHelper;
using Service.DTOs.OrderLines;
using Service.Validators.OrderLines;

namespace TopMarket.UnitTest.ValidatorTests.OrderLines;

public class OrderLineUpdateValidatorTests
{
    private readonly OrderLineUpdateValidator orderLineUpdateValidator;

    public OrderLineUpdateValidatorTests()
    {
        this.orderLineUpdateValidator = new OrderLineUpdateValidator();
    }

    [Fact]
    public void ShouldHaveErrorWhenIdIsZero()
    {
        var model = new OrderLineUpdateDto { Id = 0 };
        var result = this.orderLineUpdateValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void ShouldHaveErrorWhenQuantityIsZero()
    {
        var model = new OrderLineUpdateDto { Quantity = 0 };
        var result = this.orderLineUpdateValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Quantity);
    }

    [Fact]
    public void ShouldHaveErrorWhenPriceIsNegative()
    {
        var model = new OrderLineUpdateDto { Price = -1 };
        var result = this.orderLineUpdateValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public void ShouldHaveErrorWhenProductItemIdIsZero()
    {
        var model = new OrderLineUpdateDto { ProductItemId = 0 };
        var result = this.orderLineUpdateValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.ProductItemId);
    }

    [Fact]
    public void ShouldHaveErrorWhenOrderIdIsZero()
    {
        var model = new OrderLineUpdateDto { OrderId = 0 };
        var result = this.orderLineUpdateValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.OrderId);
    }

    [Fact]
    public void ShouldNotHaveErrorWhenModelIsValid()
    {
        var model = new OrderLineUpdateDto
        {
            Id = 1,
            Quantity = 1,
            Price = 10.5m,
            ProductItemId = 1,
            OrderId = 1
        };
        var result = this.orderLineUpdateValidator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
