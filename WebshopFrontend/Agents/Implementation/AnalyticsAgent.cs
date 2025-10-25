using Blazored.SessionStorage;
using Flurl.Http;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Agents.Implementation;

public class AnalyticsAgent(AgentUrl<AnalyticsAgent> agentUrl, ISessionStorageService sessionStorage) : IAnalyticsAgent
{
    private readonly string _baseUrl = agentUrl.Url;
    private readonly SessionStorage _sessionStorage = new(sessionStorage);
    
    public async Task<SalesDataResponse?> GetSalesStatisticsAsync(int days)
    {
        try
        {
            var authRequest = await _sessionStorage.GetAuthorizedRequest(_baseUrl,"api/admin/sales/statistics");
        
            return await authRequest
                .SetQueryParam("days", days)
                .GetJsonAsync<SalesDataResponse>();
        }
        catch (FlurlHttpException ex) when (ex.Call.Response.StatusCode == 403)
        {
            Console.WriteLine("Autorisatiefout: Alleen beheerders mogen analytics ophalen.");
            Console.WriteLine($"Status 403: {ex.Message}");
            
            return new SalesDataResponse();
        }
        catch (FlurlHttpException ex)
        {
            Console.WriteLine($"Flurl Error gathering analytics data: {ex.Message}");
            
            return new SalesDataResponse();
        }
    }
}