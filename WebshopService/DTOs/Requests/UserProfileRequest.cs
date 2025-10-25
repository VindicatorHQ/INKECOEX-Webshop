namespace WebshopService.DTOs.Requests;

public class UserProfileRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public string? FullName { get; set; }
    public string? Street { get; set; }
    public string? HouseNumber { get; set; }
    public string? ZipCode { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
}