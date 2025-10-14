using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Agents.Interface;

public interface ICategoryAgent
{
    Task<List<CategoryResponse>> GetCategoriesAsync();
}