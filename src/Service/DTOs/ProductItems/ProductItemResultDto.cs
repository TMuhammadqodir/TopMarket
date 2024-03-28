using Domain.Entities.OrderFolder;
using Domain.Entities.ProductFolder;
using Domain.Entities.Shopping;
using Service.DTOs.ProductConfigurations;
using Service.DTOs.ProductItemAttachments;
using Service.DTOs.Products;
using Service.DTOs.Variations;

namespace Service.DTOs.ProductItems;

public class ProductItemResultDto { 
    public long Id { get; set; }
    public string SKU { get; set; }
    public decimal Price { get; set; }
    public decimal QuantityInStock { get; set; }
    public ProductResultDto Product { get; set; }
    public ICollection<ProductItemAttachmentResultDto> ProductItemAttachments { get; set; }
    public ICollection<VariationFeatureResultDto> Variations { get; set; }
}