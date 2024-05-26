using Domain.Entities.ProductFolder;
using Service.DTOs.Products;
using Service.Validators.Products;
using System.Xml.Linq;

namespace TopMarket.UnitTest.ValidatorTests.Products;

public class ProductCreationValidatorTest
{
    private ProductCreationValidator productCreationValidator;
    public ProductCreationValidatorTest()
    {
        this.productCreationValidator = new ProductCreationValidator();
    }

    [Theory]
    [InlineData("uzbekistan", "", 1L)]
    [InlineData("", "uz", 2L)]
    [InlineData("", "", 0)]
    [InlineData("fd", "fd", 0)]
    [InlineData(null, "udd", 1L)]
    [InlineData("uz",null, null)]
    [InlineData("", "ds", 2)]
    [InlineData(null, null, null)]

    public void ShouldReturnFalseForName(string name, string description, long categoryId)
    {
        var product = new ProductCreationDto()
        {
            Name = name,
            Description = description,
            CategoryId = categoryId
        };

        var result = productCreationValidator.Validate(product);

        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData('u')]
    public void ShouldReturnFalseForNameLenth(char character)
    {
        var product = new ProductCreationDto()
        {
            Name = new string(character, 129),
            Description = new string(character, 513),
            CategoryId = 5
        };

        var result = productCreationValidator.Validate(product);

        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData("uzbekistan", "dw", 1L)]
    [InlineData("dew", "uz", 2L)]
    [InlineData("de", "dwd", 2L)]

    public void ShouldReturnTrueName(string name, string description, long categoryId)
    {
        var product = new ProductCreationDto()
        {
            Name = name,
            Description = description,
            CategoryId = categoryId
        };

        var result = productCreationValidator.Validate(product);

        Assert.True(result.IsValid);
    }
}
