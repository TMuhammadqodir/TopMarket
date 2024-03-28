using Service.DTOs.Users;

namespace Service.DTOs.Carts;

public class CartResultDto
{
    public long Id { get; set; }
    public ICollection<CartItemResultDto> Items { get; set; }
    public decimal TotalPrice { get; set; }
}
