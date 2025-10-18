using Microsoft.AspNetCore.Components;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs.Responses;
using WebshopFrontend.Services;

namespace WebshopFrontend.Components.Pages.Products;

public partial class ProductCatalog(IProductAgent productAgent, IDebounceService debounceService) : ComponentBase, IDisposable
{
    private readonly object _debounceKey = new();
    private List<ProductResponse> products = [];
    private bool isLoading = true;
    private SearchModel searchModel = new();
    
    private class SearchModel
    {
        public string? SearchTerm { get; set; }
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadProducts();
        
        isLoading = false;
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

    private void NavigateToProductPage(int id)
    {
        NavigationManager.NavigateTo($"/products/{id}");
    }

    public void Dispose()
    {
        debounceService.Cancel(_debounceKey);
    }
}