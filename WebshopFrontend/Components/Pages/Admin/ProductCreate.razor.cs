using Microsoft.AspNetCore.Components;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs.Requests;

namespace WebshopFrontend.Components.Pages.Admin;

public partial class ProductCreate(IProductAgent productAgent) : ComponentBase
{
    private ProductRequest ProductRequest = new();
    private bool ShowError;

    private async Task HandleCreate()
    {
        ShowError = false;
        
        var success = await productAgent.CreateAsync(ProductRequest);

        if (success)
        {
            NavigationManager.NavigateTo("/admin/products");
        }
        else
        {
            ShowError = true;
        }
    }
}