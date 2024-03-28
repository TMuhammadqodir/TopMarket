using AutoMapper;
using Data.IRepositories;
using Domain.Entities.Shopping;
using Microsoft.EntityFrameworkCore;
using Service.DTOs.Carts;
using Service.Exceptions;
using Service.Interfaces;

namespace Service.Services;

public class CartService : ICartService
{
    private readonly IMapper _mapper;
    private readonly IProductItemService _productItemService;
    private readonly IRepository<ShoppingCart> _cartRepository;
    private readonly IRepository<ShoppingCartItem> _cartItemRepository;

    public CartService(IMapper mapper,
                       IRepository<ShoppingCart> cartRepository,
                       IRepository<ShoppingCartItem> cartItemRepository,
                       IProductItemService productItemService)
    {
        _mapper = mapper;
        _productItemService = productItemService;
        _cartRepository = cartRepository;
        _cartItemRepository = cartItemRepository;
    }

    public async Task<CartResultDto> AddItemToCartAsync(long productItemId, long cartId)
    {
        var items = await _cartItemRepository.GetAll(i => i.CartId.Equals(cartId), isNoTracked: false).ToListAsync()
            ?? throw new NotFoundException($"Cart with id = '{cartId}' is not found.");
        
        var theProductItem = await _productItemService.GetByIdAsync(productItemId)
            ?? throw new NotFoundException($"ProductItem with id = '{productItemId}' is not found.");

        var theCartItem = items.FirstOrDefault(i => i.ProductItemId.Equals(productItemId));
        if (theCartItem is null)
            items.Add(new ShoppingCartItem
            {
                CartId = cartId,
                ProductItemId = productItemId,
                Quantity = 1,
                Price = theProductItem.Price,
            });
        else
            theCartItem.Quantity += 1;
    
        await _cartItemRepository.SaveAsync();

        var cart = await _cartRepository.GetAsync(cartId);
        return _mapper.Map<CartResultDto>(cart);
    }

    public async Task<CartResultDto> CreateAsync()
    {
        var newCart = new ShoppingCart();

        await _cartRepository.AddAsync(newCart);    
        await _cartRepository.SaveAsync();

        return _mapper.Map<CartResultDto>(newCart);
    }
    
    public async Task<bool> ClearCartAsync(long cartId)
    {
        var items = await _cartItemRepository.GetAll(i => i.CartId.Equals(cartId), isNoTracked: false).ToListAsync()
            ?? throw new NotFoundException($"Cart with id = '{cartId}' is not found.");
        
        foreach (var item in items)
            _cartItemRepository.Destroy(item);
        
        await _cartItemRepository.SaveAsync();

        return true;
    }

    public async Task<bool> RemoveFromCartAsync(long cartItemId)
    {
        var theItem = await _cartItemRepository.GetAsync(cartItemId)
            ?? throw new NotFoundException("Cart item is not found.");

        _cartItemRepository.Destroy(theItem);
        await _cartItemRepository.SaveAsync();

        return true;
    }

    public async Task<ICollection<CartItemResultDto>> RetrieveAllItemsAsync(long cartId)
    {
        var items = await _cartItemRepository.GetAll(i => i.CartId.Equals(cartId)).ToListAsync();
        return _mapper.Map<ICollection<CartItemResultDto>>(items);
    }

    public async Task<CartItemResultDto> UpdateItemQuantityAsync(CartItemUpdateDto dto)
    {
        var theItem = await _cartItemRepository.GetAsync(dto.Id)
            ?? throw new NotFoundException("Product is not found.");
       
        _mapper.Map(dto, theItem);
        theItem.Quantity = dto.Quantity;
        await _cartItemRepository.SaveAsync();

        return _mapper.Map<CartItemResultDto>(theItem);
    }
}