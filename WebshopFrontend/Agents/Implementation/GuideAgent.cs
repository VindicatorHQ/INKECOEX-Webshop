using Blazored.SessionStorage;
using Flurl;
using Flurl.Http;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs.Requests;
using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Agents.Implementation;

public class GuideAgent(AgentUrl<GuideAgent> agentUrl, ISessionStorageService sessionStorage) : IGuideAgent
{
    private readonly string _baseUrl = agentUrl.Url;
    private readonly SessionStorage _sessionStorage = new(sessionStorage);

    public async Task<List<GuideResponse>> GetAllGuidesAsync()
    {
        try
        {
            var guides = await _baseUrl
                .AppendPathSegment("api/guides")
                .GetJsonAsync<List<GuideResponse>>();
            
            return guides ?? [];
        }
        catch (FlurlHttpException ex)
        {
            Console.WriteLine($"Error fetching guides: {ex.Message}");
            
            return [];
        }
    }
    
    public async Task<GuideResponse?> GetGuideBySlugAsync(string slug)
    {
        try
        {
            return await _baseUrl
                .AppendPathSegment("api/guides")
                .AppendPathSegment(slug)
                .GetJsonAsync<GuideResponse>();
        }
        catch (FlurlHttpException ex) when (ex.StatusCode == 404)
        {
            return null;
        }
        catch (FlurlHttpException ex)
        {
            Console.WriteLine($"Error fetching guide by slug: {ex.Message}");
            
            return null;
        }
    }
    
    public async Task<GuideResponse?> GetGuideByIdAsync(int id)
    {
        try
        {
            return await _baseUrl
                .AppendPathSegment("api/guides")
                .AppendPathSegment(id)
                .GetJsonAsync<GuideResponse>();
        }
        catch (FlurlHttpException ex) when (ex.StatusCode == 404)
        {
            return null;
        }
        catch (FlurlHttpException ex)
        {
            Console.WriteLine($"Error fetching guide by ID: {ex.Message}");
            return null;
        }
    }

    public async Task<GuideResponse?> CreateAsync(GuideRequest request)
    {
        try
        {
            var authRequest = await _sessionStorage.GetAuthorizedRequest(_baseUrl,"api/guides");

            return await authRequest
                .PostJsonAsync(request)
                .ReceiveJson<GuideResponse>();
        }
        catch (FlurlHttpException ex)
        {
            Console.WriteLine($"Error creating guide: {ex.Message}");
            
            return null;
        }
    }

    public async Task<bool> UpdateAsync(GuideRequest request)
    {
        try
        {
            var authRequest = await _sessionStorage.GetAuthorizedRequest(_baseUrl,$"api/guides/{request.Id}");

            await authRequest.PutJsonAsync(request);

            return true;
        }
        catch (FlurlHttpException ex)
        {
            Console.WriteLine($"Error updating guide: {ex.Message}");
            
            return false;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var authRequest = await _sessionStorage.GetAuthorizedRequest(_baseUrl,$"api/guides/{id}");

            await authRequest.DeleteAsync();

            return true;
        }
        catch (FlurlHttpException ex)
        {
            Console.WriteLine($"Error deleting guide: {ex.Message}");
            
            return false;
        }
    }
}