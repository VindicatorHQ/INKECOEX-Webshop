using System.ComponentModel.DataAnnotations;

namespace WebshopService.DTOs.Requests;

public class GuideRequest
{
    public int? Id { get; set; }

    [Required]
    [StringLength(256, MinimumLength = 5)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    public string Content { get; set; } = string.Empty;
    
    public string? Slug { get; set; } 
}