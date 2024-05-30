using Service.DTOs.Carts;

namespace Service.Validators.Carts;

public class CartItemUpdateValidatorTests
{
    private readonly CartItemUpdateValidator validator;

    public CartItemUpdateValidatorTests()
    {
        this.validator = new CartItemUpdateValidator();
    }

    [Theory]
    [InlineData(0, 0, 0, 0, 0)]
    [InlineData(1, 0, 0, 0, 0)]
    [InlineData(0, 1, 0, 0, 0)]
    [InlineData(0, 0, 1, 0, 0)]
    [InlineData(0, 0, 0, 1, 0)]
    [InlineData(0, 0, 0, 0, 1)]
    public void Should_Be_Equal_To_False(long id, int cartId, long productItemId, decimal price, float quantity)
    {
        var cartItem = new CartItemUpdateDto
        {
            Id = id,
            CartId = cartId,
            ProductItemId = productItemId,
            Price = price,
            Quantity = quantity
        };

        Assert.False(this.validator.Validate(cartItem).IsValid);
    }

    [Theory]
    [InlineData(1, 2, 3, 0.04d, 0.5f)]
    [InlineData(1, 12, 123, 1234, 12345)]
    public void Should_Be_EqualToTrue(long id, int cartId, long productItemId, decimal price, float quantity)
    {
        var cartItem = new CartItemUpdateDto
        {
            Id = id,
            CartId = cartId,
            ProductItemId = productItemId,
            Price = price,
            Quantity = quantity
        };

        Assert.True(this.validator.Validate(cartItem).IsValid);
    }
}
