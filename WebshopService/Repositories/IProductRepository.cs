using WebshopService.Models;

namespace WebshopService.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product> GetByIdAsync(int id);
    Task AddAsync(Product product, IEnumerable<int> categoryIds);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
}