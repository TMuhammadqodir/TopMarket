using Domain.Configuration;
using Service.DTOs.Addresses;

namespace Service.Interfaces;

public interface IAddressService
{
    Task<AddressResultDto> CreateAsync(AddressCreationDto dto, CancellationToken cancellationToken = default);
    Task<AddressResultDto> ModifyAsync(AddressUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> RemoveAsync(long id, bool destroy = false, CancellationToken cancellationToken = default);
    Task<AddressResultDto> RetrieveByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<AddressResultDto>> RetrieveAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<AddressResultDto>> RetrieveAllAsync(PaginationParams @params, CancellationToken cancellationToken = default);
}
