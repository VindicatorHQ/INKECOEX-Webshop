namespace WebshopService.Models;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal PriceAtOrder { get; set; }
    public decimal Subtotal => PriceAtOrder * Quantity;

    public Order Order { get; set; }
    public Product Product { get; set; }
}