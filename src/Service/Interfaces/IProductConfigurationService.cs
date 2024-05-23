using Service.DTOs.ProductConfigurations;

namespace Service.Interfaces;

public interface IProductConfigurationService
{
    Task<ProductConfigurationResultDto> CreateAsync(ProductConfigurationCreationDto dto, CancellationToken cancellationToken = default);
    Task<ProductConfigurationResultDto> UpdateAsync(ProductConfigurationUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<ProductConfigurationResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductConfigurationResultDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductConfigurationResultDto>> GetByProductItemIdAsync(long productItemId, CancellationToken cancellationToken = default);
}
