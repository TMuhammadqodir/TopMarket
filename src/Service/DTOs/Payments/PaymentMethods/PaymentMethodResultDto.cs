using Service.DTOs.Payments.PaymentType;
using Service.DTOs.Users;

namespace Service.DTOs.Payments.PaymentMethods;

public class PaymentMethodResultDto
{
    public long Id { get; set; }
    public UserResultDto User { get; set; }
    public PaymentTypeResultDto PaymentType { get; set; }
    public string Provider { get; set; }
    public string AccountNumber { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool IsDefault { get; set; }
}
