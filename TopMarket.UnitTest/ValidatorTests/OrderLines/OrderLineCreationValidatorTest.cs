using FluentValidation.TestHelper;
using Service.DTOs.OrderLines;
using Service.Validators.OrderLines;
using Xunit;

public class OrderLineCreationValidatorTest
{
    private readonly OrderLineCreationValidator orderLineCreationValidator;

    public OrderLineCreationValidatorTest()
    {
        this.orderLineCreationValidator = new OrderLineCreationValidator();
    }

    [Fact]
    public void ShouldHaveErrorWhenQuantityIsZero()
    {
        var model = new OrderLineCreationDto { Quantity = 0 };
        var result = this.orderLineCreationValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Quantity);
    }

    [Fact]
    public void ShouldHaveErrorWhenPriceIsNegative()
    {
        var model = new OrderLineCreationDto { Price = -1 };
        var result = this.orderLineCreationValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public void ShouldHaveErrorWhenProductItemIdIsZero()
    {
        var model = new OrderLineCreationDto { ProductItemId = 0 };
        var result = this.orderLineCreationValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.ProductItemId);
    }

    [Fact]
    public void ShouldHaveErrorWhenOrderIdIsZero()
    {
        var model = new OrderLineCreationDto { OrderId = 0 };
        var result = this.orderLineCreationValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.OrderId);
    }

    [Fact]
    public void ShouldNotHaveErrorWhenModelIsValid()
    {
        var model = new OrderLineCreationDto
        {
            Quantity = 1,
            Price = 10.5m,
            ProductItemId = 1,
            OrderId = 1
        };
        var result = this.orderLineCreationValidator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
