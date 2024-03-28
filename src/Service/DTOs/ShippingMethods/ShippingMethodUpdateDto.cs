namespace Service.DTOs.ShippingMethods;

public class ShippingMethodUpdateDto
{
    public long Id { get; set; }
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
}

