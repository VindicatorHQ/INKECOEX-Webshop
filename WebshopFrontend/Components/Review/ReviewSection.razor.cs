using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs.Requests;
using WebshopFrontend.DTOs.Responses;
using WebshopFrontend.Providers;
using Microsoft.AspNetCore.Components.Authorization;

namespace WebshopFrontend.Components.Review;

public partial class ReviewSection(IReviewAgent reviewAgent) : ComponentBase
{
    [Inject] 
    public AuthenticationStateProvider AuthStateProvider { get; set; } = default!;

    [Parameter]
    public int ItemId { get; set; } 

    [Parameter]
    public string ItemType { get; set; } = "product"; 

    private List<ReviewResponse> Reviews { get; set; } = [];
    private bool IsLoading { get; set; } = true;
    private bool IsCreating { get; set; }
    private bool IsEditing { get; set; }
    private int EditingReviewId { get; set; }

    private string? Message { get; set; }
    private bool IsError { get; set; }

    private string? CurrentUserId { get; set; }
    private bool IsAdmin { get; set; }
    
    // --- Modal State ---
    private bool ShowModal { get; set; }
    private string ModalMessage { get; set; } = string.Empty;
    private TaskCompletionSource<bool> ModalTcs { get; set; } = new();
    
    protected override async Task OnInitializedAsync()
    {
        if (AuthStateProvider is JwtAuthenticationStateProvider jwtProvider)
        {
            await jwtProvider.InitializeAsync();
        }

        var authState = await AuthStateProvider.GetAuthenticationStateAsync();
        
        CurrentUserId = authState.User.FindFirst("nameid")?.Value;
        
        IsAdmin = authState.User.IsInRole("Admin");
        
        await LoadReviewsAsync();
    }
    
    private async Task LoadReviewsAsync()
    {
        IsLoading = true;
        Reviews.Clear();

        try
        {
            if (ItemType.Equals("product", StringComparison.OrdinalIgnoreCase))
            {
                Reviews = await reviewAgent.GetProductReviewsAsync(ItemId);
            }
            else if (ItemType.Equals("guide", StringComparison.OrdinalIgnoreCase))
            {
                Reviews = await reviewAgent.GetGuideReviewsAsync(ItemId);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }
    
    private void ResetEditState()
    {
        IsEditing = false;
        EditingReviewId = 0;
    }
    
    private async Task HandleReviewCreate(ReviewRequest request)
    {
        IsError = false;
        Message = null;
        
        if (ItemType.Equals("product", StringComparison.OrdinalIgnoreCase))
        {
            request.ProductId = ItemId;
        }
        else if (ItemType.Equals("guide", StringComparison.OrdinalIgnoreCase))
        {
            request.GuideId = ItemId;
        }

        var newReview = await reviewAgent.CreateReviewAsync(request);

        if (newReview != null)
        {
            IsCreating = false;
            
            Message = "Beoordeling succesvol geplaatst!";
            
            await LoadReviewsAsync();
        }
        else
        {
            IsError = true;
            Message = "Er is een fout opgetreden bij het plaatsen van de beoordeling. Probeer het later opnieuw. (Mogelijk te lange/korte velden).";
        }
    }
    
    private async Task HandleReviewUpdate(ReviewRequest request)
    {
        IsError = false;
        Message = null;
        
        var updatedReview = await reviewAgent.UpdateReviewAsync(EditingReviewId, request);

        if (updatedReview != null)
        {
            ResetEditState();
            
            Message = "Beoordeling succesvol bijgewerkt!";
            
            await LoadReviewsAsync();
        }
        else
        {
            IsError = true;
            Message = "Fout bij het bijwerken van de beoordeling. Controleer uw rechten of probeer het opnieuw.";
        }
    }

    private void HandleEditRequest(int reviewId)
    {
        IsCreating = false;
        IsEditing = true;
        EditingReviewId = reviewId;
    }
    
    private async Task HandleDeleteRequest(int reviewId)
    {
        IsError = false;
        Message = null;

        var confirmed = await ShowConfirmation("Weet u zeker dat u deze beoordeling wilt verwijderen?");
        
        if (confirmed)
        {
            var success = await reviewAgent.DeleteReviewAsync(reviewId);
            
            if (success)
            {
                Message = "Beoordeling succesvol verwijderd.";
                
                await LoadReviewsAsync();
            }
            else
            {
                IsError = true;
                Message = "Fout bij het verwijderen. U heeft waarschijnlijk geen rechten voor deze beoordeling.";
            }
        }
    }
    
    private Task<bool> ShowConfirmation(string message)
    {
        ModalMessage = message;
        ShowModal = true;
        ModalTcs = new TaskCompletionSource<bool>();
        
        StateHasChanged();
        
        return ModalTcs.Task;
    }
    
    private void OnModalResult(bool result)
    {
        ShowModal = false;
        
        ModalTcs.SetResult(result); 
        
        StateHasChanged();
    }
}
