using Domain.Commons;
using Domain.Entities.OrderFolder;
using Domain.Enums;

namespace Domain.Entities.UserFolder;

public class UserReview : Auditable
{
    public long UserId { get; set; }
    public User User { get; set; } = default!;

    public long OrderLineId { get; set; }
    public OrderLine OrderLine { get; set; } = default!;

    public ERating RatingValue { get; set; } 
    public string Comment { get; set; } = string.Empty;
}
