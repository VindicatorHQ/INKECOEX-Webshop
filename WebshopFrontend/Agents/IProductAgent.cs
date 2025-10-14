using WebshopFrontend.DTOs.Requests;
using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Agents;

public interface IProductAgent
{
    Task<List<ProductResponse>> GetProductsAsync();
    Task<ProductResponse?> GetProductByIdAsync(int id);

    // Admin Routes
    Task<bool> CreateProductAsync(ProductCreateRequest request);
    Task<bool> UpdateProductAsync(int id, ProductUpdateRequest request);
    Task<bool> DeleteProductAsync(int id);
}