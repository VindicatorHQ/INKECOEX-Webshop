namespace WebshopService.DTOs.Responses;

public class ReviewResponse
{
    public int Id { get; set; }
    public string ReviewerName { get; set; } = "Anoniem";
    public string Comment { get; set; } = string.Empty;
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public string? UserId { get; set; }
}