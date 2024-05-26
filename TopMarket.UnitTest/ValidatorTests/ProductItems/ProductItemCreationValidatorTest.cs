using Service.DTOs.ProductItems;
using Service.Validators.ProductItems;

namespace TopMarket.UnitTest.ValidatorTests.ProductItems;

public class ProductItemCreationValidatorTest
{
    private ProductItemCreationValidator productItemCreationValidator;
    public ProductItemCreationValidatorTest()
    {
        this.productItemCreationValidator = new ProductItemCreationValidator();
    }

    [Theory]
    [InlineData(-2, 2L)]
    [InlineData(9, 0)]
    [InlineData(4, 0)]
    [InlineData(null, null)]
    [InlineData(3, null)]

    public void ShouldReturnFalseForName(decimal price, long productId)
    {
        var productItem = new ProductItemCreationDto()
        {
            Price = price,
            ProductId = productId
        };

        var result = productItemCreationValidator.Validate(productItem);

        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData(4, 1L)]
    [InlineData(32423, 2L)]
    [InlineData(434, 1L)]
    [InlineData(0, 1L)]

    public void ShouldReturnTrueName(decimal price, long productId)
    {
        var productItem = new ProductItemCreationDto()
        {
            Price = price,
            ProductId = productId
        };

        var result = productItemCreationValidator.Validate(productItem);

        Assert.True(result.IsValid);
    }
}
