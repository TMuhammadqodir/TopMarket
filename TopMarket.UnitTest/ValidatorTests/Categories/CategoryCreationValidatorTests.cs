using Service.DTOs.Categories;

namespace Service.Validators.Categories;

public class CategoryCreationValidatorTests
{
    private CategoryCreationValidator validator;

    public CategoryCreationValidatorTests()
    {
        this.validator = new CategoryCreationValidator();
    }

    [Theory]
    [InlineData(null, null, null)]
    [InlineData("", "", 0L)]
    [InlineData("", "some description goes here", 1L)]
    [InlineData("Abc", "some description goes here", 1L)]
    public void CheckingForName_ShouldReturnFalse(string name, string description, long? parentId)
    {
        var category = new CategoryCreationDto
        {
            Name = name,
            Description = description,
            ParentId = parentId
        };

        Assert.False(this.validator.Validate(category).IsValid);
    }

    [Theory]
    [InlineData('a')]
    public void CheckingForMaximumLengthOfName_ShouldReturnFalse(char c)
    {
        var category = new CategoryCreationDto
        {
            Name = new string(c, 101),
            Description = "",
            ParentId = null
        };

        Assert.False(this.validator.Validate(category).IsValid);
    }

    [Theory]
    [InlineData("Category1", null, null)]
    public void ShouldBeEqualToTrue(string name, string description, long? parentId)
    {
        var category = new CategoryCreationDto
        {
            Name = name,
            Description = description,
            ParentId = parentId
        };

        Assert.True(this.validator.Validate(category).IsValid);
    }
}
