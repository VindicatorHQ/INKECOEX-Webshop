using Microsoft.EntityFrameworkCore;
using WebshopService.Data;
using WebshopService.DTOs.Requests;
using WebshopService.Exceptions;
using WebshopService.Models;
using WebshopService.Repositories.Interface;

namespace WebshopService.Repositories.Implementation;

public class ProductRepository(WebshopDbContext context) : IProductRepository
{
    public async Task<IEnumerable<Product>> GetAllAsync(string? searchTerm = null, string? categorySlug = null)
    {
        var query = context.Products
            .Include(p => p.ProductCategories)
            .ThenInclude(pc => pc.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(categorySlug))
        {
            var lowerCaseSlug = categorySlug.Trim().ToLower();
        
            query = query.Where(p => 
                p.ProductCategories.Any(pc => 
                    pc.Category.Slug.ToLower() == lowerCaseSlug));
        }
    
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var lowerCaseTerm = searchTerm.Trim().ToLower();
        
            query = query.Where(p => 
                p.Name.ToLower().Contains(lowerCaseTerm) || 
                p.Description.ToLower().Contains(lowerCaseTerm));
        }
        
        return await query.ToListAsync();
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

    public async Task UpdateAsync(Product existingProduct, ProductRequest productRequest)
    {
        context.Entry(existingProduct).CurrentValues.SetValues(productRequest);

        var relationshipsToRemove = existingProduct.ProductCategories
            .Where(pc => !productRequest.CategoryIds.Contains(pc.CategoryId))
            .ToList();

        foreach (var pc in relationshipsToRemove)
        {
            existingProduct.ProductCategories.Remove(pc);
        }

        var existingCategoryIds = existingProduct.ProductCategories
            .Select(pc => pc.CategoryId)
            .ToHashSet();

        var categoryIdsToAdd = productRequest.CategoryIds
            .Where(id => !existingCategoryIds.Contains(id))
            .ToList();
    
        foreach (var categoryId in categoryIdsToAdd)
        {
            existingProduct.ProductCategories.Add(new ProductCategory 
            { 
                ProductId = existingProduct.Id,
                CategoryId = categoryId
            });
        }

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