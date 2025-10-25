namespace WebshopService.DTOs.Requests;

public class ReviewRequest
{
    public string? ReviewerName { get; set; }
    
    public string Comment { get; set; } = string.Empty;
    
    public int Rating { get; set; }
    
    public int? ProductId { get; set; }
    public int? GuideId { get; set; }
}