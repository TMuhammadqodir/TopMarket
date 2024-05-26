using Data.IRepositories;
using Domain.Entities.OrderFolder;
using FluentValidation;
using Service.DTOs.OrderStatuses;

namespace Service.Validators.OrderStatuses;

public class OrderStatusUpdateValidator : AbstractValidator<OrderStatusUpdateDto>
{
    private readonly IRepository<OrderStatus> repository;

    public OrderStatusUpdateValidator(IRepository<OrderStatus> repository)
    {
        this.repository = repository;
        this.SetUpRules();
    }

    private void SetUpRules()
    {
        RuleFor(os => os.Id).NotEmpty();

        RuleFor(os => os.Name)
            .NotEmpty()
            .MinimumLength(4)
            .MaximumLength(100);

        RuleFor(os => os)
            .MustAsync(HasUniqueName)
                .WithMessage("Name should be unique.");
    }

    private async Task<bool> HasUniqueName(OrderStatusUpdateDto dto, CancellationToken cancellationToken = default)
    {
        return await this.repository.GetAsync(os => 
            os.Name.ToLower() == dto.Name.ToLower() &&
            os.Id != dto.Id, cancellationToken: cancellationToken
        ) is null;
    }
}