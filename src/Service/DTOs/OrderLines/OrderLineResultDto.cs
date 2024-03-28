using Service.DTOs.Orders;
using Service.DTOs.ProductItems;
using Service.DTOs.UserRewiev;

namespace Service.DTOs.OrderLines;

public class OrderLineResultDto
{
    public long Id { get; set; }
    public long Quantity { get; set; }
    public decimal Price { get; set; }
    public ProductItemResultDto ProductItem { get; set; }
    public OrderResultDto Order { get; set; }
    public ICollection<UserRewievResultDto> UserReviews { get; set; }
}