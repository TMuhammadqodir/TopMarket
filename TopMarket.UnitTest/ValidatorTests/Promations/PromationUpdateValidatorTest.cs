using FluentValidation.TestHelper;
using Service.DTOs.Promotions;
using Xunit;
using System;
using Service.Validators.Promations;

public class PromotionUpdateDtoValidatorTest
{
    private readonly PromotionUpdateValidator validator;

    public PromotionUpdateDtoValidatorTest()
    {
        this.validator = new PromotionUpdateValidator();
    }

    [Fact]
    public void ShouldHaveErrorWhenIdIsZero()
    {
        var model = new PromotionUpdateDto { Id = 0 };
        var result = this.validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void ShouldHaveErrorWhenNameIsEmpty()
    {
        var model = new PromotionUpdateDto { Name = string.Empty };
        var result = this.validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void ShouldHaveErrorWhenNameIsTooLong()
    {
        var model = new PromotionUpdateDto { Name = new string('a', 101) };
        var result = this.validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void ShouldHaveErrorWhenDescriptionIsEmpty()
    {
        var model = new PromotionUpdateDto { Description = string.Empty };
        var result = this.validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void ShouldHaveErrorWhenDescriptionIsTooLong()
    {
        var model = new PromotionUpdateDto { Description = new string('a', 501) };
        var result = this.validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void ShouldHaveErrorWhenDiscountRateIsOutOfRange()
    {
        var model = new PromotionUpdateDto { DiscountRate = -1 };
        var result = this.validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.DiscountRate);

        model = new PromotionUpdateDto { DiscountRate = 101 };
        result = this.validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.DiscountRate);
    }

    [Fact]
    public void ShouldHaveErrorWhenStartDateIsNotBeforeEndDate()
    {
        var model = new PromotionUpdateDto
        {
            StartDate = DateTime.Now.AddDays(1),
            EndDate = DateTime.Now
        };
        var result = this.validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.StartDate);
    }

    [Fact]
    public void ShouldHaveErrorWhenEndDateIsNotAfterStartDate()
    {
        var model = new PromotionUpdateDto
        {
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(-1)
        };
        var result = this.validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.EndDate);
    }

    [Fact]
    public void ShouldNotHaveErrorWhenModelIsValid()
    {
        var model = new PromotionUpdateDto
        {
            Id = 1,
            Name = "Valid Name",
            Description = "Valid Description",
            DiscountRate = 50,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };
        var result = this.validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
