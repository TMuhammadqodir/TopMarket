using Domain.Entities.OrderFolder;

namespace Service.DTOs.ShippingMethods;

public class ShippingMethodCreationDto
{
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
}

