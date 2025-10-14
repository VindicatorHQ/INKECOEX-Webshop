using Flurl;
using Flurl.Http;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Agents.Implementation;

public class CategoryAgent(AgentUrl<CategoryAgent> agentUrl) : ICategoryAgent
{
    private readonly string _baseUrl = agentUrl.Url;
    
    public async Task<List<CategoryResponse>> GetCategoriesAsync()
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
}