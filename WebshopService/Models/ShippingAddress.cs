namespace WebshopService.Models;

public class ShippingAddress
{
    public int Id { get; set; }
    
    public int OrderId { get; set; }
    public Order? Order { get; set; }

    public string Street { get; set; } = string.Empty;
    public string HouseNumber { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    
    public UserProfile? DefaultUserProfile { get; set; }
}