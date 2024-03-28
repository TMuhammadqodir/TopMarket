using Domain.Entities.Addresses;
using Domain.Entities.OrderFolder;
using Domain.Entities.Payment;
using Domain.Entities.UserFolder;
using Service.DTOs.Addresses;
using Service.DTOs.OrderLines;
using Service.DTOs.OrderStates;
using Service.DTOs.Payments.PaymentMethods;
using Service.DTOs.ShippingMethods;
using Service.DTOs.Users;

namespace Service.DTOs.Orders;

public class OrderResultDto
{
    public long Id { get; set; }
    public DateTime Date { get; set; }
    public decimal Total { get; set; }
    public UserResultDto User { get; set; }
    public PaymentMethodResultDto PaymentMethod { get; set; }
    public AddressResultDto Address { get; set; }
    public ShippingMethodResultDto ShippingMethod { get; set; }
    public OrderStatusResultDto Status { get; set; }
    public ICollection<OrderLineResultDto> OrderLines { get; set; }
}