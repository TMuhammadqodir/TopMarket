using AutoMapper;
using Data.IRepositories;
using Domain.Entities.OrderFolder;
using Service.Interfaces;
using Service.DTOs.Orders;
using Service.Exceptions;

namespace Service.Services;

public class OrderService : IOrderService
{
    private readonly IMapper mapper;
    private readonly IRepository<Order> orderRepository;
    private readonly IRepository<OrderStatus> addressRepository;
    private readonly IRepository<OrderStatus> orderStatusRepository;
    private readonly IRepository<ShippingMethod> shippingMethodRepository;

    public OrderService(
        IMapper mapper,
        IRepository<Order> orderRepository,
        IRepository<OrderStatus> addressRepository,
        IRepository<OrderStatus> orderStatusRepository,
        IRepository<ShippingMethod> shippingMethodRepository)
    {
        this.mapper = mapper;
        this.orderRepository = orderRepository;
        this.addressRepository = addressRepository;
        this.orderStatusRepository = orderStatusRepository;
        this.shippingMethodRepository = shippingMethodRepository;
    }

    public async Task<OrderResultDto> CreateAsync(OrderCreationDto dto)
    {
        var existAddress = await addressRepository.GetAsync(a => a.Id.Equals(dto.AddressId))
            ?? throw new NotFoundException($"This address was not found with = {dto.AddressId}");

        var existOrderStatus = await orderStatusRepository.GetAsync(o => o.Id.Equals(dto.StatusId))
            ?? throw new NotFoundException($"This orderStatus was not found with = {dto.StatusId}");
        
        var existShippingMethod = await shippingMethodRepository.GetAsync(sh => sh.Id.Equals(dto.ShippingMethodId))
            ?? throw new NotFoundException($"This shippingMethod was not found with = {dto.ShippingMethodId}");

        throw new NotImplementedException();
    }
    public Task<OrderResultDto> UpdateAsync(OrderUpdateDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<OrderResultDto>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<OrderResultDto> GetByIdAsync(long id)
    {
        throw new NotImplementedException();
    }
}
