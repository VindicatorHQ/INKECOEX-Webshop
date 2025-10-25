using System.ComponentModel.DataAnnotations;

namespace WebshopFrontend.DTOs.Requests;

public class GuideRequest
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "Titel is verplicht.")]
    [StringLength(256, MinimumLength = 5, ErrorMessage = "Titel moet tussen 5 en 256 tekens lang zijn.")]
    public string Title { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Content is verplicht.")]
    public string Content { get; set; } = string.Empty;
}