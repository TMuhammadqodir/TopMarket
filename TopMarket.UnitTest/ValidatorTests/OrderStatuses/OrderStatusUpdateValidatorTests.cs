using Service.DTOs.OrderStatuses;

namespace Service.Validators.OrderStatuses;

public class OrderStatusUpdateValidatorTests
{
    private readonly OrderStatusUpdateValidator validator;

    public OrderStatusUpdateValidatorTests()
    {
        this.validator = new OrderStatusUpdateValidator();
    }

    [Theory]
    [InlineData(default, default)]
    [InlineData(0L, "")]
    [InlineData(1L, "")]
    [InlineData(1L, "abc")]// checking for min length
    public void CheckingForName_ShouldNotBeValid(long id, string name)
    {
        var orderStatus = new OrderStatusUpdateDto
        {
            Id = id,
            Name = name
        };

        Assert.False(this.validator.Validate(orderStatus).IsValid);
    }

    [Theory]
    [InlineData(1L, 'a')]
    public void CheckingForNameMaxLength_ShouldNotBeValid(long id, char c)
    {
        var orderStatus = new OrderStatusUpdateDto
        {
            Id = id,
            Name = new string(c, 101)
        };

        Assert.False(this.validator.Validate(orderStatus).IsValid);
    }

    [Theory]
    [InlineData(1L, "abcd")]
    [InlineData(2L, "OrderStatus1")]
    public void CheckingForName_ShouldBeValid(long id, string name)
    {
        var orderStatus = new OrderStatusUpdateDto
        {
            Id = id,
            Name = name
        };

        Assert.True(this.validator.Validate(orderStatus).IsValid);
    }
}
