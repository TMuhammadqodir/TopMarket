using AutoMapper;
using Data.IRepositories;
using Domain.Entities.OrderFolder;
using Domain.Entities.ProductFolder;
using Microsoft.EntityFrameworkCore;
using Service.DTOs.Categories;
using Service.DTOs.ShippingMethods;
using Service.Exceptions;
using Service.Interfaces;

namespace Service.Services;

public class ShippingMethodService : IShippingMethodService
{
    private readonly IMapper mapper;
    private readonly IRepository<ShippingMethod> repository;
    public ShippingMethodService(IMapper mapper, IRepository<ShippingMethod> repository)
    {
        this.mapper = mapper;
        this.repository = repository;
    }

    public async Task<ShippingMethodResultDto> CreateAsync(ShippingMethodCreationDto dto)
    {
        var existShippingMethod = await this.repository.GetAsync(c => c.Name.ToLower().Equals(dto.Name.ToLower()));
        if (existShippingMethod is not null)
            throw new AlreadyExistException($"This shippingMethod already exist with {dto.Name}");

        var mappedShippingMethod = this.mapper.Map<ShippingMethod>(dto);

        await this.repository.AddAsync(mappedShippingMethod);
        await this.repository.SaveAsync();

        return this.mapper.Map<ShippingMethodResultDto>(mappedShippingMethod);
    }

    public async Task<ShippingMethodResultDto> UpdateAsync(ShippingMethodUpdateDto dto)
    {
        var existShippingMethod = await this.repository.GetAsync(c => c.Id.Equals(dto.Id), includes: new[] { "Orders" })
            ?? throw new NotFoundException($"This shippingMethod was not found with {dto.Id}");

        if (!existShippingMethod.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase))
        {
            var existShippingMethodName = await this.repository.GetAsync(c => c.Name.ToLower().Equals(dto.Name.ToLower()));
            if (existShippingMethodName is not null)
                throw new AlreadyExistException($"This shippingMethod already exist with {dto.Name}");
        }

        var mappedShippingMethod = this.mapper.Map(dto, existShippingMethod);

        this.repository.Update(mappedShippingMethod);
        await this.repository.SaveAsync();

        return this.mapper.Map<ShippingMethodResultDto>(mappedShippingMethod);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var existShippingMethod = await this.repository.GetAsync(c => c.Id.Equals(id))
            ?? throw new NotFoundException($"This shippingMethod was not found with {id}");

        this.repository.Delete(existShippingMethod);
        await this.repository.SaveAsync();

        return true;
    }

    public async Task<ShippingMethodResultDto> GetByIdAsync(long id)
    {
        var existShippingMethod = await this.repository.GetAsync(c => c.Id.Equals(id), includes: new[] { "Orders" })
            ?? throw new NotFoundException($"This shippingMethod was not found with {id}");

        return this.mapper.Map<ShippingMethodResultDto>(existShippingMethod);
    }

    public async Task<IEnumerable<ShippingMethodResultDto>> GetAllAsync()
    {
        var categories = await this.repository.GetAll(includes: new[] { "Orders" }).ToListAsync();

        return this.mapper.Map<IEnumerable<ShippingMethodResultDto>>(categories);
    }
}
