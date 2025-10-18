using Blazored.SessionStorage;
using Microsoft.JSInterop;

namespace WebshopFrontend.Services;

public class ThemeService(IJSRuntime jsRuntime, ISessionStorageService localStorage) : IThemeService
{
    private const string LocalStorageKey = "theme_preference";

    public event Action? OnThemeChanged;
    public string CurrentTheme { get; private set; } = "light";

    public async Task InitializeThemeAsync()
    {
        try
        {
            var savedTheme = await localStorage.GetItemAsStringAsync(LocalStorageKey);
            
            if (savedTheme is "dark" or "light")
            {
                CurrentTheme = savedTheme;
            }
            
            await jsRuntime.InvokeVoidAsync("setBodyTheme", CurrentTheme);
            
            OnThemeChanged?.Invoke();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Theme init error: {ex.Message}");
        }
    }

    public async Task ToggleThemeAsync()
    {
        CurrentTheme = CurrentTheme == "light" ? "dark" : "light";

        await localStorage.SetItemAsStringAsync(LocalStorageKey, CurrentTheme);
        
        await jsRuntime.InvokeVoidAsync("setBodyTheme", CurrentTheme);
        
        OnThemeChanged?.Invoke();
    }
}