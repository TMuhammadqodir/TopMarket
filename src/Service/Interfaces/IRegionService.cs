using Domain.Configuration;
using Service.DTOs.Regions;

namespace Service.Interfaces;

public interface IRegionService
{
    Task<bool> SetAsync(CancellationToken cancellationToken = default);
    Task<RegionResultDto> RetrieveByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<RegionResultDto>> RetrieveAllAsync(PaginationParams @params, CancellationToken cancellationToken = default);
}
