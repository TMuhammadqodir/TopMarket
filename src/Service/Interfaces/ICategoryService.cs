using Service.DTOs.Categories;

namespace Service.Interfaces;

public interface ICategoryService
{
    Task<CategoryResultDto> CreateAsync(CategoryCreationDto dto, CancellationToken cancellationToken = default);
    Task<CategoryResultDto> ModifyAsync(CategoryUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> RemoveAsync(long id, bool destroy = false, CancellationToken cancellationToken = default);
    Task<CategoryResultDto> RetrieveAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CategoryResultDto>> RetrieveAllAsync(CancellationToken cancellationToken = default);
}
