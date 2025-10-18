using WebshopService.DTOs.Requests;
using WebshopService.Models;

namespace WebshopService.Repositories.Interface;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync(string? searchTerm = null);
    Task<Product> GetByIdAsync(int id);
    
    // Admin
    Task<Product> CreateAsync(Product product);
    Task UpdateAsync(Product product, ProductRequest request);
    Task DeleteAsync(int id);
}