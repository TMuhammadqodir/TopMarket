using AutoMapper;
using Data.IRepositories;
using Domain.Entities.ProductFolder;
using Microsoft.EntityFrameworkCore;
using Service.DTOs.ProductConfigurations;
using Service.DTOs.VariationOptions;
using Service.Exceptions;
using Service.Interfaces;

namespace Service.Services;

public class VariationOptionService : IVariationOptionService
{
    private readonly IMapper mapper;
    private readonly IRepository<VariationOption> repository;
    private readonly IRepository<Variation> variationRepository;
    private readonly IRepository<ProductItem> productItemRepository;
    private readonly IProductConfigurationService productConfigurationService;
    private readonly IRepository<ProductConfiguration> productConfigurationRepository;
    public VariationOptionService(
        IMapper mapper,
        IRepository<VariationOption> repository,
        IRepository<Variation> variationRepository,
        IRepository<ProductItem> productItemRepository,
        IProductConfigurationService productConfigurationService,
        IRepository<ProductConfiguration> productConfigurationRepository)
    {
        this.mapper = mapper;
        this.repository = repository;
        this.variationRepository = variationRepository;
        this.productItemRepository = productItemRepository;
        this.productConfigurationService = productConfigurationService;
        this.productConfigurationRepository = productConfigurationRepository;
    }

    public async Task<VariationOptionResultDto> CreateAsync(VariationOptionCreationDto dto)
    {
        var existVariation = await this.variationRepository.GetAsync(c => c.Id.Equals(dto.VariationId))
            ?? throw new NotFoundException($"This variation was not found with {dto.VariationId}");

        var existProductItem = await this.productItemRepository.GetAsync(c => c.Id.Equals(dto.ProductItemId))
            ?? throw new NotFoundException($"This productItem was not found with {dto.ProductItemId}");

        var mappedVariationOption = this.mapper.Map<VariationOption>(dto);

        await this.repository.AddAsync(mappedVariationOption);
        await this.repository.SaveAsync();

        mappedVariationOption.Variation = existVariation;

        var productConfiguration = new ProductConfigurationCreationDto() {
            ProductItemId = dto.ProductItemId,
            VariationOptionId = mappedVariationOption.Id
        };
        await this.productConfigurationService.CreateAsync(productConfiguration);

        return this.mapper.Map<VariationOptionResultDto>(mappedVariationOption);
    }

    public async Task<VariationOptionResultDto> UpdateAsync(VariationOptionUpdateDto dto)
    {
        var existVariationOption = await this.repository.GetAsync(c => c.Id.Equals(dto.Id), includes: new[] { "Variation" })
            ?? throw new NotFoundException($"This variationOption was not found with {dto.Id}");

        var mappedVariationOption = this.mapper.Map(dto, existVariationOption);

        this.repository.Update(mappedVariationOption);
        await this.repository.SaveAsync();

        return this.mapper.Map<VariationOptionResultDto>(mappedVariationOption);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var existVariationOption = await this.repository.GetAsync(c => c.Id.Equals(id))
            ?? throw new NotFoundException($"This variationOption was not found with {id}");

        var productConfiguration = await productConfigurationRepository.GetAsync(p => p.VariationOptionId.Equals(id));

        this.repository.Delete(existVariationOption);
        await this.repository.SaveAsync();

        await productConfigurationService.DeleteAsync(productConfiguration.Id);
        return true;
    }

    public async Task<VariationOptionResultDto> GetByIdAsync(long id)
    {
        var existVariationOption = await this.repository.GetAsync(c => c.Id.Equals(id), includes: new[] { "Variation" })
            ?? throw new NotFoundException($"This variationOption was not found with {id}");

        return this.mapper.Map<VariationOptionResultDto>(existVariationOption);
    }

    public async Task<IEnumerable<VariationOptionResultDto>> GetAllAsync()
    {
        var variationOptions = await this.repository.GetAll(includes: new[] { "Variation" }).ToListAsync();

        return this.mapper.Map<IEnumerable<VariationOptionResultDto>>(variationOptions);
    }
}
