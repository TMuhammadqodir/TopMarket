using AutoMapper;
using Data.IRepositories;
using Domain.Entities.OrderFolder;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.DTOs.OrderStatuses;
using Service.Exceptions;
using Service.Interfaces;

namespace Service.Services;

public class OrderStatusService : IOrderStatusService
{
    private readonly ILogger<OrderStatusService> logger;
    private readonly IMapper mapper;
    private readonly IRepository<OrderStatus> repository;
    private readonly IValidator<OrderStatusCreationDto> orderStatusCreationValidator;
    private readonly IValidator<OrderStatusUpdateDto> orderStatusUpdateValidator;

    public OrderStatusService(
        ILogger<OrderStatusService> logger,
        IMapper mapper,
        IRepository<OrderStatus> repository,
        IValidator<OrderStatusCreationDto> orderStatusCreationValidator,
        IValidator<OrderStatusUpdateDto> orderStatusUpdateValidator)
    {
        this.logger = logger;
        this.mapper = mapper;
        this.repository = repository;
        this.orderStatusCreationValidator = orderStatusCreationValidator;
        this.orderStatusUpdateValidator = orderStatusUpdateValidator;
    }

    public async Task<OrderStatusResultDto> CreateAsync(OrderStatusCreationDto dto, CancellationToken cancellationToken = default)
    {
        var validationResult = await this.orderStatusCreationValidator.ValidateAsync(dto, cancellationToken);
        if(!validationResult.IsValid)
        {
            this.logger.LogError("Order status has NOT been created. See details: {@errors}", validationResult.Errors);
            throw new CustomException(StatusCodes.Status409Conflict, "Invalid order status");
        }

        var newOrderStatus = this.mapper.Map<OrderStatus>(dto);

        await this.repository.AddAsync(newOrderStatus, cancellationToken);
        await this.repository.SaveAsync(cancellationToken);
        this.logger.LogInformation("Order status '{name}' has been successfully created.", newOrderStatus.Name);

        return this.mapper.Map<OrderStatusResultDto>(newOrderStatus);
    }

    public async Task<OrderStatusResultDto> ModifyAsync(OrderStatusUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var validationResult = await this.orderStatusUpdateValidator.ValidateAsync(dto, cancellationToken);
        if(!validationResult.IsValid)
        {
            this.logger.LogError("Order status has NOT been updated. See details: {@errors}", validationResult.Errors);
            throw new CustomException(StatusCodes.Status409Conflict, "Invalid order status");
        }
        
        var oldOrderStatus = await this.repository.GetAsync(dto.Id, cancellationToken: cancellationToken);
        
        var modifiedOrderStatus = this.mapper.Map(dto, oldOrderStatus);

        this.repository.Update(modifiedOrderStatus!);
        await this.repository.SaveAsync(cancellationToken);
        this.logger.LogInformation("Order status has been successfully modified.");

        return this.mapper.Map<OrderStatusResultDto>(modifiedOrderStatus);
    }

    public async Task<bool> RemoveAsync(long id, bool destroy = false, CancellationToken cancellationToken = default)
    {
        var orderStatus = await this.repository.GetAsync(id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"OrderStatus with id = {id} is not found.");

        try
        {
            if (destroy)
                this.repository.Destroy(orderStatus);
            else
                this.repository.Delete(orderStatus);
            
            await this.repository.SaveAsync(cancellationToken);
            this.logger.LogInformation("OrderStatus has been successfully {action}.", destroy ? "destroyed" : "deleted");
            
            return true;
        }
        catch (Exception ex)
        {
            this.logger.LogError("OrderStatus has NOT been deleted. See details {@exception}", ex);
            return false;
        }
    }

    public async Task<OrderStatusResultDto> RetrieveByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var orderStatus = await this.repository.GetAsync(id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Order status with id = {id} is not found.");

        return this.mapper.Map<OrderStatusResultDto>(orderStatus);
    }

    public async Task<IEnumerable<OrderStatusResultDto>> RetrieveAllAsync(CancellationToken cancellationToken = default)
    {
        var categories = await this.repository
                .GetAll()
                .ToListAsync(cancellationToken: cancellationToken);

        return this.mapper.Map<IEnumerable<OrderStatusResultDto>>(categories);
    }
}
