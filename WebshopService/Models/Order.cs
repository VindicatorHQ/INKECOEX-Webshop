using Microsoft.AspNetCore.Identity;

namespace WebshopService.Models;

public class Order
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "New";

    public IdentityUser User { get; set; }
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    public ShippingAddress? ShippingAddress { get; set; }
}
