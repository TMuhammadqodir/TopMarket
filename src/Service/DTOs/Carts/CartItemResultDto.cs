using Service.DTOs.ProductItems;
using Service.DTOs.Users;

namespace Service.DTOs.Carts;

public class CartItemResultDto
{
    public long Id { get; set; }
    public double Quantity { get; set; }
    public decimal Price { get; set; }
    public ProductItemResultDto Product { get; set; }
}
