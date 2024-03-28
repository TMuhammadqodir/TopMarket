using Service.DTOs.Categories;
using Service.DTOs.OrderStates;

namespace Service.Interfaces;

public interface IOrderStatusService
{
    Task<OrderStatusResultDto> CreateAsync(OrderStatusCreationDto dto);
    Task<OrderStatusResultDto> UpdateAsync(OrderStatusUpdateDto dto);
    Task<bool> DeleteAsync(long id);
    Task<OrderStatusResultDto> GetByIdAsync(long id);
    Task<IEnumerable<OrderStatusResultDto>> GetAllAsync();
}
