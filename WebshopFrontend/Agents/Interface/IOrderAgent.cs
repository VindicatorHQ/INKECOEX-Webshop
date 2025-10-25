using WebshopFrontend.DTOs.Requests;
using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Agents.Interface;

public interface IOrderAgent
{
    Task<int?> PlaceOrderAsync(CheckoutRequest request);
    Task<List<OrderSummaryResponse>?> GetMyOrdersAsync();
    Task<OrderDetailResponse?> GetOrderDetailAsync(int orderId);
    
    // Admin
    Task<List<OrderSummaryResponse>?> GetAllOrdersForAdminAsync();
    Task<OrderDetailResponse?> GetAdminOrderDetailAsync(int orderId);
    Task<bool> UpdateOrderStatusAsync(int orderId, string newStatus);
}