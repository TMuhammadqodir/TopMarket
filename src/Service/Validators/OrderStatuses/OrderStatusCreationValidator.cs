using Data.IRepositories;
using Domain.Entities.OrderFolder;
using FluentValidation;
using Service.DTOs.OrderStatuses;

namespace Service.Validators.OrderStatuses;

public class OrderStatusCreationValidator : AbstractValidator<OrderStatusCreationDto>
{
    private readonly IRepository<OrderStatus> repository;

    public OrderStatusCreationValidator(IRepository<OrderStatus> repository)
    {
        this.repository = repository;
        this.SetUpRules();
    }

    private void SetUpRules()
    {
        RuleFor(os => os.Name)
                    .NotEmpty()
                    .MustAsync(IsUnique)
                    .MinimumLength(4)
                    .MaximumLength(100);
    }

    private async Task<bool> IsUnique(string name, CancellationToken token = default)
        => await this.repository
            .GetAsync(os => os.Name.ToLower() == name.ToLower(), cancellationToken: token) is null;
}