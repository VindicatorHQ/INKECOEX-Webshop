namespace WebshopService.DTOs;

public record AuthResponse(string Token, DateTime Expiration);