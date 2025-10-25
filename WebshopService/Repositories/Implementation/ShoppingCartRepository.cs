using Microsoft.EntityFrameworkCore;
using WebshopService.Data;
using WebshopService.Models;
using WebshopService.Repositories.Interface;

namespace WebshopService.Repositories.Implementation;

public class ShoppingCartRepository(WebshopDbContext context) : IShoppingCartRepository
{
    public async Task<ShoppingCart?> GetByUserIdAsync(string userId)
    {
        return await context.ShoppingCarts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task SaveAsync(ShoppingCart cart)
    {
        if (cart.Id == 0)
        {
            context.ShoppingCarts.Add(cart);
        }
        else
        {
            context.ShoppingCarts.Update(cart);
        }

        await context.SaveChangesAsync();
    }
}