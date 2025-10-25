using WebshopFrontend.DTOs.Requests;
using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Agents.Interface;

public interface ICategoryAgent
{
    Task<List<CategoryResponse>> GetAllCategoriesAsync();
    Task<CategoryResponse?> GetCategoryByIdAsync(int id);
    
    // Admin
    Task<CategoryResponse?> CreateCategoryAsync(CategoryRequest request);
    Task<bool> UpdateCategoryAsync(int id, CategoryRequest request);
    Task<bool> DeleteCategoryAsync(int id);
}