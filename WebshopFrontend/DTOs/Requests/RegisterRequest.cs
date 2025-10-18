using System.ComponentModel.DataAnnotations;

namespace WebshopFrontend.DTOs.Requests;

public class RegisterRequest
{
    [Required(ErrorMessage = "E-mail is verplicht.")]
    [EmailAddress(ErrorMessage = "Ongeldig e-mailadres.")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Wachtwoord is verplicht.")]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "Wachtwoord moet minimaal 6 karakters bevatten.")]
    public string Password { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Bevestiging is verplicht.")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Wachtwoorden komen niet overeen.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}