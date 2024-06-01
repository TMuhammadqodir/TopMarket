using Data.IRepositories;
using Domain.Entities.OrderFolder;
using FluentValidation;
using Moq;
using Service.DTOs.OrderStatuses;

namespace Service.Validators.OrderStatuses;

public class OrderStatusUpdateValidatorTests
{
    private readonly OrderStatusUpdateValidator validator;
    private readonly Mock<IRepository<OrderStatus>> repositoryMock = new();

    public OrderStatusUpdateValidatorTests()
    {
        this.validator = new(this.repositoryMock.Object);
    }

    [Theory]
    [InlineData(default, default)]
    [InlineData(0L, "")]
    [InlineData(1L, "")]
    [InlineData(1L, "abc")] // checking for min length
    public void CheckingForName_ShouldNotBeValid(long id, string name)
    {
        var orderStatus = new OrderStatusUpdateDto
        {
            Id = id,
            Name = name
        };

        var validationResult = Task.Run(() => this.validator.ValidateAsync(orderStatus)).Result;
        Assert.False(validationResult.IsValid);
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

        var validationResult = Task.Run(() => this.validator.ValidateAsync(orderStatus)).Result;
        Assert.False(validationResult.IsValid);
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

        var validationResult = Task.Run(() => this.validator.ValidateAsync(orderStatus)).Result;
        Assert.True(validationResult.IsValid);
    }
}
