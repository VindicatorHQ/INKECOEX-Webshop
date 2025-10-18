using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Services;

public class ShoppingCartService
{
    public ShoppingCartResponse CurrentCart { get; private set; } = new();
    
    public event Action? OnChange;

    public void SetCart(ShoppingCartResponse cart)
    {
        CurrentCart = cart;
        
        NotifyStateChanged();
    }
    
    public int GetTotalItemCount()
    {
        return CurrentCart.Items.Sum(i => i.Quantity);
    }
    
    private void NotifyStateChanged() => OnChange?.Invoke();
}