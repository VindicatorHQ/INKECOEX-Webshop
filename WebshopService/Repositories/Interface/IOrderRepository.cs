using WebshopService.Models;

namespace WebshopService.Repositories.Interface;

public interface IOrderRepository
{
    Task<int> PlaceOrderAsync(string userId, ShippingAddress address);
    
    // Admin
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task<Order?> GetOrderByIdAsync(int orderId);
}