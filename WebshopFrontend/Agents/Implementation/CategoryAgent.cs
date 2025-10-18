using Blazored.SessionStorage;
using Flurl;
using Flurl.Http;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs.Requests;
using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Agents.Implementation;

public class CategoryAgent(AgentUrl<CategoryAgent> agentUrl, ISessionStorageService sessionStorage) : ICategoryAgent
{
    private readonly string _baseUrl = agentUrl.Url;
    private readonly SessionStorage _sessionStorage = new(sessionStorage);
    
    public async Task<List<CategoryResponse>> GetAllCategoriesAsync()
    {
        try
        {
            var categories = await _baseUrl
                .AppendPathSegment("api/categories")
                .GetJsonAsync<List<CategoryResponse>>();
            
            return categories ?? [];
        }
        catch (FlurlHttpException ex)
        {
            Console.WriteLine($"Error fetching categories: {ex.Message}");
            
            return [];
        }
    }
    
    public async Task<CategoryResponse?> GetCategoryByIdAsync(int id)
    {
        try
        {
            return await _baseUrl.GetJsonAsync<CategoryResponse>();
        }
        catch (FlurlHttpException)
        {
            return null;
        }
    }

    public async Task<CategoryResponse?> CreateCategoryAsync(CategoryRequest request)
    {
        try
        {
            var authRequest = await _sessionStorage.GetAuthorizedRequest(_baseUrl, "api/admin/categories");

            return await authRequest
                .PostJsonAsync(request)
                .ReceiveJson<CategoryResponse>();
        }
        catch (FlurlHttpException)
        {
            return null;
        }
    }

    public async Task<bool> UpdateCategoryAsync(int id, CategoryRequest request)
    {
        try
        {
            var authRequest = await _sessionStorage.GetAuthorizedRequest(_baseUrl, $"api/admin/categories/{id}");

            await authRequest.PutJsonAsync(request);

            return true;
        }
        catch (FlurlHttpException)
        {
            return false;
        }
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        try
        {
            var authRequest = await _sessionStorage.GetAuthorizedRequest(_baseUrl, $"api/admin/categories/{id}");
            
            await authRequest.DeleteAsync();
            
            return true;
        }
        catch (FlurlHttpException ex) 
        { 
            if (ex.Call.Response.StatusCode == 409)
            {
                Console.WriteLine("Categorie kan niet verwijderd worden; nog gekoppeld.");
            }
            
            return false; 
        }
    }
}