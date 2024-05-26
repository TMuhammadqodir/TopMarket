using Service.DTOs.Products;
using Service.Validators.Products;

namespace TopMarket.UnitTest.ValidatorTests.Products;

public class ProductUpdateValidatorTest
{
    private ProductUpdateValidator productUpdateValidator;
    public ProductUpdateValidatorTest()
    {
        this.productUpdateValidator = new ProductUpdateValidator();
    }

    [Theory]
    [InlineData(1, "uzbekistan", "", 1L)]
    [InlineData(2, "", "uz", 2L)]
    [InlineData(3, "", "", 0)]
    [InlineData(4, "fd", "fd", 0)]
    [InlineData(5, "u", "u", 1L)]
    [InlineData(0, "uz", null, null)]
    [InlineData(0, "32d", "ds", 2)]
    [InlineData(null, null, null, null)]

    public void ShouldReturnFalseForName(long id, string name, string description, long categoryId)
    {
        var product = new ProductUpdateDto()
        {
            Id = id,
            Name = name,
            Description = description,
            CategoryId = categoryId
        };

        var result = productUpdateValidator.Validate(product);

        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData(1, "uzbekistan", "dw", 1L)]
    [InlineData(2, "dew", "uz", 2L)]
    [InlineData(3, "de", "dwd", 2L)]

    public void ShouldReturnTrueName(long id, string name, string description, long categoryId)
    {
        var product = new ProductUpdateDto()
        {
            Id = id,
            Name = name,
            Description = description,
            CategoryId = categoryId
        };

        var result = productUpdateValidator.Validate(product);

        Assert.True(result.IsValid);
    }
}
