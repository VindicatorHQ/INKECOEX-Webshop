namespace WebshopFrontend.DTOs.Responses;

public class OrderItemResponse
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal PriceAtOrder { get; set; }
    public int Quantity { get; set; }
    public decimal Subtotal => PriceAtOrder * Quantity;
}