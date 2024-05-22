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
    private readonly IMapper mapper;
    private readonly IProductItemService productItemService;
    private readonly IRepository<ShoppingCart> cartRepository;
    private readonly IRepository<ShoppingCartItem> cartItemRepository;

    public CartService(IMapper mapper,
                       IRepository<ShoppingCart> cartRepository,
                       IRepository<ShoppingCartItem> cartItemRepository,
                       IProductItemService productItemService)
    {
        this.mapper = mapper;
        this.productItemService = productItemService;
        this.cartRepository = cartRepository;
        this.cartItemRepository = cartItemRepository;
    }

    public async Task<CartResultDto> AddItemToCartAsync(
        long productItemId,
        long cartId,
        CancellationToken cancellationToken = default)
    {
        var items = await this.cartItemRepository
                .GetAll(i => i.CartId == cartId, isNoTracked: false)
                .ToListAsync(cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Cart with id = '{cartId}' is not found.");
        
        var productItem = await this.productItemService.RetrieveByIdAsync(productItemId)
            ?? throw new NotFoundException($"ProductItem with id = '{productItemId}' is not found.");

        var cartItem = items.FirstOrDefault(i => i.ProductItemId == productItemId);
        if (cartItem is null)
            items.Add(new ShoppingCartItem
            {
                CartId = cartId,
                ProductItemId = productItemId,
                Quantity = 1,
                Price = productItem.Price,
            });
        else
            cartItem.Quantity += 1;
    
        await this.cartItemRepository.SaveAsync(cancellationToken);

        var cart = await this.cartRepository.GetAsync(cartId, cancellationToken: cancellationToken);
        return this.mapper.Map<CartResultDto>(cart);
    }

    public async Task<CartResultDto> CreateAsync(CancellationToken cancellationToken = default)
    {
        var newCart = new ShoppingCart();

        await this.cartRepository.AddAsync(newCart, cancellationToken);    
        await this.cartRepository.SaveAsync(cancellationToken);

        return this.mapper.Map<CartResultDto>(newCart);
    }
    
    public async Task<bool> ClearCartAsync(long cartId, CancellationToken cancellationToken = default)
    {
        var items = await this.cartItemRepository
                .GetAll(i => i.CartId == cartId, isNoTracked: false)
                .ToListAsync(cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Cart with id = '{cartId}' is not found.");
        
        foreach (var item in items)
            this.cartItemRepository.Destroy(item);
        
        await this.cartItemRepository.SaveAsync(cancellationToken);

        return true;
    }

    public async Task<bool> RemoveFromCartAsync(long cartItemId, CancellationToken cancellationToken = default)
    {
        var item = await this.cartItemRepository.GetAsync(cartItemId, cancellationToken: cancellationToken)
            ?? throw new NotFoundException("Cart item is not found.");

        this.cartItemRepository.Destroy(item);
        await this.cartItemRepository.SaveAsync(cancellationToken);

        return true;
    }

    public async Task<ICollection<CartItemResultDto>> RetrieveAllItemsAsync(long cartId, CancellationToken cancellationToken = default)
    {
        var items = await this.cartItemRepository
                .GetAll(i => i.CartId == cartId)
                .ToListAsync(cancellationToken: cancellationToken);
        return this.mapper.Map<ICollection<CartItemResultDto>>(items);
    }

    public async Task<CartItemResultDto> UpdateItemQuantityAsync(CartItemUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var item = await this.cartItemRepository
                .GetAsync(dto.Id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException("Product is not found.");
       
        this.mapper.Map(dto, item);
        item.Quantity = dto.Quantity;
        await this.cartItemRepository.SaveAsync(cancellationToken);

        return this.mapper.Map<CartItemResultDto>(item);
    }
}