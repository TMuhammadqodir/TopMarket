using AutoMapper;
using Data.IRepositories;
using Domain.Entities.ProductFolder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.DTOs.ProductConfigurations;
using Service.DTOs.VariationOptions;
using Service.Exceptions;
using Service.Interfaces;

namespace Service.Services;

public class VariationOptionService : IVariationOptionService
{
    private readonly ILogger<VariationOptionService> logger;
    private readonly IMapper mapper;
    private readonly IRepository<VariationOption> repository;
    private readonly IRepository<Variation> variationRepository;
    private readonly IProductConfigurationService productConfigurationService;
    private readonly IRepository<ProductConfiguration> productConfigurationRepository;
    public VariationOptionService(
        ILogger<VariationOptionService> logger,
        IMapper mapper,
        IRepository<VariationOption> repository,
        IRepository<Variation> variationRepository,
        IProductConfigurationService productConfigurationService,
        IRepository<ProductConfiguration> productConfigurationRepository)
    {
        this.logger = logger;
        this.mapper = mapper;
        this.repository = repository;
        this.variationRepository = variationRepository;
        this.productConfigurationService = productConfigurationService;
        this.productConfigurationRepository = productConfigurationRepository;
    }

    public async Task<VariationOptionResultDto> CreateAsync(VariationOptionCreationDto dto, CancellationToken cancellationToken = default)
    {
        var variation = await this.variationRepository.GetAsync(dto.VariationId, cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Variation with id = {dto.VariationId} is not found.");

        var mappedVariationOption = this.mapper.Map<VariationOption>(dto);

        await this.repository.AddAsync(mappedVariationOption, cancellationToken);
        await this.repository.SaveAsync(cancellationToken);

        mappedVariationOption.Variation = variation;

        var productConfiguration = new ProductConfigurationCreationDto()
        {
            ProductItemId = dto.ProductItemId,
            VariationOptionId = mappedVariationOption.Id
        };
        await this.productConfigurationService.CreateAsync(productConfiguration, cancellationToken);

        return this.mapper.Map<VariationOptionResultDto>(mappedVariationOption);
    }

    public async Task<VariationOptionResultDto> ModifyAsync(VariationOptionUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var variationOption = await this.repository.GetAsync(dto.Id, includes: new[] { "Variation" }, cancellationToken)
            ?? throw new NotFoundException($"Variation option with id = {dto.Id} is not found.");

        var mappedVariationOption = this.mapper.Map(dto, variationOption);

        this.repository.Update(mappedVariationOption);
        await this.repository.SaveAsync(cancellationToken);

        return this.mapper.Map<VariationOptionResultDto>(mappedVariationOption);
    }

    public async Task<bool> RemoveAsync(long id, bool destroy = false, CancellationToken cancellationToken = default)
    {
        var variationOption = await this.repository.GetAsync(id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Variation option with id = {id} is not found.");

        var productConfiguration = await productConfigurationRepository.GetAsync(id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Product configuration with id = {id} is not found."); ;

        try
        {
            if (destroy) 
                this.repository.Destroy(variationOption);
            else
                this.repository.Delete(variationOption);

            await this.repository.SaveAsync(cancellationToken);
            await productConfigurationService.DeleteAsync(productConfiguration.Id);

            this.logger.LogInformation("Variation option has been successfully {action}", destroy ? "destroyed" : "deleted");
            
            return true;

        }
        catch (Exception ex)
        {
            this.logger.LogError("Variation option has NOT been deleted. See details: {@exception}", ex);
            return false;
        }
    }

    public async Task<VariationOptionResultDto> RetrieveByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var variationOption = await this.repository.GetAsync(id, includes: new[] { "Variation" }, cancellationToken)
            ?? throw new NotFoundException($"Variation option with id = {id} is not found.");

        return this.mapper.Map<VariationOptionResultDto>(variationOption);
    }

    public async Task<IEnumerable<VariationOptionResultDto>> RetrieveAllAsync(CancellationToken cancellationToken = default)
    {
        var variationOptions = await this.repository
                .GetAll(includes: new[] { "Variation" })
                .ToListAsync(cancellationToken);

        return this.mapper.Map<IEnumerable<VariationOptionResultDto>>(variationOptions);
    }
}
