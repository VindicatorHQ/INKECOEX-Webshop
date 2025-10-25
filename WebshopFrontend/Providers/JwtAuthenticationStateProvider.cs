using System.Security.Claims;
using System.Text.Json;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace WebshopFrontend.Providers;

public class JwtAuthenticationStateProvider: AuthenticationStateProvider
{
    private readonly ISessionStorageService _sessionStorage;
    private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());
    private const string TokenKey = "authToken";

    private AuthenticationState _currentState;

    public JwtAuthenticationStateProvider(ISessionStorageService sessionStorage)
    {
        _sessionStorage = sessionStorage;
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

        _currentState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        NotifyAuthenticationStateChanged(Task.FromResult(_currentState));
    }
    
    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        const string roleClaimKey = "role"; 
        
        if (keyValuePairs.TryGetValue(roleClaimKey, out object role))
        {
            switch (role)
            {
                case JsonElement { ValueKind: JsonValueKind.String } roleElement:
                    claims.Add(new Claim(ClaimTypes.Role, roleElement.GetString()));
                    break;
                case JsonElement { ValueKind: JsonValueKind.Array } roleArray:
                    claims.AddRange(roleArray.EnumerateArray()
                        .Select(r => new Claim(ClaimTypes.Role, r.GetString()))
                    );
                    break;
            }

            keyValuePairs.Remove(roleClaimKey);
        }
        
        const string nameClaimKey = "unique_name";
        
        if (keyValuePairs.TryGetValue(nameClaimKey, out object name) && name is JsonElement { ValueKind: JsonValueKind.String } nameElement)
        {
            claims.Add(new Claim(ClaimTypes.Name, nameElement.GetString()));
            
            keyValuePairs.Remove(nameClaimKey);
        }
        
        claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString() ?? string.Empty)));
        
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