namespace WebshopFrontend.DTOs.Responses;

public class ShoppingCartResponse
{
    public List<CartItemResponse> Items { get; set; } = [];
    public decimal TotalPrice => Items.Sum(i => i.Subtotal);
}