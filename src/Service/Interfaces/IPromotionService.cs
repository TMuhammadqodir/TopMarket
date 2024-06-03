using Service.DTOs.Categories;
using Service.DTOs.Promotions;

namespace Service.Interfaces;

public interface IPromotionService
{
    Task<PromotionResultDto> CreateAsync(PromotionCreationDto dto, CancellationToken cancellationToken = default);
    Task<PromotionResultDto> UpdateAsync(PromotionUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<PromotionResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<PromotionResultDto>> GetAllAsync(CancellationToken cancellationToken = default);
}
