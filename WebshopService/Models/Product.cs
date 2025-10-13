namespace WebshopService.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
}