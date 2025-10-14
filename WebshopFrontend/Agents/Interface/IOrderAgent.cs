using WebshopFrontend.DTOs.Requests;

namespace WebshopFrontend.Agents.Interface;

public interface IOrderAgent
{
    Task<int?> PlaceOrderAsync(CheckoutRequest request);
    
    // TODO: Later voor Admin/Mijn Bestellingen
    // Task<List<OrderResponse>> GetOrdersAsync();
}