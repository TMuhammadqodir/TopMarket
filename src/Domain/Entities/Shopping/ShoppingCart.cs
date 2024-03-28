using Domain.Commons;
using Domain.Entities.UserFolder;

namespace Domain.Entities.Shopping;

public class ShoppingCart : Auditable
{
    public ICollection<ShoppingCartItem> Items { get; set; }
}