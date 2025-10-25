namespace WebshopService.Models;

public class Review
{
    public int Id { get; set; }
    
    public string Comment { get; set; } = string.Empty;
    
    public int Rating { get; set; } 
    
    public string ReviewerName { get; set; } = string.Empty;
    
    public string? UserId { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int? ProductId { get; set; }
    public Product? Product { get; set; }
    
    public int? GuideId { get; set; }
    public Guide? Guide { get; set; }
}