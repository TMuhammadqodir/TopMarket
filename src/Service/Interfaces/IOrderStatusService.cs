using Service.DTOs.OrderStates;

namespace Service.Interfaces;

public interface IOrderStatusService
{
    Task<OrderStatusResultDto> CreateAsync(OrderStatusCreationDto dto, CancellationToken cancellationToken = default);
    Task<OrderStatusResultDto> ModifyAsync(OrderStatusUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> RemoveAsync(long id, bool destroy = false, CancellationToken cancellationToken = default);
    Task<OrderStatusResultDto> RetrieveByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<OrderStatusResultDto>> RetrieveAllAsync(CancellationToken cancellationToken = default);
}
