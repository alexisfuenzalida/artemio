using Blazored.LocalStorage;

namespace ArtemioBlazorWA.Services;

public class ThemeService
{
    private const string StorageKey = "darkMode";

    private readonly ILocalStorageService _localStorage;

    public bool IsDarkMode { get; private set; }
    public bool IsInitialized { get; private set; }

    public event Action? OnChange;

    public ThemeService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task InitializeAsync()
    {
        if (IsInitialized)
        {
            return;
        }

        IsDarkMode = await _localStorage.GetItemAsync<bool>(StorageKey);
        IsInitialized = true;
        OnChange?.Invoke();
    }

    public async Task ToggleAsync()
    {
        IsDarkMode = !IsDarkMode;
        await _localStorage.SetItemAsync(StorageKey, IsDarkMode);
        OnChange?.Invoke();
    }
}
