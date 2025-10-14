using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Agents.Interface;

public interface IShoppingCartAgent
{
    Task<ShoppingCartResponse?> GetCartAsync();
    Task<ShoppingCartResponse?> AddToCartAsync(int productId, int quantity);
    Task<ShoppingCartResponse?> RemoveFromCartAsync(int productId);
}