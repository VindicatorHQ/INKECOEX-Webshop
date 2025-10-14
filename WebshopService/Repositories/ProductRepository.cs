using Microsoft.EntityFrameworkCore;
using WebshopService.Data;
using WebshopService.DTOs.Requests;
using WebshopService.Exceptions;
using WebshopService.Models;

namespace WebshopService.Repositories;

public class ProductRepository(WebshopDbContext context) : IProductRepository
{
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await context.Products
            .Include(p => p.ProductCategories)
            .ThenInclude(pc => pc.Category)
            .ToListAsync();
    }

    public Task<Product> GetByIdAsync(int id)
    {
        var existingProduct = context.Products.FirstOrDefault(p => p.Id == id);
        
        if (existingProduct == null)
        {
            throw new ProductNotFoundException($"Product with id '{id}' was not found.");
        }
        
        return Task.FromResult(existingProduct);
    }

    public Task AddAsync(Product product, IEnumerable<int> categoryIds)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Product existingProduct, ProductUpdateRequest product)
    {
        context.Entry(existingProduct).CurrentValues.SetValues(product);
        
        context.SaveChanges();

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        var product = context.Products.FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            throw new ProductNotFoundException($"Product with id '{id}' was not found.");
        }
        
        context.Products.Remove(product);
        
        context.SaveChanges();
        
        return Task.CompletedTask;
    }
}