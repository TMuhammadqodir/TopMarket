using Service.DTOs.Categories;
using Service.DTOs.ShippingMethods;

namespace Service.Interfaces;

public interface IShippingMethodService
{
    Task<ShippingMethodResultDto> CreateAsync(ShippingMethodCreationDto dto,CancellationToken cancellationToken=default);
    Task<ShippingMethodResultDto> UpdateAsync(ShippingMethodUpdateDto dto,CancellationToken cancellationToken=default);
    Task<bool> DeleteAsync(long id,CancellationToken cancellationToken=default);
    Task<ShippingMethodResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ShippingMethodResultDto>> GetAllAsync(CancellationToken cancellationToken=default);
}
