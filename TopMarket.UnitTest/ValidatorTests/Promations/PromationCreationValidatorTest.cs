using FluentValidation.TestHelper;
using Service.DTOs.Promotions;
using Xunit;
using System;
using Service.Validators.Promations;

public class PromotionCreationDtoValidatorTest
{
    private readonly PromotionCreationValidator promationCreationValidator;

    public PromotionCreationDtoValidatorTest()
    {
        this.promationCreationValidator = new PromotionCreationValidator();
    }

    [Fact]
    public void ShouldHaveErrorWhenNameIsEmpty()
    {
        var model = new PromotionCreationDto { Name = string.Empty };
        var result = this.promationCreationValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void ShouldHaveErrorWhenNameIsTooLong()
    {
        var model = new PromotionCreationDto { Name = new string('a', 101) };
        var result = this.promationCreationValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void ShouldHaveErrorWhenDescriptionIsEmpty()
    {
        var model = new PromotionCreationDto { Description = string.Empty };
        var result = this.promationCreationValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void ShouldHaveErrorWhenDescriptionIsTooLong()
    {
        var model = new PromotionCreationDto { Description = new string('a', 501) };
        var result = this.promationCreationValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void ShouldHaveErrorWhenDiscountRateIsOutOfRange()
    {
        var model = new PromotionCreationDto { DiscountRate = -1 };
        var result = this.promationCreationValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.DiscountRate);

        model = new PromotionCreationDto { DiscountRate = 101 };
        result = this.promationCreationValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.DiscountRate);
    }

    [Fact]
    public void ShouldHaveErrorWhenStartDateIsNotBeforeEndDate()
    {
        var model = new PromotionCreationDto
        {
            StartDate = DateTime.Now.AddDays(1),
            EndDate = DateTime.Now
        };
        var result = this.promationCreationValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.StartDate);
    }

    [Fact]
    public void ShouldHaveErrorWhenEndDateIsNotAfterStartDate()
    {
        var model = new PromotionCreationDto
        {
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(-1)
        };
        var result = this.promationCreationValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.EndDate);
    }

    [Fact]
    public void ShouldNotHaveErrorWhenModelIsValid()
    {
        var model = new PromotionCreationDto
        {
            Name = "Valid Name",
            Description = "Valid Description",
            DiscountRate = 50,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };
        var result = this.promationCreationValidator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
