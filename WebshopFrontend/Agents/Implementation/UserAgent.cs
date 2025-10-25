using Blazored.SessionStorage;
using Flurl.Http;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs.Requests;
using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Agents.Implementation;

public class UserAgent(AgentUrl<UserAgent> agentUrl, ISessionStorageService sessionStorage): IUserAgent
{
    private readonly string _baseUrl = agentUrl.Url;
    private readonly SessionStorage _sessionStorage = new(sessionStorage);
    
    public async Task<UserProfileResponse?> GetUserProfileAsync()
    {
        try
        {
            var authRequest = await _sessionStorage.GetAuthorizedRequest(_baseUrl, "api/profile");
            
            return await authRequest.GetJsonAsync<UserProfileResponse>();
        }
        catch (FlurlHttpException ex)
        {
            Console.WriteLine($"Profiel ophalen mislukt: {ex.StatusCode}");
            
            return null;
        }
    }

    public async Task<bool> UpdateUserProfileAsync(UserProfileRequest profile)
    {
        try
        {
            var authRequest = await _sessionStorage.GetAuthorizedRequest(_baseUrl, "api/profile");

            await authRequest.PutJsonAsync(profile);

            return true;
        }
        catch (FlurlHttpException ex)
        {
            Console.WriteLine($"Profiel updaten mislukt: {ex.StatusCode}");
            
            return false;
        }
    }
}