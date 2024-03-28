using Domain.Entities.Shopping;
using Domain.Enums;

namespace Service.DTOs.Users;

public class UserResultDto
{
    public long Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public UserRole UserRole { get; set; }

    public long CartId { get; set; }
    public ShoppingCart Cart { get; set; } = default!;
}
