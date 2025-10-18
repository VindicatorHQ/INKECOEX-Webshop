namespace WebshopService.DTOs.Responses;

public class OrderDetailResponse
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    
    public List<OrderItemResponse> Items { get; set; } = [];
    
    public string FullName { get; set; } = string.Empty;
    public string AddressLine1 { get; set; } = string.Empty;
    public string AddressLine2 { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}