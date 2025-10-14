using WebshopService.DTOs.Requests;
using WebshopService.Models;

namespace WebshopService.Repositories.Interface;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product> GetByIdAsync(int id);
    Task<Product> CreateAsync(Product product);
    Task UpdateAsync(Product product, ProductCreateRequest request);
    Task DeleteAsync(int id);
}