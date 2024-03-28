using Domain.Entities.Shopping;
using Service.DTOs.Carts;
using System.Linq.Expressions;

namespace Service.Interfaces;

public interface ICartItemService
{
    ValueTask<CartItemResultDto> AddAsync(CartItemCreationDto dto);
    ValueTask<CartItemResultDto> ModifyAsync(CartItemUpdateDto dto);
    ValueTask<bool> RemoveAsync(long id);
    ValueTask<bool> RemoveAllAsync(long cartId);
    ValueTask<CartItemResultDto> RetrieveAsync(Expression<Func<ShoppingCartItem, bool>> expression);
    ValueTask<CartItemResultDto> RetrieveAsync(long id);
    Task<IEnumerable<CartItemResultDto>> RetrieveAllAsync(long cartId);
}
