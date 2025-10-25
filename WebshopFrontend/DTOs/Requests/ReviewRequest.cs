using System.ComponentModel.DataAnnotations;

namespace WebshopFrontend.DTOs.Requests;

public class ReviewRequest
{
    public string? ReviewerName { get; set; }
    [Required(ErrorMessage = "You can't post an empty Review!")]
    public string Comment { get; set; } = string.Empty;
    
    [Range(1, 5)]
    public int Rating { get; set; } = 5;
    
    public int? ProductId { get; set; }
    public int? GuideId { get; set; }
}