using Service.DTOs.Carts;

namespace Service.Validators.Carts;

public class CartItemCreationValidatorTests
{
    private readonly CartItemCreationValidator validator;
    
    public CartItemCreationValidatorTests()
    {
        this.validator = new CartItemCreationValidator();
    }

    [Theory]
    [InlineData(0, 0, 0, 0)]
    [InlineData(1, 0, 0, 0)]
    [InlineData(0, 1, 0, 0)]
    [InlineData(0, 0, 1, 0)]
    [InlineData(0, 0, 0, 1)]
    public void Should_Be_Equal_To_False(long cartId, long productItemId, decimal price, float quantity)
    {
        var cartItem = new CartItemCreationDto
        {
            CartId = cartId,
            ProductItemId = productItemId,
            Price = price,
            Quantity = quantity
        };
        
        Assert.False(this.validator.Validate(cartItem).IsValid);
    }

    [Theory]
    [InlineData(1, 1, 0.01d, 0.1f)]
    [InlineData(1, 12, 123, 1234)]
    public void Should_Be_Equal_To_True(long cartId, long productItemId, decimal price, float quantity)
    {
        var cartItem = new CartItemCreationDto
        {
            CartId = cartId,
            ProductItemId = productItemId,
            Price = price,
            Quantity = quantity
        };

        Assert.True(this.validator.Validate(cartItem).IsValid);
    }
}
