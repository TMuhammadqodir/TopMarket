using Domain.Configuration;
using Domain.Entities.ProductFolder;
using Service.DTOs.Attachments;
using Service.DTOs.Products;
using System.Linq.Expressions;

namespace Service.Interfaces;

public interface IProductService
{
    Task<ProductResultDto> RetrieveAsync(Expression<Func<Product, bool>> expression);
    Task<ProductResultDto> RetrieveAsync(long id);
    Task<IEnumerable<ProductResultDto>> RetrieveAllAsync(PaginationParams? paginationParams = null);
    Task<IEnumerable<ProductResultDto>> RetrieveByCategoryIdAsync(long categoryId);
    Task<ProductResultDto> CreateAsync(ProductCreationDto dto);
    Task<ProductResultDto> ModifyAsync(ProductUpdateDto dto);

    /// <summary>
    /// By default, method equals 'IsDeleted' property of the entity to 'true'.
    /// By giving 'true' to the 'destroy' parameter, the entity will be 
    /// totally removed from the database.
    /// </summary>
    /// <returns></returns>
    Task<bool> RemoveAsync(long id, bool destroy = false);
    Task<ProductResultDto> ImageUploadAsync(long productId, AttachmentCreationDto dto);
}
