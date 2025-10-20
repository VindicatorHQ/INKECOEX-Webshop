using WebshopFrontend.DTOs.Requests;
using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Agents.Interface;

public interface IProductAgent
{
    Task<List<ProductResponse>> GetAllProductsAsync(string? categorySlug = null, string? searchTerm = null);
    Task<ProductResponse?> GetProductByIdAsync(int id);

    // Admin Routes
    Task<bool> CreateAsync(ProductRequest request);
    Task<bool> UpdateAsync(int id, ProductRequest request);
    Task<bool> DeleteAsync(int id);
}