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

            return order.Id;
        }
        catch
        {
            await transaction.RollbackAsync();
            
            throw; 
        }
    }

    public Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Order?> GetOrderByIdAsync(int orderId)
    {
        throw new NotImplementedException();
    }
}