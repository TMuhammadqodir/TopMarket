using Domain.Entities.OrderFolder;
using Domain.Entities.ProductFolder;
using Domain.Entities.UserFolder;

namespace Service.DTOs.OrderLines;

public class OrderLineCreationDto
{
    public long Quantity { get; set; }
    public decimal Price { get; set; }
    public long ProductItemId { get; set; }
    public long OrderId { get; set; }
}
