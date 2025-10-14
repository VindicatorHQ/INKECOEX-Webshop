using System.Net;
using Blazored.SessionStorage;
using Flurl;
using Flurl.Http;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs.Requests;

namespace WebshopFrontend.Agents.Implementation;

public class OrderAgent(AgentUrl<OrderAgent> agentUrl, ISessionStorageService sessionStorage) : IOrderAgent
{
    private readonly string _baseUrl = agentUrl.Url;
    
    private async Task<IFlurlRequest> GetAuthorizedRequest(string path)
    {
        var token = await sessionStorage.GetItemAsStringAsync("authToken");

        return _baseUrl
            .AppendPathSegment(path)
            .WithOAuthBearerToken(token);
    }
    
    public async Task<int?> PlaceOrderAsync(CheckoutRequest request)
    {
        try
        {
            var flurlRequest = await GetAuthorizedRequest("api/orders");
            
            return flurlRequest
                .PostJsonAsync(request)
                .ReceiveJson<int>().Result;
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
}