using Domain.Commons;

namespace Domain.Entities.ProductFolder;

public class ProductConfiguration : Auditable
{
    public long ProductItemId { get; set; }
    public ProductItem ProductItem { get; set; }

    public long VariationOptionId { get; set; }
    public VariationOption VariationOption { get; set; }
}
