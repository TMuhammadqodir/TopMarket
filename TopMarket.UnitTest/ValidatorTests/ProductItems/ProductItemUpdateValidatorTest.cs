using Service.DTOs.ProductItems;
using Service.Validators.ProductItems;

namespace TopMarket.UnitTest.ValidatorTests.ProductItems;

public class ProductItemUpdateValidatorTest
{
    private ProductItemUpdateValidator productItemUpdateValidator;
    public ProductItemUpdateValidatorTest()
    {
        this.productItemUpdateValidator = new ProductItemUpdateValidator();
    }

    [Theory]
    [InlineData(2,"sa",-2, 54, 2L)]
    [InlineData(3,"as", 9, -4, 0)]
    [InlineData(0,"", 4, 5, 0)]
    [InlineData(4,null, null, 5, null)]
    [InlineData(6,"e3e3", 3, 56, null)]
    [InlineData(0,"e3e3", 3, 56, 3)]
    [InlineData(3,"e3e3", -3, 56, 3)]
    [InlineData(3,"e3e3", 3, -32, 3)]

    public void ShouldReturnFalseForName(long id, string sku, decimal price, decimal quantity, long productId)
    {
        var productItem = new ProductItemUpdateDto()
        {
            Id = id,
            SKU = sku,
            Price = price,
            QuantityInStock = quantity,
            ProductId = productId
        };

        var result = productItemUpdateValidator.Validate(productItem);

        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData(2, "sae", 2, 54, 2L)]
    [InlineData(3, "as", 9, 4, 3)]
    [InlineData(4, "d", 4, 0, 2)]
    [InlineData(4, "sads", 3, 5, 1)]
    [InlineData(6, "e3e3", 3, 56, 1)]

    public void ShouldReturnTrueName(long id, string sku, decimal price, decimal quantity, long productId)
    {
        var productItem = new ProductItemUpdateDto()
        {
            Id = id,
            SKU = sku,
            Price = price,
            QuantityInStock = quantity,
            ProductId = productId
        };

        var result = productItemUpdateValidator.Validate(productItem);

        Assert.True(result.IsValid);
    }
}