namespace WebshopService.DTOs.Requests;

public record AddToCartRequest(int ProductId, int Quantity);