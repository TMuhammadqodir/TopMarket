using AutoMapper;
using Data.IRepositories;
using Domain.Configuration;
using Domain.Entities.OrderFolder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.DTOs.Orders;
using Service.Exceptions;
using Service.Extensions;
using Service.Interfaces;

namespace Service.Services;

public class OrderService : IOrderService
{
    private readonly ILogger<OrderService> logger;
    private readonly IMapper mapper;
    private readonly IRepository<Order> repository;

    public OrderService(
        ILogger<OrderService> logger,
        IMapper mapper,
        IRepository<Order> repository)
    {
        this.logger = logger;
        this.mapper = mapper;
        this.repository = repository;
    }

    public async Task<OrderResultDto> CreateAsync(OrderCreationDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var newOrder = this.mapper.Map<Order>(dto);
            await this.repository.AddAsync(newOrder, cancellationToken);
            await this.repository.SaveAsync(cancellationToken);

            this.logger.LogInformation("New order has been created successfully. Order id # {OrderId}", newOrder.Id);

            return this.mapper.Map<OrderResultDto>(newOrder);
        }
        catch (Exception ex)
        {
            this.logger.LogError("Order has NOT been created. See details: {@exception}", ex);
            throw new CustomException(StatusCodes.Status409Conflict, ex.Message);
        }
    }
    public async Task<OrderResultDto> ModifyAsync(OrderUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var order = await this.repository.GetAsync(dto.Id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Order with id = {dto.Id} is not found.");

        this.mapper.Map(dto, order);
        this.repository.Update(order);
        
        await this.repository.SaveAsync(cancellationToken);
        this.logger.LogInformation("Order has been successfully updated. Order id # {id}", order.Id);

        return this.mapper.Map<OrderResultDto>(order);
    }

    public async Task<bool> RemoveAsync(long id, bool destroy = false, CancellationToken cancellationToken = default)
    {
        var order = await this.repository.GetAsync(id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Order with id = {id} is not found.");
        try
        {
            if (destroy)
                this.repository.Destroy(order);
            else
                this.repository.Delete(order);

            await this.repository.SaveAsync(cancellationToken);
            this.logger.LogInformation("Order has been successfully {action}.", destroy ? "destroyed" : "deleted");

            return true;
        }
        catch (Exception ex)
        {
            this.logger.LogError("Order has NOT been deleted. See details {@exception}", ex);
            return false;
        }
    }

    public async Task<IEnumerable<OrderResultDto>> RetrieveAllAsync(PaginationParams? paginationParams = null, CancellationToken cancellationToken = default)
    {
        var inclusion = new string[] { };

        var query = this.repository.GetAll(includes: inclusion);

        if (paginationParams is not null)
            query = query.ToPaginate(paginationParams);

        var orders = await query.ToListAsync(cancellationToken);

        return this.mapper.Map<IEnumerable<OrderResultDto>>(orders);
    }

    public async Task<OrderResultDto> RetrieveByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var inclusion = new string[] { };

        var order = await this.repository.GetAsync(id, inclusion, cancellationToken)
            ?? throw new NotFoundException($"Order with id = {id} is not found.");

        return this.mapper.Map<OrderResultDto>(order);
    }
}
