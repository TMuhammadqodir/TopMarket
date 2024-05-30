using Service.DTOs.OrderStatuses;

namespace Service.Validators.OrderStatuses;

public class OrderStatusCreationValidatorTests
{
    private readonly OrderStatusCreationValidator validator;

    public OrderStatusCreationValidatorTests()
    {
        this.validator = new OrderStatusCreationValidator(); //TODO Saidkamol aka: Xatolik bermoqda
    }

    [Theory]
    [InlineData(default)]
    [InlineData("")]
    [InlineData("abc")] // checking for min length
    public void CheckingForName_ShouldNotBeValid(string name)
    {
        var orderStatus = new OrderStatusCreationDto
        {
            Name = name
        };

        Assert.False(this.validator.Validate(orderStatus).IsValid);
    }

    [Theory]
    [InlineData('a')]
    public void CheckingForNameMaxLength_ShouldNotBeValid(char c)
    {
        var orderStatus = new OrderStatusCreationDto
        {
            Name = new string(c, 101)
        };

        Assert.False(this.validator.Validate(orderStatus).IsValid);
    }

    [Theory]
    [InlineData("abcd")]
    [InlineData("OrderStatus1")]
    public void CheckingForName_ShouldBeValid(string name)
    {
        var orderStatus = new OrderStatusCreationDto
        {
            Name = name
        };

        Assert.True(this.validator.Validate(orderStatus).IsValid);
    }
}
