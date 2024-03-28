using AutoMapper;
using Data.IRepositories;
using Domain.Entities.OrderFolder;
using Domain.Entities.ProductFolder;
using Microsoft.EntityFrameworkCore;
using Service.DTOs.Categories;
using Service.DTOs.OrderStates;
using Service.Exceptions;
using Service.Interfaces;

namespace Service.Services;

public class OrderStatusService : IOrderStatusService
{
    private readonly IMapper mapper;
    private readonly IRepository<OrderStatus> repository;
    public OrderStatusService(IMapper mapper, IRepository<OrderStatus> repository)
    {
        this.mapper = mapper;
        this.repository = repository;
    }

    public async Task<OrderStatusResultDto> CreateAsync(OrderStatusCreationDto dto)
    {
        var existOrderStatus = await this.repository.GetAsync(c => c.Name.ToLower().Equals(dto.Name.ToLower()));
        if (existOrderStatus is not null)
            throw new AlreadyExistException($"This orderStatus already exist with {dto.Name}");

        var mappedOrderStatus = this.mapper.Map<OrderStatus>(dto);

        await this.repository.AddAsync(mappedOrderStatus);
        await this.repository.SaveAsync();

        return this.mapper.Map<OrderStatusResultDto>(mappedOrderStatus);
    }

    public async Task<OrderStatusResultDto> UpdateAsync(OrderStatusUpdateDto dto)
    {
        var existOrderStatus = await this.repository.GetAsync(c => c.Id.Equals(dto.Id))
            ?? throw new NotFoundException($"This orderStatus was not found with {dto.Id}");

        if (!existOrderStatus.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase))
        {
            var existOrderStatusName = await this.repository.GetAsync(c => c.Name.ToLower().Equals(dto.Name.ToLower()));
            if (existOrderStatusName is not null)
                throw new AlreadyExistException($"This orderStatus already exist with {dto.Name}");
        }

        var mappedOrderStatus = this.mapper.Map(dto, existOrderStatus);

        this.repository.Update(mappedOrderStatus);
        await this.repository.SaveAsync();

        return this.mapper.Map<OrderStatusResultDto>(mappedOrderStatus);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var existOrderStatus = await this.repository.GetAsync(c => c.Id.Equals(id))
            ?? throw new NotFoundException($"This orderStatus was not found with {id}");

        this.repository.Delete(existOrderStatus);
        await this.repository.SaveAsync();

        return true;
    }

    public async Task<OrderStatusResultDto> GetByIdAsync(long id)
    {
        var existOrderStatus = await this.repository.GetAsync(c => c.Id.Equals(id))
            ?? throw new NotFoundException($"This orderStatus was not found with {id}");

        return this.mapper.Map<OrderStatusResultDto>(existOrderStatus);
    }

    public async Task<IEnumerable<OrderStatusResultDto>> GetAllAsync()
    {
        var categories = await this.repository.GetAll().ToListAsync();

        return this.mapper.Map<IEnumerable<OrderStatusResultDto>>(categories);
    }
}
