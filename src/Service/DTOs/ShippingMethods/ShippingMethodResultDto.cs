using Service.DTOs.Orders;

namespace Service.DTOs.ShippingMethods;

public class ShippingMethodResultDto
{
    public long Id { get; set; }
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public ICollection<OrderResultDto> Orders { get; set; }
}