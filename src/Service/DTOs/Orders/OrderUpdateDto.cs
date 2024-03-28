namespace Service.DTOs.Orders;

public class OrderUpdateDto
{
    public long Id { get; set; }
    public DateTime Date { get; set; }
    public decimal Total { get; set; }
    public long UserId { get; set; }
    public long PaymentMethodId { get; set; }
    public long AddressId { get; set; }
    public long ShippingMethodId { get; set; }
    public long StatusId { get; set; }
}
