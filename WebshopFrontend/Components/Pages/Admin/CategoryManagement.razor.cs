using Microsoft.AspNetCore.Components;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs.Requests;
using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Components.Pages.Admin;

public partial class CategoryManagement(ICategoryAgent categoryAgent) : ComponentBase
{
    private List<CategoryResponse>? categories;
    private CategoryRequest editingCategory = new();
    private bool statusSuccess;
    private string? statusMessage;

    protected override async Task OnInitializedAsync()
    {
        await LoadCategories();
    }

    private async Task LoadCategories()
    {
        categories = await categoryAgent.GetAllCategoriesAsync();
    }

    private void ResetForm()
    {
        editingCategory = new CategoryRequest();
        statusMessage = null;
    }
    
    private void EditCategory(CategoryResponse category)
    {
        editingCategory = new CategoryRequest 
        { 
            Id = category.Id,
            Name = category.Name, 
            Slug = category.Slug
        };
        statusMessage = null;
    }

    private async Task HandleSubmit()
    {
        statusMessage = null;
        bool success;

        if (editingCategory.Id == 0)
        {
            var newCategory = await categoryAgent.CreateCategoryAsync(editingCategory);
            
            success = newCategory != null;
            
            if (success)
            {
                statusMessage = $"Categorie '{newCategory!.Name}' succesvol toegevoegd.";
            }
        }
        else
        {
            success = await categoryAgent.UpdateCategoryAsync(editingCategory.Id, editingCategory);
            
            if (success)
            {
                statusMessage = $"Categorie '{editingCategory.Name}' succesvol bijgewerkt.";
            }
        }

        if (success)
        {
            statusSuccess = true;
            await LoadCategories();
            ResetForm();
        }
        else
        {
            statusSuccess = false;
            statusMessage ??= "Fout opgetreden bij verwerken.";
        }
    }
    
    private async Task DeleteCategory(int id)
    {
        object[] arguments = ["Weet je zeker dat je deze categorie wilt verwijderen?"];
        
        if (await JsRuntime.InvokeAsync<bool>("confirm", arguments))
        {
            var success = await categoryAgent.DeleteCategoryAsync(id);

            if (success)
            {
                statusSuccess = true;
                statusMessage = "Categorie succesvol verwijderd.";
                await LoadCategories();
            }
            else
            {
                statusSuccess = false;
                statusMessage = "Fout bij verwijderen. Mogelijk zijn er nog producten gekoppeld.";
            }
        }
    }
}