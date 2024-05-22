using Service.DTOs.VariationOptions;

namespace Service.Interfaces;

public interface IVariationOptionService
{
    Task<VariationOptionResultDto> CreateAsync(VariationOptionCreationDto dto, CancellationToken cancellationToken = default);
    Task<VariationOptionResultDto> ModifyAsync(VariationOptionUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> RemoveAsync(long id, bool destroy = false, CancellationToken cancellationToken = default);
    Task<VariationOptionResultDto> RetrieveByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<VariationOptionResultDto>> RetrieveAllAsync(CancellationToken cancellationToken = default);
}
