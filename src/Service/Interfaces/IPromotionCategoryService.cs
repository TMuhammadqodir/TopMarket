using Service.DTOs.PromotionCategories;

namespace Service.Interfaces;

public interface IPromotionCategoryService
{
    Task<PromotionCategoryResultDto> CreateAsync(PromotionCategoryCreationDto dto, CancellationToken cancellationToken = default);
    Task<PromotionCategoryResultDto> ModifyAsync(PromotionCategoryUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> RemoveAsync(long id, bool destroy = false, CancellationToken cancellationToken = default);
    Task<PromotionCategoryResultDto> RetrieveAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<PromotionCategoryResultDto>> RetrieveAllAsync(CancellationToken cancellationToken = default);
}
