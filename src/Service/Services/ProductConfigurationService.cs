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
    private readonly IRepository<ProductConfiguration> productConfigurationRepository;
    private readonly IRepository<ProductItem> productItemRepository;
    private readonly IRepository<VariationOption> variationOptionRepository;
    public ProductConfigurationService(
        IMapper mapper, 
        IRepository<ProductConfiguration> productConfigurationRepository,
        IRepository<ProductItem> productItemRepository,
        IRepository<VariationOption> variationOptionRepository)
    {
        this.mapper = mapper;
        this.productConfigurationRepository = productConfigurationRepository;
        this.productItemRepository = productItemRepository;
        this.variationOptionRepository = variationOptionRepository;
    }

    public async Task<ProductConfigurationResultDto> CreateAsync(ProductConfigurationCreationDto dto, 
        CancellationToken cancellationToken = default)
    {
        var existProductItem = await this.productItemRepository.GetAsync(pir => pir.Id.Equals(dto.ProductItemId),
            cancellationToken: cancellationToken) 
            ?? throw new NotFoundException($"This productItem was not found with {dto.ProductItemId}");

        var existVariationOption = await this.variationOptionRepository.GetAsync(vor => vor.Id.Equals(dto.VariationOptionId), 
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"This variationOption was not found with {dto.VariationOptionId}");

        var mappedProductConfiguration = this.mapper.Map<ProductConfiguration>(dto);

        await this.productConfigurationRepository.AddAsync(mappedProductConfiguration);
        await this.productConfigurationRepository.SaveAsync(cancellationToken);

        return this.mapper.Map<ProductConfigurationResultDto>(mappedProductConfiguration);
    }

    public async Task<ProductConfigurationResultDto> UpdateAsync(ProductConfigurationUpdateDto dto, 
        CancellationToken cancellationToken = default)
    {
        var existProductConfiguration = await this.productConfigurationRepository.GetAsync(pcr => pcr.Id.Equals(dto.Id), 
            includes: new[] { "ProductItem", "VariationOption" }, 
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"This productConfiguration was not found with {dto.Id}");

        var existProductItem = await this.productItemRepository.GetAsync(pir => pir.Id.Equals(dto.ProductItemId),
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"This productItem was not found with {dto.ProductItemId}");

        var existVariationOption = await this.variationOptionRepository.GetAsync(vor => vor.Id.Equals(dto.VariationOptionId),
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"This variationOption was not found with {dto.VariationOptionId}");

        var mappedProductConfiguration = this.mapper.Map(dto, existProductConfiguration);

        this.productConfigurationRepository.Update(mappedProductConfiguration);
        await this.productConfigurationRepository.SaveAsync(cancellationToken);

        return this.mapper.Map<ProductConfigurationResultDto>(mappedProductConfiguration);
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var existProductConfiguration = await this.productConfigurationRepository.GetAsync(pcr => pcr.Id.Equals(id),
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"This productConfiguration was not found with {id}");

        this.productConfigurationRepository.Delete(existProductConfiguration);
        await this.productConfigurationRepository.SaveAsync(cancellationToken);

        return true;
    }

    public async Task<ProductConfigurationResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var existProductConfiguration = await this.productConfigurationRepository.GetAsync(pcr => pcr.Id.Equals(id), 
            includes: new[] { "ProductItem", "VariationOption" },
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"This productConfiguration was not found with {id}");

        return this.mapper.Map<ProductConfigurationResultDto>(existProductConfiguration);
    }

    public async Task<IEnumerable<ProductConfigurationResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var productConfigurations = await this.productConfigurationRepository.GetAll(includes: new[] { "ProductItem", "VariationOption" })
            .ToListAsync(cancellationToken);

        return this.mapper.Map<IEnumerable<ProductConfigurationResultDto>>(productConfigurations);
    }

    public async Task<IEnumerable<ProductConfigurationResultDto>> GetByProductItemIdAsync(long productItemId, CancellationToken cancellationToken = default)
    {
        var productConfigurations = await this.productConfigurationRepository.GetAll(pcr=> pcr.ProductItemId.Equals(productItemId), 
            includes: new[] { "ProductItem", "VariationOption" })
            .ToListAsync(cancellationToken);

        return this.mapper.Map<IEnumerable<ProductConfigurationResultDto>>(productConfigurations);
    }
}
