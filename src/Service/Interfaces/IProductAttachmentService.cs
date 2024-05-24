using Service.DTOs.ProductAttachments;
using Service.DTOs.Variations;

namespace Service.Interfaces;

public interface IProductAttachmentService
{
    Task<ProductAttachmentResultDto> CreateAsync(ProductAttachmentCreationDto dto,CancellationToken cancellationToken = default);
    Task<ProductAttachmentResultDto> UpdateAsync(ProductAttachmentUpdateDto dto,CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<ProductAttachmentResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductAttachmentResultDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long productId, long attachmentId,CancellationToken cancellationToken = default);
}
