using Domain.Entities.Payment;
using Service.DTOs.Payments.PaymentMethods;

namespace Service.DTOs.Payments.PaymentType;

public class PaymentTypeResultDto
{
    public long Id { get; set; }
    public string Value { get; set; }
    public ICollection<PaymentMethodResultDto> PaymentMethods { get; set; }
}


