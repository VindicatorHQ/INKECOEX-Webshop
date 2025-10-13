using Microsoft.AspNetCore.Identity;

namespace WebshopService.Models;

public class ShoppingCart
{
    public int Id { get; set; }
    public string UserId { get; set; }

    public IdentityUser User { get; set; }
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}