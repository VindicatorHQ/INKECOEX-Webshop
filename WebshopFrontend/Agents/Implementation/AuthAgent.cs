using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Components.Authorization;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs.Requests;
using WebshopFrontend.DTOs.Responses;
using WebshopFrontend.Providers;

namespace WebshopFrontend.Agents.Implementation;

public class AuthAgent(AgentUrl<AuthAgent> agentUrl, AuthenticationStateProvider authProvider) : IAuthAgent
{
    private readonly string _baseUrl = agentUrl.Url;
    private readonly JwtAuthenticationStateProvider _jwtProvider = (JwtAuthenticationStateProvider)authProvider;

    public async Task<bool> LoginAsync(LoginRequest model)
    {
        try
        {
            var authResponse = await _baseUrl
                .AppendPathSegment("api/auth/login")
                .PostJsonAsync(model)
                .ReceiveJson<AuthResponse>();

            if (string.IsNullOrEmpty(authResponse.Token))
            {
                return false;
            }

            await _jwtProvider.MarkUserAuthenticated(authResponse.Token);
            
            return true;
        }
        catch (FlurlHttpException ex) when (ex.Call.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            Console.WriteLine("Login mislukt: Ongeldige gegevens.");
            
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fout bij inloggen: {ex.Message}");
            
            return false;
        }
    }
    
    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        try
        {
            var registerDto = new 
            {
                request.Email,
                request.Password
            };
            
            var response = await _baseUrl
                .AppendPathSegment("api/auth/register")
                .PostJsonAsync(registerDto);
            
            return response.ResponseMessage.IsSuccessStatusCode;
        }
        catch (FlurlHttpException ex)
        {
            Console.WriteLine($"Registratiefout: Status {ex.StatusCode}, Bericht: {await ex.GetResponseStringAsync()}");
            
            return false;
        }
    }

    public async Task LogoutAsync()
    {
        await _jwtProvider.MarkUserLoggedOut();
    }
}