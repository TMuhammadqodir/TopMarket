﻿using FluentValidation;
using Service.DTOs.OrderLines;

namespace Service.Validators.OrderLines
{
    public class OrderLineUpdateValidator : AbstractValidator<OrderLineUpdateDto>
    {
        public OrderLineUpdateValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                    .WithMessage("Id must be greater than zero.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                    .WithMessage("Quantity must be greater than zero.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0)
                    .WithMessage("Price must be greater than or equal to zero.");

            RuleFor(x => x.ProductItemId)
                .GreaterThan(0)
                    .WithMessage("ProductItemId must be greater than zero.");

            RuleFor(x => x.OrderId)
                .GreaterThan(0)
                    .WithMessage("OrderId must be greater than zero.");
        }
    }
}