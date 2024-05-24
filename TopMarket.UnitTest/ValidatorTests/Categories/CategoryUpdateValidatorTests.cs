using Service.DTOs.Categories;

namespace Service.Validators.Categories;

public class CategoryUpdateValidatorTests
{
    private readonly CategoryUpdateValidator validator;

    public CategoryUpdateValidatorTests()
    {
        this.validator = new CategoryUpdateValidator();
    }

    [Theory]
    [InlineData(1, null, null, null)]
    [InlineData(2, "", "", 0L)]
    [InlineData(3, "", "some description goes here", 1L)]
    [InlineData(4, "Abc", "some description goes here", 1L)]
    [InlineData(1, "Product1", "some description goes here", 1L)] // id and parentId are same
    public void FullChecking_ShouldReturnFalse(long id, string name, string description, long? parentId)
    {
        var category = new CategoryUpdateDto
        {
            Id = id,
            Name = name,
            Description = description,
            ParentId = parentId
        };

        Assert.False(this.validator.Validate(category).IsValid);
    }

    [Theory]
    [InlineData('a')]
    public void CheckingMaximumLengthOfName_ShouldReturnFalse(char c)
    {
        var category = new CategoryUpdateDto
        {
            Name = new string(c, 101),
            Description = "",
            ParentId = null
        };

        Assert.False(this.validator.Validate(category).IsValid);
    }

    [Theory]
    [InlineData(1L, "Category1", null, null)]
    [InlineData(1L, "Category1", null, 2L)]
    public void ShouldBeEqualToTrue(long id, string name, string description, long? parentId)
    {
        var category = new CategoryUpdateDto
        {
            Id = id,
            Name = name,
            Description = description,
            ParentId = parentId
        };

        Assert.True(this.validator.Validate(category).IsValid);
    }
}
