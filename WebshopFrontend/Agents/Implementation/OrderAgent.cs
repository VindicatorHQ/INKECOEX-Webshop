using System.Net;
using Blazored.SessionStorage;
using Flurl;
using Flurl.Http;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs.Requests;
using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Agents.Implementation;

public class OrderAgent(AgentUrl<OrderAgent> agentUrl, ISessionStorageService sessionStorage) : IOrderAgent
{
    private readonly string _baseUrl = agentUrl.Url;
    private readonly SessionStorage _sessionStorage = new(sessionStorage);
    
    public async Task<int?> PlaceOrderAsync(CheckoutRequest request)
    {
        try
        {
            var authRequest = await _sessionStorage.GetAuthorizedRequest(_baseUrl,"api/orders");
            
            return await authRequest
                .PostJsonAsync(request)
                .ReceiveJson<int>();
        }
        catch (FlurlHttpException ex) when (ex.Call.Response.StatusCode == (int)HttpStatusCode.BadRequest)
        {
            var error = await ex.GetResponseStringAsync();
            
            Console.WriteLine($"Checkout Error: {error}");
            
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General Checkout Error: {ex.Message}");
            
            return null;
        }
    }
    
    public async Task<List<OrderSummaryResponse>?> GetMyOrdersAsync()
    {
        try
        {
            var authRequest = await _sessionStorage.GetAuthorizedRequest(_baseUrl,"api/orders");
            
            return await authRequest.GetJsonAsync<List<OrderSummaryResponse>>();
        }
        catch (FlurlHttpException ex) when (ex.Call.Response.StatusCode == 401)
        {
            return null; 
        }
        catch (Exception)
        {
            return null;
        }
    }
    
    public async Task<OrderDetailResponse?> GetOrderDetailAsync(int orderId)
    {
        try
        {
            var authRequest = await _sessionStorage.GetAuthorizedRequest(_baseUrl,$"api/orders/{orderId}");
            
            return await authRequest.GetJsonAsync<OrderDetailResponse>();
        }
        catch (FlurlHttpException ex) when (ex.Call.Response.StatusCode == 404)
        {
            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }
    
    public async Task<List<OrderSummaryResponse>?> GetAllOrdersForAdminAsync()
    {
        try
        {
            var authRequest = await _sessionStorage.GetAuthorizedRequest(_baseUrl,"api/admin/orders");
            
            return await authRequest.GetJsonAsync<List<OrderSummaryResponse>>();
        }
        catch (FlurlHttpException ex) when (ex.Call.Response.StatusCode == 403)
        {
            return null; 
        }
    }

    public async Task<OrderDetailResponse?> GetAdminOrderDetailAsync(int orderId)
    {
        try
        {
            var authRequest = await _sessionStorage.GetAuthorizedRequest(_baseUrl,$"api/admin/orders/{orderId}");
            
            return await authRequest.GetJsonAsync<OrderDetailResponse>();
        }
        catch (FlurlHttpException)
        {
            return null;
        }
    }

    public async Task<bool> UpdateOrderStatusAsync(int orderId, string newStatus)
    {
        try
        {
            var authRequest = await _sessionStorage.GetAuthorizedRequest(_baseUrl,$"api/admin/orders/{orderId}/status");
            
            var request = new { NewStatus = newStatus };
            
            await authRequest.PutJsonAsync(request);
            
            return true;
        }
        catch (FlurlHttpException)
        {
            return false;
        }
    }
}