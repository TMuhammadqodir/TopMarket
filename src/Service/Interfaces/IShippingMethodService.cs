using Service.DTOs.Categories;
using Service.DTOs.ShippingMethods;

namespace Service.Interfaces;

public interface IShippingMethodService
{
    Task<ShippingMethodResultDto> CreateAsync(ShippingMethodCreationDto dto);
    Task<ShippingMethodResultDto> UpdateAsync(ShippingMethodUpdateDto dto);
    Task<bool> DeleteAsync(long id);
    Task<ShippingMethodResultDto> GetByIdAsync(long id);
    Task<IEnumerable<ShippingMethodResultDto>> GetAllAsync();
}
