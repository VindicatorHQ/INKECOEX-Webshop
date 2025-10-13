namespace WebshopService.DTOs.Requests;

public class ProductCreateRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public List<int> CategoryIds { get; set; } = new();
}