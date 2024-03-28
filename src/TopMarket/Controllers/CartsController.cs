using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Carts;
using Service.Interfaces;
using System.Security.Claims;
using TopMarket.Models;

namespace TopMarket.Controllers
{
    public class CartsController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;
        
        public CartsController(ICartService cartService, IAuthService authService)
        {
            _cartService = cartService;
            _authService = authService;
        }


        [HttpPost("add-item/{id:long}")]
        public async Task<IActionResult> AddToCart(long productItemId)
        {
            long userId = Convert.ToInt32(HttpContext.User.FindFirstValue("id"));
            long cartId = (await _authService.GetByIdAsync(userId)).CartId;

            return Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await _cartService.AddItemToCartAsync(productItemId, cartId)
            });
        }


        [HttpDelete("clear-cart")]
        public async Task<IActionResult> ClearCart()
        {
            long userId = Convert.ToInt32(HttpContext.User.FindFirstValue("id"));
            long cartId = (await _authService.GetByIdAsync(userId)).CartId;

            return Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await _cartService.ClearCartAsync(cartId)
            });
        }


        [HttpGet("get-items")]
        public async Task<IActionResult> GetCartItems()
        {
            long userId = Convert.ToInt32(HttpContext.User.FindFirstValue("id"));
            long cartId = (await _authService.GetByIdAsync(userId)).CartId;

            return Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await _cartService.RetrieveAllItemsAsync(cartId)
            });
        }


        [HttpPut("remove-item/{id:long}")]
        public async Task<IActionResult> RemoveItemFromCart(long cartItemId)
            => Ok(new Response
            {
                StatusCode =200,
                Message = "Success",
                Data = await _cartService.RemoveFromCartAsync(cartItemId)
            });


        [HttpPut("update-quantity")]
        public async Task<IActionResult> UpdateQuantity(CartItemUpdateDto dto)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await _cartService.UpdateItemQuantityAsync(dto)
            });
    }
}
