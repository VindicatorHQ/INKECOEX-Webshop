namespace WebshopService.DTOs.Requests;

public class CategoryRequest
{
    public int? Id { get; set; }
    public string Name { get; set; } = string.Empty;
}