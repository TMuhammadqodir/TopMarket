using Data.Contexts;
using Data.IRepositories;
using Domain.Entities.Shopping;

namespace Data.Repositories;

public class CartRepository : Repository<ShoppingCart>, ICartRepository
{
    private readonly AppDbContext _appDbContext;
    
    public CartRepository(AppDbContext appDbContext) : base(appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public Task<ShoppingCart> CreateCartAsync()
    {
        throw new Exception();   
    }
}
