using Service.DTOs.ProductConfigurations;

namespace Service.Interfaces;

public interface IProductConfigurationService
{
    Task<ProductConfigurationResultDto> CreateAsync(ProductConfigurationCreationDto dto);
    Task<ProductConfigurationResultDto> UpdateAsync(ProductConfigurationUpdateDto dto);
    Task<bool> DeleteAsync(long id);
    Task<ProductConfigurationResultDto> GetByIdAsync(long id);
    Task<IEnumerable<ProductConfigurationResultDto>> GetAllAsync();
    Task<IEnumerable<ProductConfigurationResultDto>> GetByProductItemIdAsync(long productItemId);
}
