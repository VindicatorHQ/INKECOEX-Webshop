using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace WebshopFrontend.Providers;

public class JwtAuthenticationStateProvider: AuthenticationStateProvider
{
    private readonly ISessionStorageService _sessionStorage;
    private readonly HttpClient _httpClient;
    private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());
    private const string TokenKey = "authToken";

    private AuthenticationState _currentState;

    public JwtAuthenticationStateProvider(ISessionStorageService sessionStorage, HttpClient httpClient)
    {
        _sessionStorage = sessionStorage;
        _httpClient = httpClient;
        _currentState = new AuthenticationState(_anonymous); 
    }
    
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(_currentState);
    }

    public async Task InitializeAsync()
    {
        var savedToken = await _sessionStorage.GetItemAsStringAsync(TokenKey);

        if (string.IsNullOrWhiteSpace(savedToken))
        {
            _currentState = new AuthenticationState(_anonymous);
        }
        else
        {
            try
            {
                var claims = ParseClaimsFromJwt(savedToken);
                var expiry = claims.FirstOrDefault(c => c.Type == "exp")?.Value;
                
                if (expiry != null && DateTimeOffset.FromUnixTimeSeconds(long.Parse(expiry)).UtcDateTime > DateTime.UtcNow)
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);
                    _currentState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")));
                }
                else
                {
                    await MarkUserLoggedOut();
                }
            }
            catch
            {
                await MarkUserLoggedOut();
            }
        }
        
        NotifyAuthenticationStateChanged(Task.FromResult(_currentState));
    }
    
    public async Task MarkUserAuthenticated(string token)
    {
        await _sessionStorage.SetItemAsStringAsync(TokenKey, token);
     
        await InitializeAsync(); 
    }

    public async Task MarkUserLoggedOut()
    {
        await _sessionStorage.RemoveItemAsync(TokenKey);
        
        _httpClient.DefaultRequestHeaders.Authorization = null;
        _currentState = new AuthenticationState(_anonymous);
        
        NotifyAuthenticationStateChanged(Task.FromResult(_currentState));
    }
    
    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        if (keyValuePairs.TryGetValue(ClaimTypes.Role, out object role))
        {
            if (role is JsonElement roleElement && roleElement.ValueKind == JsonValueKind.String)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleElement.GetString()));
            }
            else if (role is JsonElement roleArray && roleArray.ValueKind == JsonValueKind.Array)
            {
                claims.AddRange(roleArray.EnumerateArray().Select(r => new Claim(ClaimTypes.Role, r.GetString())));
            }
            
            keyValuePairs.Remove(ClaimTypes.Role);
        }
        
        claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
        
        return claims;
    }

    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        
        return Convert.FromBase64String(base64);
    }
}