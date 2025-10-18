using Microsoft.EntityFrameworkCore;
using WebshopService.Data;
using WebshopService.DTOs.Requests;
using WebshopService.Exceptions;
using WebshopService.Models;
using WebshopService.Repositories.Interface;

namespace WebshopService.Repositories.Implementation;

public class CategoryRepository(WebshopDbContext context) : ICategoryRepository
{
    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await context.Categories
            .Include(c => c.ProductCategories)
            .ToListAsync();
    }
    
    public Task<Category> GetByIdAsync(int id)
    {
        var existingCategory = context.Categories.FirstOrDefault(p => p.Id == id);
        
        if (existingCategory == null)
        {
            throw new CategoryNotFoundException($"Product with id '{id}' was not found.");
        }
        
        return Task.FromResult(existingCategory);
    }
    
    public async Task AddAsync(Category category)
    {
        context.Categories.Add(category);
        
        await context.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(Category existingCategory, CategoryRequest category)
    {
        context.Entry(existingCategory).CurrentValues.SetValues(category);
        
        await context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(int id)
    {
        var category = await context.Categories.FindAsync(id);
        
        if (category != null)
        {
            context.Categories.Remove(category);
            
            await context.SaveChangesAsync();
        }
    }
}