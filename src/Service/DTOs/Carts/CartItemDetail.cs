using Domain.Entities.ProductFolder;
using Domain.Entities.Shopping;

namespace Service.DTOs.Carts;

public class CartItemDetail
{
    public double Quantity { get; set; }
    public decimal Price { get; set; }
    public long ProductItemId { get; set; }
}
