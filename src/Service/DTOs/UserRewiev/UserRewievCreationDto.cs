using Domain.Enums;

namespace Service.DTOs.UserRewiev;

public class UserRewievCreationDto
{
    public long UserId { get; set; }
    public long OrderProductId { get; set; }
    public ERating RatingValue { get; set; }
    public string Comment { get; set; }
}
