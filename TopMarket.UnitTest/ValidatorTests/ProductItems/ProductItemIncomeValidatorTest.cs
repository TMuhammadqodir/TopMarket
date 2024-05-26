using Service.DTOs.ProductItems;
using Service.Validators.ProductItems;

namespace TopMarket.UnitTest.ValidatorTests.ProductItems;

public class ProductItemIncomeValidatorTest
{
    private ProductItemIncomeValidator productItemIncomeValidator;
    public ProductItemIncomeValidatorTest()
    {
        this.productItemIncomeValidator = new ProductItemIncomeValidator();
    }

    [Theory]
    [InlineData(-2, 2L)]
    [InlineData(9, 0)]
    [InlineData(4, 0)]
    [InlineData(null, null)]
    [InlineData(3, null)]

    public void ShouldReturnFalseForName(decimal quantity, long id)
    {
        var productItem = new ProductItemIncomeDto()
        {
            Id = id,
            QuantityInStock = quantity,
        };

        var result = productItemIncomeValidator.Validate(productItem);

        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData(4, 1L)]
    [InlineData(32423, 2L)]
    [InlineData(434, 1L)]
    [InlineData(0, 1L)]

    public void ShouldReturnTrueName(decimal quantity, long id)
    {
        var productItem = new ProductItemIncomeDto()
        {
            Id = id,
            QuantityInStock= quantity,
        };

        var result = productItemIncomeValidator.Validate(productItem);

        Assert.True(result.IsValid);
    }
}
