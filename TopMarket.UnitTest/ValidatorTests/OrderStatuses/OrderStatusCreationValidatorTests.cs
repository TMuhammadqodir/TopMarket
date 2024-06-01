using Data.IRepositories;
using Domain.Entities.OrderFolder;
using Service.DTOs.OrderStatuses;
using Moq;

namespace Service.Validators.OrderStatuses;

public class OrderStatusCreationValidatorTests
{
    private readonly OrderStatusCreationValidator validator;
    private readonly Mock<IRepository<OrderStatus>> repositoryMock = new();

    public OrderStatusCreationValidatorTests()
    {
        this.validator = new(this.repositoryMock.Object);
    }

    [Theory]
    [InlineData(default)]
    [InlineData("")]
    [InlineData("abc")] // checking for min length
    public void CheckingForName_ShouldNotBeValid(string name)
    {
        var orderStatus = new OrderStatusCreationDto { Name = name };
        
        var validationResult = Task.Run(() => this.validator.ValidateAsync(orderStatus)).Result;
        Assert.False(validationResult.IsValid);
    }

    [Theory]
    [InlineData('a')]
    public void CheckingForNameMaxLength_ShouldNotBeValid(char c)
    {
        var orderStatus = new OrderStatusCreationDto { Name = new string(c, 101) };

        var validationResult = Task.Run(() => this.validator.ValidateAsync(orderStatus)).Result;
        Assert.False(validationResult.IsValid);
    }

    [Theory]
    [InlineData("abcd")]
    [InlineData("OrderStatus1")]
    public void CheckingForName_ShouldBeValid(string name)
    {
        var orderStatus = new OrderStatusCreationDto { Name = name };

        var validationResult = Task.Run(() => this.validator.ValidateAsync(orderStatus)).Result;
        Assert.True(validationResult.IsValid);
    }
}
