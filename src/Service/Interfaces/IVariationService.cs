using Domain.Entities.ProductFolder;
using Service.DTOs.Variations;

namespace Service.Interfaces;

public interface IVariationService
{
    Task<VariationResultDto> CreateAsync(VariationCreationDto dto, CancellationToken cancellationToken = default);
    Task<VariationResultDto> UpdateAsync(VariationUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<VariationResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<VariationResultDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<VariationFeatureResultDto>> GetFeaturesOfProduct(long categoryId, long productItemId, 
        CancellationToken cancellationToken = default);
}
