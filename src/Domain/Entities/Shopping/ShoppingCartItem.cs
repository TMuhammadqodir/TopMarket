using Domain.Commons;
using Domain.Entities.ProductFolder;

namespace Domain.Entities.Shopping;

public class ShoppingCartItem : Auditable
{
    public long CartId { get; set; }
    public ShoppingCart Cart { get; set; }

    public long ProductItemId { get; set; }
    public ProductItem ProductItem { get; set; }
    
    public double Quantity { get; set; }
    public decimal Price { get; set; }
}
