using Domain.Configuration;
using Service.DTOs.Orders;

namespace Service.Interfaces;

public interface IOrderService
{
    Task<OrderResultDto> CreateAsync(OrderCreationDto dto, CancellationToken cancellationToken = default);
    Task<OrderResultDto> ModifyAsync(OrderUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> RemoveAsync(long id, bool destroy = false, CancellationToken cancellationToken = default);
    Task<OrderResultDto> RetrieveByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<OrderResultDto>> RetrieveAllAsync(PaginationParams? paginationParams = null, CancellationToken cancellationToken = default);
}
