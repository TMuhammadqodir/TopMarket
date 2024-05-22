using Domain.Configuration;
using Domain.Entities.ProductFolder;
using Service.DTOs.Attachments;
using Service.DTOs.Products;
using System.Linq.Expressions;

namespace Service.Interfaces;

public interface IProductService
{
    Task<ProductResultDto> RetrieveAsync(Expression<Func<Product, bool>> expression, CancellationToken cancellationToken = default);
    Task<ProductResultDto> RetrieveAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductResultDto>> RetrieveAllAsync(PaginationParams? paginationParams = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductResultDto>> RetrieveByCategoryIdAsync(long categoryId, CancellationToken cancellationToken = default);
    Task<ProductResultDto> CreateAsync(ProductCreationDto dto, CancellationToken cancellationToken = default);
    Task<ProductResultDto> ModifyAsync(ProductUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> RemoveAsync(long id, bool destroy = false, CancellationToken cancellationToken = default);
    Task<ProductResultDto> UploadImageAsync(long productId, AttachmentCreationDto dto, CancellationToken cancellationToken = default);
}
