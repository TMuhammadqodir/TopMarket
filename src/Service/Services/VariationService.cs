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
    private readonly IRepository<Variation> repository;
    private readonly IRepository<Category> categoryRepository;
    private readonly IProductConfigurationService productConfigurationService;
    public VariationService(
        IMapper mapper, 
        IRepository<Variation> repository,
        IRepository<Category> categoryRepository,
        IProductConfigurationService productConfigurationService)
    {
        this.mapper = mapper;
        this.repository = repository;
        this.categoryRepository = categoryRepository;
        this.productConfigurationService = productConfigurationService;
    }

    public async Task<VariationResultDto> CreateAsync(VariationCreationDto dto)
    {
        var existCategory = await this.categoryRepository.GetAsync(c => c.Id.Equals(dto.CategoryId))
            ?? throw new NotFoundException($"This category was not found with {dto.CategoryId}");

        var mappedVariation = this.mapper.Map<Variation>(dto);

        await this.repository.AddAsync(mappedVariation);
        await this.repository.SaveAsync();

        return this.mapper.Map<VariationResultDto>(mappedVariation);
    }

    public async Task<VariationResultDto> UpdateAsync(VariationUpdateDto dto)
    {
        var existVariation = await this.repository.GetAsync(c => c.Id.Equals(dto.Id), includes: new[] { "Category", "VariationOptions" })
            ?? throw new NotFoundException($"This variation was not found with {dto.Id}");

        var existCategory = await this.categoryRepository.GetAsync(c => c.Id.Equals(dto.CategoryId))
            ?? throw new NotFoundException($"This category was not found with {dto.CategoryId}");

        var mappedVariation = this.mapper.Map(dto, existVariation);

        this.repository.Update(mappedVariation);
        await this.repository.SaveAsync();

        return this.mapper.Map<VariationResultDto>(mappedVariation);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var existVariation = await this.repository.GetAsync(c => c.Id.Equals(id))
            ?? throw new NotFoundException($"This variation was not found with {id}");

        this.repository.Delete(existVariation);
        await this.repository.SaveAsync();

        return true;
    }

    public async Task<VariationResultDto> GetByIdAsync(long id)
    {
        var existVariation = await this.repository.GetAsync(c => c.Id.Equals(id), includes: new[] { "Category", "VariationOptions" })
            ?? throw new NotFoundException($"This variation was not found with {id}");

        return this.mapper.Map<VariationResultDto>(existVariation);
    }

    public async Task<IEnumerable<VariationResultDto>> GetAllAsync()
    {
        var variations = await this.repository.GetAll(includes: new[] { "Category", "VariationOptions" }).ToListAsync();

        return this.mapper.Map<IEnumerable<VariationResultDto>>(variations);
    }

    public async Task<IEnumerable<VariationFeatureResultDto>> GetFeaturesOfProduct(long categoryId, long productItemId)
    {
        var variations = this.repository.GetAll(v=> v.CategoryId.Equals(categoryId)).ToList();

        var resultVariations = this.mapper.Map<List<VariationFeatureResultDto>>(variations);

        var variationOptions = (await productConfigurationService.GetByProductItemIdAsync(productItemId)).Select(p=>p.VariationOption).ToList();

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
