using Domain.Enums;
using FluentValidation.TestHelper;
using Service.DTOs.UserRewiev;
using Service.Validators.UserRewievs;

public class UserRewievCreationValidatorTest
{
    private readonly UserRewievCreationValidator validator;

    public UserRewievCreationValidatorTest()
    {
        validator = new UserRewievCreationValidator();
    }

    [Fact]
    public void ShouldHaveErrorWhenUserIdIsZero()
    {
        var model = new UserRewievCreationDto { UserId = 0 };
        var result = validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void ShouldHaveErrorWhenOrderProductIdIsZero()
    {
        var model = new UserRewievCreationDto { OrderProductId = 0 };
        var result = validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.OrderProductId);
    }

    [Fact]
    public void ShouldHaveErrorWhenCommentIsEmpty()
    {
        var model = new UserRewievCreationDto { Comment = string.Empty };
        var result = validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Comment);
    }

    [Fact]
    public void ShouldHaveErrorWhenCommentIsTooLong()
    {
        var model = new UserRewievCreationDto { Comment = new string('a', 513) };
        var result = validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Comment);
    }

    [Fact]
    public void ShouldHaveErrorWhenRatingValueIsInvalid()
    {
        var model = new UserRewievCreationDto { RatingValue = (ERating)6 };
        var result = validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.RatingValue);
    }

    [Fact]
    public void ShouldNotHaveErrorWhenModelIsValid()
    {
        var model = new UserRewievCreationDto
        {
            UserId = 1,
            OrderProductId = 1,
            RatingValue = ERating.Good,
            Comment = "This is a valid comment."
        };
        var result = validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
