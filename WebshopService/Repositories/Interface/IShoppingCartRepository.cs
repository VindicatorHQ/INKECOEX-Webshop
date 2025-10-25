using WebshopService.Models;

namespace WebshopService.Repositories.Interface;

public interface IShoppingCartRepository
{
    Task<ShoppingCart?> GetByUserIdAsync(string userId);
    
    Task SaveAsync(ShoppingCart cart);
}