using Microsoft.EntityFrameworkCore;
using WebshopService.Data;
using WebshopService.DTOs.Requests;
using WebshopService.Exceptions;
using WebshopService.Models;
using WebshopService.Repositories.Interface;

namespace WebshopService.Repositories.Implementation;

public class ProductRepository(WebshopDbContext context) : IProductRepository
{
    public async Task<IEnumerable<Product>> GetAllAsync(string? searchTerm = null)
    {
        var query = context.Products
            .Include(p => p.ProductCategories)
            .ThenInclude(pc => pc.Category);

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return await query.ToListAsync();
        }

        var lowerCaseTerm = searchTerm.Trim().ToLower();
        
        return await query.Where(p => 
            p.Name.ToLower().Contains(lowerCaseTerm) || 
            p.Description.ToLower().Contains(lowerCaseTerm))
        .ToListAsync();
    }

    public Task<Product> GetByIdAsync(int id)
    {
        var existingProduct = context.Products
            .Include(p => p.ProductCategories)
            .ThenInclude(pc => pc.Category)
            .FirstOrDefault(p => p.Id == id);
        
        if (existingProduct == null)
        {
            throw new ProductNotFoundException($"Product with id '{id}' was not found.");
        }
        
        return Task.FromResult(existingProduct);
    }

    public async Task<Product> CreateAsync(Product product)
    {
        context.Products.Add(product);
        
        await context.SaveChangesAsync();
        
        return product;
    }

    public async Task UpdateAsync(Product existingProduct, ProductRequest product)
    {
        context.Entry(existingProduct).CurrentValues.SetValues(product);
        
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var product = context.Products.FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            throw new ProductNotFoundException($"Product with id '{id}' was not found.");
        }
        
        context.Products.Remove(product);
        
        await context.SaveChangesAsync();
    }
}