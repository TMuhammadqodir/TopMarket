using Domain.Commons;
using Domain.Entities.OrderFolder;
using Domain.Entities.Payment;
using Domain.Entities.Shopping;
using Domain.Enums;
using System.Diagnostics.Contracts;

namespace Domain.Entities.UserFolder;

public class User : Auditable
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } = default!;
    public byte[] PasswordSalt { get; set; } = default!;
    public UserRole UserRole { get; set; }
    public ICollection<Order> Orders { get; set; }
    public ICollection<PaymentMethod> PaymentMethods { get; set; }
    public ICollection<UserAddress> UserAddresses { get; set; }
    public ICollection<UserReview> UserReviews { get; set; }

    public long CartId { get; set; }
    public ShoppingCart Cart { get; set; } = default!;
}
