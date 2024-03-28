using Domain.Entities.ProductFolder;
using Service.DTOs.Variations;

namespace Service.Interfaces;

public interface IVariationService
{
    Task<VariationResultDto> CreateAsync(VariationCreationDto dto);
    Task<VariationResultDto> UpdateAsync(VariationUpdateDto dto);
    Task<bool> DeleteAsync(long id);
    Task<VariationResultDto> GetByIdAsync(long id);
    Task<IEnumerable<VariationResultDto>> GetAllAsync();
    Task<IEnumerable<VariationFeatureResultDto>> GetFeaturesOfProduct(long categoryId, long productItemId);
}
