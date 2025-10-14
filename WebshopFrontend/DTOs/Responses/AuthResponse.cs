namespace WebshopFrontend.DTOs.Responses;

public record AuthResponse(string Token, DateTime Expiration);