using Service.DTOs.Attachments;

namespace Service.Interfaces;

public interface IAttachmentService
{
    Task<AttachmentResultDto> UploadImageAsync(AttachmentCreationDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteImageAsync(long id, CancellationToken cancellationToken = default);
}
