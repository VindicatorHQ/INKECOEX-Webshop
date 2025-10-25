using Microsoft.EntityFrameworkCore;
using WebshopService.Data;
using WebshopService.Models;
using WebshopService.Repositories.Interface;

namespace WebshopService.Repositories.Implementation;

public class OrderRepository(WebshopDbContext context, IShoppingCartRepository cartRepository, IProductRepository productRepository) : IOrderRepository
{
    public async Task<int> PlaceOrderAsync(string userId, ShippingAddress address)
    {
        using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            var cart = await cartRepository.GetByUserIdAsync(userId);
            
            if (cart == null || cart.CartItems.Count == 0)
            {
                throw new InvalidOperationException("Winkelwagen is leeg of niet gevonden.");
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                ShippingAddress = address,
                Status = "New"
            };
            
            decimal totalAmount = 0;

            foreach (var cartItem in cart.CartItems)
            {
                var product = await productRepository.GetByIdAsync(cartItem.ProductId);
                
                if (product == null || product.StockQuantity < cartItem.Quantity)
                {
                    throw new InvalidOperationException($"Onvoldoende voorraad voor product: {cartItem.ProductId}");
                }

                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    PriceAtOrder = product.Price,
                    Quantity = cartItem.Quantity,
                };
                order.Items.Add(orderItem);
                
                totalAmount += orderItem.Subtotal;
                
                product.StockQuantity -= cartItem.Quantity;
                context.Update(product); 
            }
            
            order.TotalAmount = totalAmount;

            context.Orders.Add(order);
            
            context.ShoppingCarts.Remove(cart);

            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            var profile = context.UserProfiles.FirstOrDefault(up => up.UserId == userId);

            if (profile == null)
            {
                profile = new UserProfile { UserId = userId };
                
                context.UserProfiles.Add(profile);
            }

            var userNames = address.FullName.Split([" "], StringSplitOptions.None);
            
            var updatedProfile = new UserProfile
            {
                UserId = profile.UserId,
                FirstName = userNames.First(),
                LastName = userNames.Last(),
                DefaultShippingAddressId = address.Id,
            };
            
            context.Entry(profile).CurrentValues.SetValues(updatedProfile);

            await context.SaveChangesAsync();

            return order.Id;
        }
        catch
        {
            await transaction.RollbackAsync();
            
            throw; 
        }
    }
    
    public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
    {
        return await context.Orders
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .Include(o => o.Items)
            .ToListAsync();
    }
    
    public async Task<Order?> GetOrderDetailAsync(int orderId, string userId)
    {
        return await context.Orders
            .Where(o => o.Id == orderId && o.UserId == userId)
            .Include(o => o.Items)
            .Include(o => o.ShippingAddress)
            .FirstOrDefaultAsync();
    }
    
    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await context.Orders
            .OrderByDescending(o => o.OrderDate)
            .Include(o => o.Items)
            .ToListAsync();
    }
    
    public async Task<Order?> GetOrderDetailsByIdAsync(int orderId)
    {
        return await context.Orders
            .Where(o => o.Id == orderId)
            .Include(o => o.Items)
            .Include(o => o.ShippingAddress)
            .FirstOrDefaultAsync();
    }
    
    public async Task UpdateOrderStatusAsync(int orderId, string newStatus)
    {
        var order = await context.Orders.FindAsync(orderId);
        
        if (order == null)
        {
            throw new KeyNotFoundException($"Order met ID {orderId} niet gevonden.");
        }
        
        order.Status = newStatus;
        await context.SaveChangesAsync();
    }
}