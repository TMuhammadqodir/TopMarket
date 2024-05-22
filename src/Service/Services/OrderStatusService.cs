using AutoMapper;
using Data.IRepositories;
using Domain.Entities.OrderFolder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.DTOs.OrderStates;
using Service.Exceptions;
using Service.Interfaces;

namespace Service.Services;

public class OrderStatusService : IOrderStatusService
{
    private readonly ILogger<OrderStatusService> logger;
    private readonly IMapper mapper;
    private readonly IRepository<OrderStatus> repository;
    public OrderStatusService(
        ILogger<OrderStatusService> logger,
        IMapper mapper,
        IRepository<OrderStatus> repository)
    {
        this.logger = logger;
        this.mapper = mapper;
        this.repository = repository;
    }

    public async Task<OrderStatusResultDto> CreateAsync(OrderStatusCreationDto dto, CancellationToken cancellationToken = default)
    {
        var existOrderStatus = await this.repository.GetAsync(c => c.Name.ToLower().Equals(dto.Name.ToLower()));
        if (existOrderStatus is not null)
            throw new AlreadyExistException($"This orderStatus already exist with {dto.Name}");

        var mappedOrderStatus = this.mapper.Map<OrderStatus>(dto);

        await this.repository.AddAsync(mappedOrderStatus);
        await this.repository.SaveAsync();

        return this.mapper.Map<OrderStatusResultDto>(mappedOrderStatus);
    }

    public async Task<OrderStatusResultDto> ModifyAsync(OrderStatusUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var orderStatus = await this.repository.GetAsync(dto.Id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"OrderStatus with id = {dto.Id} is not found.");
        
        var mappedOrderStatus = this.mapper.Map(dto, orderStatus);

        this.repository.Update(mappedOrderStatus);
        await this.repository.SaveAsync(cancellationToken);

        return this.mapper.Map<OrderStatusResultDto>(mappedOrderStatus);
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
        var inclusion = new string[] { };

        var orderStatus = await this.repository.GetAsync(id, inclusion, cancellationToken)
            ?? throw new NotFoundException($"OrderStatus with id = {id} is not found.");

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
