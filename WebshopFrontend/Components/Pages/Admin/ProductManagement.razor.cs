using Microsoft.AspNetCore.Components;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs.Responses;
using WebshopFrontend.Services;

namespace WebshopFrontend.Components.Pages.Admin;

public partial class ProductManagement(IProductAgent productAgent, IDebounceService debounceService) : ComponentBase, IDisposable
{
    private readonly object _debounceKey = new();
    private SearchModel searchModel = new();
    private List<ProductResponse> products = [];
    private string ErrorMessage { get; set; } = string.Empty;
    private bool isLoading = true;
    
    private class SearchModel
    {
        public string? SearchTerm { get; set; }
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadProducts();
        
        isLoading = false;
    }

    private async Task DeleteProduct(int id)
    {
        var success = await productAgent.DeleteAsync(id);
        
        if (success)
        {
            products.RemoveAll(p => p.Id == id);
            
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
    
    private async Task LoadProducts()
    {
        products = await productAgent.GetAllProductsAsync(searchModel.SearchTerm);
        
        StateHasChanged(); 
    }

    private async Task HandleSearch()
    {
        await LoadProducts();
    }
    
    private async Task ClearSearch()
    {
        searchModel.SearchTerm = null;
        
        debounceService.Cancel(_debounceKey);
        
        await LoadProducts();
    }
    
    private void HandleInput(ChangeEventArgs e)
    {
        searchModel.SearchTerm = e.Value?.ToString();
        
        debounceService.Debounce(300, SearchAction, _debounceKey);
    }

    private void SearchAction()
    {
        InvokeAsync(LoadProducts);
    }

    public void Dispose()
    {
        debounceService.Cancel(_debounceKey);
    }
}