using Service.DTOs.ProductAttachments;
using Service.DTOs.Variations;

namespace Service.Interfaces;

public interface IProductAttachmentService
{
    Task<ProductAttachmentResultDto> CreateAsync(ProductAttachmentCreationDto dto,CancellationToken cancellationToken);
    Task<ProductAttachmentResultDto> UpdateAsync(ProductAttachmentUpdateDto dto,CancellationToken cancellationToken);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken);
    Task<ProductAttachmentResultDto> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<IEnumerable<ProductAttachmentResultDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<bool> DeleteAsync(long productId, long attachmentId,CancellationToken cancellationToken);
}
