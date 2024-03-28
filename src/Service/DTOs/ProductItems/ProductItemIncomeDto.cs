using Domain.Entities.OrderFolder;
using Domain.Entities.ProductFolder;

namespace Service.DTOs.ProductItems;

public class ProductItemIncomeDto
{
    public long Id { get; set; }
    public decimal QuantityInStock { get; set; }
}
