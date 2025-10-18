using WebshopService.Models;

namespace WebshopService.Repositories.Interface;

public interface IOrderRepository
{
    Task<int> PlaceOrderAsync(string userId, ShippingAddress address);
    Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
    Task<Order?> GetOrderDetailAsync(int orderId, string userId);
    
    // Admin
    Task<IEnumerable<Order>> GetAllOrdersAsync(); 
    Task<Order?> GetOrderDetailsByIdAsync(int orderId); 
    Task UpdateOrderStatusAsync(int orderId, string newStatus);
}