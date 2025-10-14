namespace WebshopFrontend.DTOs.Responses;

public class ProductResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    
    public List<int> CategoryIds { get; set; } = []; 
    
    public string CategoryNames { get; set; } = string.Empty; 
}