using Domain.Entities.OrderFolder;
using Service.DTOs.Orders;

namespace Service.DTOs.OrderStates;

public class OrderStatusResultDto
{
    public long Id { get; set; }
    public string Name { get; set; }
}