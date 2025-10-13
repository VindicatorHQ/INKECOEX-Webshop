namespace WebshopFrontend.DTOs;

public record AuthResponse(string Token, DateTime Expiration);