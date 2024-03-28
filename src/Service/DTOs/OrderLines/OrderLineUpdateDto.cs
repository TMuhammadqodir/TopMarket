namespace Service.DTOs.OrderLines;

public class OrderLineUpdateDto
{
    public long Id { get; set; }
    public long Quantity { get; set; }
    public decimal Price { get; set; }
    public long OrderId { get; set; }
    public long ProductItemId { get; set; }
}
