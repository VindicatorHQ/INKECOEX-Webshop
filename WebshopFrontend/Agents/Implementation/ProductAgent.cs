using Blazored.SessionStorage;
using Flurl;
using Flurl.Http;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs.Requests;
using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Agents.Implementation;

public class ProductAgent(AgentUrl<ProductAgent> agentUrl, ISessionStorageService sessionStorage) : IProductAgent
{
    private readonly string _baseUrl = agentUrl.Url;
    private readonly SessionStorage _sessionStorage = new(sessionStorage);
    
    public async Task<List<ProductResponse>> GetAllProductsAsync(string? searchTerm = null)
    {
        try
        {
            var request = _baseUrl.AppendPathSegment("api/products");
            
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                request = request.SetQueryParam("search", searchTerm);
            }
            
            var products = await request.GetJsonAsync<List<ProductResponse>>();
            
            return products ?? [];
        }
        catch (FlurlHttpException ex)
        {
            Console.WriteLine($"Flurl Error fetching products: {ex.Message}");
            
            return [];
        }
    }
    
    public async Task<ProductResponse?> GetProductByIdAsync(int id)
    {
        try
        {
            var product = await _baseUrl
                .AppendPathSegment($"api/products/{id}")
                .GetJsonAsync<ProductResponse>();
            
            return product;
        }
        catch (FlurlHttpException ex)
        {
            Console.WriteLine($"Flurl Error fetching product with id '{id}': {ex.Message}");
            
            return null;
        }
    }
    
    public async Task<bool> CreateAsync(ProductRequest request)
    {
        try
        {
            var authRequest = await _sessionStorage.GetAuthorizedRequest(_baseUrl,"api/admin/products");

            await authRequest
                .PostJsonAsync(request)
                .ReceiveJson<ProductResponse>();

            return true;
        }
        catch (FlurlHttpException ex) when (ex.Call.Response.StatusCode == 403)
        {
            Console.WriteLine("Autorisatiefout: Alleen beheerders mogen producten toevoegen.");
            Console.WriteLine($"Status 403: {ex.Message}");
            
            return false;
        }
        catch (FlurlHttpException ex)
        {
            Console.WriteLine($"Flurl Error creating product: {ex.Message}");
            
            return false;
        }
    }

    public async Task<bool> UpdateAsync(int id, ProductRequest request)
    {
        try
        {
            var authRequest = await _sessionStorage.GetAuthorizedRequest(_baseUrl,$"api/admin/products/{id}");

            await authRequest.PutJsonAsync(request);

            return true;
        }
        catch (FlurlHttpException ex) when (ex.Call.Response.StatusCode == 404)
        {
            Console.WriteLine($"Status 404: {ex.Message}");
            
            return false;
        }
        catch (FlurlHttpException ex)
        {
            Console.WriteLine($"Flurl Error updating product with id '{id}': {ex.Message}");
            
            return false;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var authRequest = await _sessionStorage.GetAuthorizedRequest(_baseUrl,$"api/admin/products/{id}");

            await authRequest.DeleteAsync();

            return true;
        }
        catch (FlurlHttpException ex) when (ex.Call.Response.StatusCode == 404)
        {
            Console.WriteLine($"Status 404: {ex.Message}");
            
            return false;
        }
        catch (FlurlHttpException ex)
        {
            Console.WriteLine($"Flurl Error deleting product with id '{id}': {ex.Message}");
            
            return false;
        }
    }
}