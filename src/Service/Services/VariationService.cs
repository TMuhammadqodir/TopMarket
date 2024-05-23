using AutoMapper;
using Data.IRepositories;
using Domain.Entities.ProductFolder;
using Microsoft.EntityFrameworkCore;
using Service.DTOs.VariationOptions;
using Service.DTOs.Variations;
using Service.Exceptions;
using Service.Interfaces;

namespace Service.Services;

public class VariationService : IVariationService
{
    private readonly IMapper mapper;
    private readonly IRepository<Variation> variationRepository;
    private readonly IRepository<Category> categoryRepository;
    private readonly IProductConfigurationService productConfigurationService;
    public VariationService(
        IMapper mapper, 
        IRepository<Variation> variationRepository,
        IRepository<Category> categoryRepository,
        IProductConfigurationService productConfigurationService)
    {
        this.mapper = mapper;
        this.variationRepository = variationRepository;
        this.categoryRepository = categoryRepository;
        this.productConfigurationService = productConfigurationService;
    }

    public async Task<VariationResultDto> CreateAsync(VariationCreationDto dto, CancellationToken cancellationToken = default)
    {
        var existCategory = await this.categoryRepository.GetAsync(cr => cr.Id.Equals(dto.CategoryId),
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"This category was not found with {dto.CategoryId}");

        var mappedVariation = this.mapper.Map<Variation>(dto);

        await this.variationRepository.AddAsync(mappedVariation);
        await this.variationRepository.SaveAsync(cancellationToken);

        return this.mapper.Map<VariationResultDto>(mappedVariation);
    }

    public async Task<VariationResultDto> UpdateAsync(VariationUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var existVariation = await this.variationRepository.GetAsync(vr => vr.Id.Equals(dto.Id), 
            includes: new[] { "Category", "VariationOptions" },
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"This variation was not found with {dto.Id}");

        var existCategory = await this.categoryRepository.GetAsync(cr => cr.Id.Equals(dto.CategoryId), 
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"This category was not found with {dto.CategoryId}");

        var mappedVariation = this.mapper.Map(dto, existVariation);

        this.variationRepository.Update(mappedVariation);
        await this.variationRepository.SaveAsync(cancellationToken);

        return this.mapper.Map<VariationResultDto>(mappedVariation);
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var existVariation = await this.variationRepository.GetAsync(vr => vr.Id.Equals(id),
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"This variation was not found with {id}");

        this.variationRepository.Delete(existVariation);
        await this.variationRepository.SaveAsync(cancellationToken);

        return true;
    }

    public async Task<VariationResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var existVariation = await this.variationRepository.GetAsync(vr =>vr.Id.Equals(id), 
            includes: new[] { "Category", "VariationOptions" },
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"This variation was not found with {id}");

        return this.mapper.Map<VariationResultDto>(existVariation);
    }

    public async Task<IEnumerable<VariationResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var variations = await this.variationRepository.GetAll(includes: new[] { "Category", "VariationOptions" })
            .ToListAsync(cancellationToken);

        return this.mapper.Map<IEnumerable<VariationResultDto>>(variations);
    }

    public async Task<IEnumerable<VariationFeatureResultDto>> GetFeaturesOfProduct(long categoryId, 
        long productItemId,
        CancellationToken cancellationToken = default)
    {
        var variations = await this.variationRepository.GetAll(r=> r.CategoryId.Equals(categoryId))
            .ToListAsync(cancellationToken);

        var resultVariations = this.mapper.Map<List<VariationFeatureResultDto>>(variations);

        var variationOptions = (await productConfigurationService.GetByProductItemIdAsync(productItemId))
            .Select(p=>p.VariationOption)
            .ToList();

        if(variationOptions is not null)
        {
            for(int i=0; i< resultVariations.Count; i++)
            {
                for(int j=0; j< variationOptions.Count; j++)
                {
                    if (resultVariations[i].Id.Equals(variationOptions[j].VariationId))
                    {
                        resultVariations[i].VariationOption = this.mapper.Map<VariationOptionFeatureResult>(variationOptions[j]);
                    }
                }
            }
        }

        return resultVariations.AsEnumerable();
    }
}
