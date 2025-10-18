using Blazored.SessionStorage;
using Flurl;
using Flurl.Http;

namespace WebshopFrontend;

public class SessionStorage(ISessionStorageService sessionStorage)
{
    public async Task<IFlurlRequest> GetAuthorizedRequest(string baseUrl, string path)
    {
        var token = await sessionStorage.GetItemAsStringAsync("authToken");

        return baseUrl
            .AppendPathSegment(path)
            .WithOAuthBearerToken(token);
    }
}