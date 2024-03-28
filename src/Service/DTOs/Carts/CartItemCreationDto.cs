using Domain.Entities.ProductFolder;

namespace Service.DTOs.Carts;

public class CartItemCreationDto
{
    public long CartId { get; set; }

    public double Quantity { get; set; }
    public decimal Price { get; set; }

    public long ProductItemId { get; set; }
    public ProductItem ProductItem { get; set; }
}
