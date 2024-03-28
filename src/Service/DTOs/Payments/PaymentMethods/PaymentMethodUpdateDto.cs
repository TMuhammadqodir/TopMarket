namespace Service.DTOs.Payments.PaymentMethods;

public class PaymentMethodUpdateDto
{
    public long Id { get; set; }
    public string Provider { get; set; }
    public string AccountNumber { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool IsDefault { get; set; }
}
