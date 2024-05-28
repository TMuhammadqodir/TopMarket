using FluentValidation.TestHelper;
using Service.DTOs.Promotions;
using Xunit;
using System;
using Service.Validators.Promations;

public class PromotionCreationDtoValidatorTests
{
    private readonly PromotionCreationValidator validator;

    public PromotionCreationDtoValidatorTests()
    {
        this.validator = new PromotionCreationValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var model = new PromotionCreationDto { Name = string.Empty };
        var result = this.validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Too_Long()
    {
        var model = new PromotionCreationDto { Name = new string('a', 101) };
        var result = this.validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Empty()
    {
        var model = new PromotionCreationDto { Description = string.Empty };
        var result = this.validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Too_Long()
    {
        var model = new PromotionCreationDto { Description = new string('a', 501) };
        var result = this.validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Have_Error_When_DiscountRate_Is_Out_Of_Range()
    {
        var model = new PromotionCreationDto { DiscountRate = -1 };
        var result = this.validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.DiscountRate);

        model = new PromotionCreationDto { DiscountRate = 101 };
        result = this.validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.DiscountRate);
    }

    [Fact]
    public void Should_Have_Error_When_StartDate_Is_Not_Before_EndDate()
    {
        var model = new PromotionCreationDto
        {
            StartDate = DateTime.Now.AddDays(1),
            EndDate = DateTime.Now
        };
        var result = this.validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.StartDate);
    }

    [Fact]
    public void Should_Have_Error_When_EndDate_Is_Not_After_StartDate()
    {
        var model = new PromotionCreationDto
        {
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(-1)
        };
        var result = this.validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.EndDate);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Model_Is_Valid()
    {
        var model = new PromotionCreationDto
        {
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
