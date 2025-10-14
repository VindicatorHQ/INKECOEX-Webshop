using WebshopFrontend.DTOs.Requests;
using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Agents.Interface;

public interface IProductAgent
{
    Task<List<ProductResponse>> GetAsync();
    Task<ProductResponse?> GetByIdAsync(int id);

    // Admin Routes
    Task<bool> CreateAsync(ProductCreateRequest request);
    Task<bool> UpdateAsync(int id, ProductCreateRequest request);
    Task<bool> DeleteAsync(int id);
}