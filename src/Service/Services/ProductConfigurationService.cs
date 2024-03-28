using AutoMapper;
using Data.IRepositories;
using Domain.Entities.ProductFolder;
using Microsoft.EntityFrameworkCore;
using Service.DTOs.ProductConfigurations;
using Service.Exceptions;
using Service.Interfaces;

namespace Service.Services;

public class ProductConfigurationService : IProductConfigurationService
{
    private readonly IMapper mapper;
    private readonly IRepository<ProductConfiguration> repository;
    private readonly IRepository<ProductItem> productItemRepository;
    private readonly IRepository<VariationOption> variationOptionRepository;
    public ProductConfigurationService(
        IMapper mapper, 
        IRepository<ProductConfiguration> repository,
        IRepository<ProductItem> productItemRepository,
        IRepository<VariationOption> variationOptionRepository)
    {
        this.mapper = mapper;
        this.repository = repository;
        this.productItemRepository = productItemRepository;
        this.variationOptionRepository = variationOptionRepository;
    }

    public async Task<ProductConfigurationResultDto> CreateAsync(ProductConfigurationCreationDto dto)
    {
        var existProductItem = await this.productItemRepository.GetAsync(c => c.Id.Equals(dto.ProductItemId))
            ?? throw new NotFoundException($"This productItem was not found with {dto.ProductItemId}");

        var existVariationOption = await this.variationOptionRepository.GetAsync(c => c.Id.Equals(dto.VariationOptionId))
            ?? throw new NotFoundException($"This variationOption was not found with {dto.VariationOptionId}");

        var mappedProductConfiguration = this.mapper.Map<ProductConfiguration>(dto);

        await this.repository.AddAsync(mappedProductConfiguration);
        await this.repository.SaveAsync();

        return this.mapper.Map<ProductConfigurationResultDto>(mappedProductConfiguration);
    }

    public async Task<ProductConfigurationResultDto> UpdateAsync(ProductConfigurationUpdateDto dto)
    {
        var existProductConfiguration = await this.repository.GetAsync(c => c.Id.Equals(dto.Id), includes: new[] { "ProductItem", "VariationOption" })
            ?? throw new NotFoundException($"This productConfiguration was not found with {dto.Id}");

        var existProductItem = await this.productItemRepository.GetAsync(c => c.Id.Equals(dto.ProductItemId))
            ?? throw new NotFoundException($"This productItem was not found with {dto.ProductItemId}");

        var existVariationOption = await this.variationOptionRepository.GetAsync(c => c.Id.Equals(dto.VariationOptionId))
            ?? throw new NotFoundException($"This variationOption was not found with {dto.VariationOptionId}");

        var mappedProductConfiguration = this.mapper.Map(dto, existProductConfiguration);

        this.repository.Update(mappedProductConfiguration);
        await this.repository.SaveAsync();

        return this.mapper.Map<ProductConfigurationResultDto>(mappedProductConfiguration);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var existProductConfiguration = await this.repository.GetAsync(c => c.Id.Equals(id))
            ?? throw new NotFoundException($"This productConfiguration was not found with {id}");

        this.repository.Delete(existProductConfiguration);
        await this.repository.SaveAsync();

        return true;
    }

    public async Task<ProductConfigurationResultDto> GetByIdAsync(long id)
    {
        var existProductConfiguration = await this.repository.GetAsync(c => c.Id.Equals(id), includes: new[] { "ProductItem", "VariationOption" })
            ?? throw new NotFoundException($"This productConfiguration was not found with {id}");

        return this.mapper.Map<ProductConfigurationResultDto>(existProductConfiguration);
    }

    public async Task<IEnumerable<ProductConfigurationResultDto>> GetAllAsync()
    {
        var productConfigurations = await this.repository.GetAll(includes: new[] { "ProductItem", "VariationOption" }).ToListAsync();

        return this.mapper.Map<IEnumerable<ProductConfigurationResultDto>>(productConfigurations);
    }

    public async Task<IEnumerable<ProductConfigurationResultDto>> GetByProductItemIdAsync(long productItemId)
    {
        var productConfigurations = this.repository.GetAll(p=> p.ProductItemId.Equals(productItemId), includes: new[] { "ProductItem", "VariationOption" });

        return this.mapper.Map<IEnumerable<ProductConfigurationResultDto>>(productConfigurations);
    }
}
