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
    private readonly IRepository<ShippingMethod> shippingMethodRepository;
    public ShippingMethodService(IMapper mapper, IRepository<ShippingMethod> shippingMethodRepository)
    {
        this.mapper = mapper;
        this.shippingMethodRepository = shippingMethodRepository;
    }

    public async Task<ShippingMethodResultDto> CreateAsync(ShippingMethodCreationDto dto, CancellationToken cancellationToken = default)
    {
        var shippingMethod = await this.shippingMethodRepository.GetAsync(sr => sr.Name.ToLower().Equals(dto.Name.ToLower()));
        if (shippingMethod is not null)
            throw new AlreadyExistException($"This shippingMethod already exist with {dto.Name}");

        var mappedShippingMethod = this.mapper.Map<ShippingMethod>(dto);

        await this.shippingMethodRepository.AddAsync(mappedShippingMethod,cancellationToken);
        await this.shippingMethodRepository.SaveAsync(cancellationToken);

        return this.mapper.Map<ShippingMethodResultDto>(mappedShippingMethod);
    }

    public async Task<ShippingMethodResultDto> UpdateAsync(ShippingMethodUpdateDto dto,CancellationToken cancellationToken=default)
    {
        var shippingMethod = await this.shippingMethodRepository.GetAsync(sr => sr.Id.Equals(dto.Id), includes: new[] { "Orders" })
            ?? throw new NotFoundException($"This shippingMethod was not found with {dto.Id}");

        if (!shippingMethod.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase))
        {
            var shippingMethodName = await this.shippingMethodRepository.GetAsync(sr => sr.Name.ToLower().Equals(dto.Name.ToLower()));
            if (shippingMethodName is not null)
                throw new AlreadyExistException($"This shippingMethod already exist with {dto.Name}");
        }

        var mappedShippingMethod = this.mapper.Map(dto, shippingMethod);

        this.shippingMethodRepository.Update(mappedShippingMethod);
        await this.shippingMethodRepository.SaveAsync(cancellationToken);

        return this.mapper.Map<ShippingMethodResultDto>(mappedShippingMethod);
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var shippingMethod = await this.shippingMethodRepository.GetAsync(sr => sr.Id.Equals(id))
            ?? throw new NotFoundException($"This shippingMethod was not found with {id}");

        this.shippingMethodRepository.Delete(shippingMethod);
        await this.shippingMethodRepository.SaveAsync(cancellationToken);

        return true;
    }

    public async Task<ShippingMethodResultDto> GetByIdAsync(long id,CancellationToken cancellationToken=default)
    {
        var shippingMethod = await this.shippingMethodRepository.GetAsync(sr => sr.Id.Equals(id), includes: new[] { "Orders" })
            ?? throw new NotFoundException($"This shippingMethod was not found with {id}");

        return this.mapper.Map<ShippingMethodResultDto>(shippingMethod);
    }

    public async Task<IEnumerable<ShippingMethodResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categories = await this.shippingMethodRepository.GetAll(includes: new[] { "Orders" }).ToListAsync(cancellationToken:cancellationToken);

        return this.mapper.Map<IEnumerable<ShippingMethodResultDto>>(categories);
    }
}
