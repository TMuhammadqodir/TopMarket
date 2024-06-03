using Domain.Configuration;
using Service.DTOs.Districts;

namespace Service.Interfaces;

public interface IDistrictService
{
    Task<bool> SetAsync(CancellationToken cancellationToken = default);
    Task<DistrictResultDto> RetrieveByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<DistrictResultDto>> RetrieveAllAsync(PaginationParams @params, CancellationToken cancellationToken = default);
}
