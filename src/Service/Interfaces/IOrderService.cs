using Domain.Configuration;
using Service.DTOs.Orders;

namespace Service.Interfaces;

public interface IOrderService
{
    Task<OrderResultDto> CreateAsync(OrderCreationDto dto, CancellationToken cancellationToken = default);
    Task<OrderResultDto> UpdateAsync(OrderUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, bool destroy = false, CancellationToken cancellationToken = default);
    Task<OrderResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<OrderResultDto>> GetAllAsync(PaginationParams? paginationParams = null, CancellationToken cancellationToken = default);
}
