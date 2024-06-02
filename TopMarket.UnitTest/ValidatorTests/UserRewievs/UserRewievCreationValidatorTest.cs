using Domain.Entities.Addresses;
using Domain.Entities.OrderFolder;
using Domain.Entities.Payment;
using Domain.Enums;
using FluentValidation.TestHelper;
using Service.DTOs.UserRewiev;
using Service.Validators.UserRewievs;

namespace TopMarket.UnitTest.ValidatorTests.UserRewievs;
public class UserRewievCreationValidatorTest
{
    private readonly UserRewievCreationValidator userRewievCreationValidator;

    public UserRewievCreationValidatorTest()
    {
        this.userRewievCreationValidator = new UserRewievCreationValidator();
    }

    [Theory]
    [InlineData(1, 2, (ERating)6, "test")]
    [InlineData(1, 2, (ERating)5, "")]
    [InlineData(1, 0, (ERating)5, "test")]
    [InlineData(0, 2, (ERating)4, "test")]
    public void ShouldBeEqualToFalse(long userId, long orderProductId, ERating rating, string comment)
    {
        var userRewiev = new UserRewievCreationDto
        {
            UserId = userId,
            OrderProductId = orderProductId,
            RatingValue = rating,
            Comment = comment 
        };

        var result = userRewievCreationValidator.Validate(userRewiev);

        Assert.False(result.IsValid);
    }


    [Fact]
    public void ShouldBeEqualToTrue()
    {
        var userRewiev = new UserRewievCreationDto
        {
           UserId = 1,
           OrderProductId = 1,
           RatingValue = ERating.Good,
           Comment = "test"
        };

        var result = userRewievCreationValidator.TestValidate(userRewiev);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
