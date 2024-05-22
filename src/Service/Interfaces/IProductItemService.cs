using Service.DTOs.Attachments;
using Service.DTOs.ProductItems;

namespace Service.Interfaces;

public interface IProductItemService
{
    Task<ProductItemResultDto> CreateAsync(ProductItemCreationDto dto, CancellationToken cancellationToken = default);
    Task<ProductItemResultDto> ModifyAsync(ProductItemUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> RemoveAsync(long id, bool destroy = false, CancellationToken cancellationToken = default);
    Task<ProductItemResultDto> RetrieveByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductItemResultDto>> RetrieveAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductItemResultDto>> RetrieveByProductIdAsync(long productId, CancellationToken cancellationToken = default);
    Task<ProductItemResultDto> AddAsync(ProductItemIncomeDto dto, CancellationToken cancellationToken = default);
    Task<ProductItemResultDto> SubstractAsync(ProductItemIncomeDto dto, CancellationToken cancellationToken = default);
    Task<ProductItemResultDto> UploadImageAsync(long productItemId, AttachmentCreationDto dto, CancellationToken cancellationToken = default);
    Task<bool> RemoveImageAsync(long productItemId, long imageId, CancellationToken cancellationToken = default);
}
