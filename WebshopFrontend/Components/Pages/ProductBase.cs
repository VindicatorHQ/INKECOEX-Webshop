using Microsoft.AspNetCore.Components;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs;
using WebshopFrontend.DTOs.Responses;
using WebshopFrontend.Services;

namespace WebshopFrontend.Components.Pages;

public abstract class ProductBase : ComponentBase, IDisposable
{
    [Inject] protected IProductAgent ProductAgent { get; set; } = default!;
    [Inject] protected ICategoryAgent CategoryAgent { get; set; } = default!;
    [Inject] protected IDebounceService DebounceService { get; set; } = default!;
    [Inject] protected NavigationManager NavigationManager { get; set; } = default!;

    private readonly object _debounceKey = new();
    protected SearchModel SearchModel = new();
    protected List<ProductResponse> Products = [];
    protected List<CategoryResponse> Categories = [];
    protected bool IsLoading = true;
    
    [SupplyParameterFromQuery(Name = "category")]
    public string? CategorySlugFromUrl { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await InitialLoadAsync();
        
        IsLoading = false;
    }
    
    protected async Task InitialLoadAsync()
    {
        Categories = await CategoryAgent.GetAllCategoriesAsync();
        
        await LoadProducts();
    }
    
    protected override async Task OnParametersSetAsync()
    {
        var normalizedSlug = CategorySlugFromUrl?.ToLowerInvariant();

        if (SearchModel.CategorySlug != normalizedSlug)
        {
            SearchModel.CategorySlug = normalizedSlug;
            
            await LoadProducts();
            
            if (IsLoading)
            {
                IsLoading = false;
            }
        }
    }

    protected async Task LoadProducts()
    {
        Products = await ProductAgent.GetAllProductsAsync(
            categorySlug: SearchModel.CategorySlug,
            searchTerm: SearchModel.SearchTerm
        );
        
        StateHasChanged(); 
    }

    protected void HandleInput(ChangeEventArgs e)
    {
        SearchModel.SearchTerm = e.Value?.ToString();
        
        DebounceService.Debounce(300, SearchAction, _debounceKey);
    }
    
    private void SearchAction()
    {
        InvokeAsync(LoadProducts);
    }

    protected async Task ClearSearch()
    {
        SearchModel.SearchTerm = null;
        SearchModel.CategorySlug = null;
        
        DebounceService.Cancel(_debounceKey);
        
        await LoadProducts();
    }
    
    protected async Task FilterByCategory(string? slug)
    {
        DebounceService.Cancel(_debounceKey);
    
        SearchModel.CategorySlug = slug;
    
        await LoadProducts();
    }

    protected void NavigateToProductPage(int id)
    {
        NavigationManager.NavigateTo($"/products/{id}");
    }

    public void Dispose()
    {
        DebounceService.Cancel(_debounceKey);
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) {}
}
