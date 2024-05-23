using Service.DTOs.ProductItemAttachments;

namespace Service.Interfaces;

public interface IProductItemAttachmentService
{
    Task<ProductItemAttachmentResultDto> CreateAsync(ProductItemAttachmentCreationDto dto, CancellationToken cancellationToken = default);
    Task<ProductItemAttachmentResultDto> UpdateAsync(ProductItemAttachmentUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<ProductItemAttachmentResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductItemAttachmentResultDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long productItemId, long attachmentId, CancellationToken cancellationToken = default);
}
