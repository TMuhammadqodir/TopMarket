using Service.DTOs.Carts;

namespace Service.Interfaces;

public interface ICartService
{
    /// <summary>
    /// 1. Creates 'CartItem' with the given 'ProductItem'
    /// 2. Inserts the created 'CartItem' to the given 'ShoppingCart'
    /// </summary>
    /// <param name="cartId">ShoppingCart.Id</param>
    /// <param name="productItemId">ProductItem.Id</param>
    /// <returns></returns>
    Task<CartResultDto> AddItemToCartAsync(long productItemId, long cartId);
    
    /// <summary>
    /// Clears the cart totally.
    /// </summary>
    /// <param name="cartId">ShoppingCart.Id</param>
    /// <returns>bool</returns>
    Task<bool> ClearCartAsync(long cartId);

    /// <summary>
    /// Creates a new shopping cart.
    /// </summary>
    /// <returns>CartResultDto</returns>
    Task<CartResultDto> CreateAsync();

    /// <summary>
    /// Removes the asked item from the cart.
    /// </summary>
    /// <param name="cartItemId"></param>
    /// <returns>bool</returns>
    Task<bool> RemoveFromCartAsync(long cartItemId);

    /// <summary>
    /// Returns all items within the cart.
    /// </summary>
    /// <param name="cartId">ShoppingCart.Id</param>
    /// <returns>List of CartItems</returns>
    Task<ICollection<CartItemResultDto>> RetrieveAllItemsAsync(long cartId);
    
    /// <summary>
    /// Updates actual quantity of the cart item in database.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns>Cart item result itself</returns>
    Task<CartItemResultDto> UpdateItemQuantityAsync(CartItemUpdateDto dto);
}
