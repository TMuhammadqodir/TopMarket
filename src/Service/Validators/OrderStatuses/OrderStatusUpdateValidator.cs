﻿using FluentValidation;
using Service.DTOs.OrderStatuses;

namespace Service.Validators.OrderStatuses;

public class OrderStatusUpdateValidator : AbstractValidator<OrderStatusUpdateDto>
{
    public OrderStatusUpdateValidator()
    {
        RuleFor(os => os.Id)
            .NotEmpty()
                .WithMessage("Id should NOT be empty.");

        RuleFor(os => os.Name)
            .NotEmpty()
                .WithMessage("Name should NOT be empty.")
            .MinimumLength(4)
                .WithMessage("Name length should NOT be less than 4.")
            .MaximumLength(100)
                .WithMessage("Name length should NOT be more than 100.");
    }
}