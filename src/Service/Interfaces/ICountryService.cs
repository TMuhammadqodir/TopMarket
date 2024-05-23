using Domain.Configuration;
using Service.DTOs.Countries;

namespace Service.Interfaces;

public interface ICountryService
{
    Task<bool> SetAsync(CancellationToken cancellationToken = default);
    Task<CountryResultDto> RetrieveByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CountryResultDto>> RetrieveAllAsync(PaginationParams @params, CancellationToken cancellationToken = default);
}
