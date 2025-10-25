namespace WebshopFrontend.Components.Pages.Admin.Products;

public partial class ProductManagement : ProductBase
{
    private string ErrorMessage { get; set; } = string.Empty;

    private async Task DeleteProduct(int id)
    {
        var success = await ProductAgent.DeleteAsync(id);
        
        if (success)
        {
            Products.RemoveAll(p => p.Id == id);
            
            ErrorMessage = string.Empty;
        }
        else
        {
            ErrorMessage = "Something went wrong while deleting the product";
        }
    }

    private void NavigateToProductCreatePage()
    {
        NavigationManager.NavigateTo("/admin/products/create");
    }

    private void NavigateToProductEditPage(int id)
    {
        NavigationManager.NavigateTo($"/admin/products/edit/{id}");
    }
}